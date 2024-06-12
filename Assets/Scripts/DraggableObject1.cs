using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DraggableObject1 : MonoBehaviour
{
    public LayerMask placeLayer;
    public GameObject previewPrefab; // 설치 미리보기 프리팹
    public GameObject Controller; // 컨트롤러 게임 오브젝트
    public GameObject Haaand; // 손 게임 오브젝트
    public GameObject Shop; // 상점 게임 오브젝트
    public Material redMaterial; // 충돌 시 적용할 적색 Material
    public Material greenMaterial; // 충돌 해제 시 적용할 녹색 Material

    private GameObject objectInHand; // 현재 손에 들린 오브젝트
    private GameObject previewObject; // 설치 미리보기 객체
    private bool isCarrying = false; // 오브젝트를 손에 들고 있는지 여부
    private InputDevice leftController; // 왼쪽 컨트롤러
    private InputDevice rightController; // 오른쪽 컨트롤러
    private bool isRTriggerPressed = false; // 오른쪽 트리거 버튼이 눌렸는지 여부
    private bool prevRTriggerPressed = false; // 이전 프레임의 오른쪽 트리거 버튼 눌림 여부
    private bool isRBbuttonPressed = false; // 오른쪽 B 버튼이 눌렸는지 여부
    private bool prevRBPressed = false; // 이전 프레임의 오른쪽 B 버튼 눌림 여부
    BoxCollider boxCollider;


    private Renderer previewRenderer; // 미리보기 오브젝트의 렌더러
    private PreviewObjectScript previewScript; // 미리보기 오브젝트의 스크립트 참조

    private Vector3 originalScale; // 오브젝트의 원래 크기
    private Vector3 scaledDownScale; // 줄인 크기

    float OriginalRotate = 0f;


    void Start()
    {
        OriginalRotate = 0f;
        boxCollider = GetComponent<BoxCollider>();
        InitializeControllers(); // 컨트롤러 초기화
    }

    void InitializeControllers()
    {
        leftController = GetController(XRNode.LeftHand); // 왼쪽 컨트롤러 가져오기
        rightController = GetController(XRNode.RightHand); // 오른쪽 컨트롤러 가져오기
    }

    InputDevice GetController(XRNode hand)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(hand, devices); // 특정 손에 연결된 장치 가져오기
        if (devices.Count > 0)
        {
            return devices[0]; // 장치가 있다면 첫 번째 장치 반환
        }
        return new InputDevice(); // 장치가 없다면 빈 장치 반환
    }

    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            InitializeControllers(); // 컨트롤러가 유효하지 않으면 다시 초기화
        }

        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed); // 오른쪽 트리거 버튼 상태 가져오기

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            HandleTriggerPress(); // 트리거 버튼이 눌렸다면 처리
        }
        prevRTriggerPressed = isRTriggerPressed; // 이전 프레임의 트리거 버튼 상태 업데이트

        if (isCarrying)
        {
            UpdatePreview(); // 오브젝트를 들고 있다면 미리보기 업데이트
        }

        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isRBbuttonPressed); // 오른쪽 B 버튼 상태 가져오기
        if (isCarrying && isRBbuttonPressed && !prevRBPressed)
        {
            RotateObject(); // 오브젝트를 들고 있고 B 버튼이 눌렸다면 회전
        }
        prevRBPressed = isRBbuttonPressed; // 이전 프레임의 B 버튼 상태 업데이트
    }

    void HandleTriggerPress()
    {
        if (isCarrying)
        {
            if (previewScript != null && !previewScript.isColliding) // 충돌 중이 아닐 때만 설치
            {
                RaycastHit hit;
                if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity, placeLayer))
                {
                    Debug.Log("Raycast hit at position: " + hit.point);
                    if (objectInHand != null)
                    {
                        float height = boxCollider.size.y;
                        Debug.Log(hit.point);
                        objectInHand.transform.localScale = originalScale; // 크기를 원래대로 돌립니다.
                        Vector3 newPosition = hit.point + new Vector3(0, (objectInHand.transform.localScale.y * height)/2, 0);
                        Debug.Log("Moving object to position: " + newPosition);
                        objectInHand.transform.position = newPosition;
                        isCarrying = false; // 이동 후 손에 들고 있는 상태를 해제합니다.
                        Destroy(previewObject); // 설치 미리보기 제거
                    }
                }
                else
                {
                    Debug.Log("Cannot place object here. Position is blocked.");
                }
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit))
            {
                Debug.Log("Raycast hit at position: " + hit.point);
                if (hit.collider.gameObject == gameObject) // 수정: 충돌 상태인지 확인
                {
                    Debug.Log("Hit gameobject");
                    isCarrying = true;

                    // 손에 오브젝트를 들도록 설정합니다.
                    objectInHand = gameObject;
                    this.transform.position = Haaand.transform.position;
                    this.transform.rotation = Quaternion.Euler(0, OriginalRotate, 0);
                    // 원래 크기를 저장하고 줄인 크기를 설정합니다.
                    originalScale = objectInHand.transform.localScale;
                    scaledDownScale = originalScale * 0.5f; // 크기를 절반으로 줄입니다.
                    objectInHand.transform.localScale = scaledDownScale; // 오브젝트의 크기를 줄입니다.

                    // 설치 미리보기를 생성합니다.
                    CreatePreview();
                }
            }
        }
    }

    void RotateObject()
    {

        if (objectInHand != null)
        {
            OriginalRotate += 90f;
            if (OriginalRotate >= 360)
            {
                OriginalRotate = 0;
            }
            objectInHand.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f); // 오브젝트를 Y 축을 기준으로 90도 회전합니다.
            if (previewObject != null)
            {
                previewObject.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f); // 미리보기 오브젝트도 동일하게 회전시킵니다.
            }
        }
    }

    void CreatePreview()
    {
        previewObject = Instantiate(previewPrefab); // 미리보기 프리팹을 생성합니다.
        previewObject.transform.position = transform.position; // 미리보기 위치를 현재 위치로 설정합니다.
        previewRenderer = previewObject.GetComponent<Renderer>(); // 미리보기 오브젝트의 렌더러 가져오기
        previewRenderer.material = greenMaterial; // 초기에는 녹색 Material 적용

        previewScript = previewObject.GetComponent<PreviewObjectScript>(); // 미리보기 오브젝트의 스크립트 가져오기
    }

    void UpdatePreview()
    {
        if (previewObject != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity, placeLayer))
            {
                float height = boxCollider.size.y;
                Vector3 newPosition = hit.point + new Vector3(0, (previewObject.transform.localScale.y * height) / 2, 0);
                previewObject.transform.position = newPosition;
                previewObject.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f);
            }
        }
    }
}
