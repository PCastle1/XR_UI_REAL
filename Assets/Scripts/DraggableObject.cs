using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DraggableObject : MonoBehaviour
{
    public LayerMask placeLayer; // 배치 감지를 위한 레이어 마스크
    public GameObject previewPrefab; // 배치 미리보기 프리팹
    public GameObject Controller; // VR 컨트롤러에 대한 참조
    public GameObject Haaand; // VR 손 오브젝트에 대한 참조
    public GameObject Shop; // 상점 오브젝트에 대한 참조 (현재 사용되지 않음)
    public Material redMaterial; // 배치가 불가능할 때 적용할 적색 Material
    public Material greenMaterial; // 배치가 가능할 때 적용할 녹색 Material

    private GameObject objectInHand; // 현재 손에 들고 있는 오브젝트
    private GameObject previewObject; // 배치 미리보기 오브젝트
    private bool isCarrying = false; // 오브젝트를 들고 있는지 여부를 나타내는 플래그
    private InputDevice leftController; // 왼쪽 컨트롤러 입력 장치
    private InputDevice rightController; // 오른쪽 컨트롤러 입력 장치
    private bool isRTriggerPressed = false; // 오른쪽 트리거 버튼이 눌렸는지 여부
    private bool prevRTriggerPressed = false; // 이전 프레임에서 오른쪽 트리거 버튼 상태
    private bool isRBbuttonPressed = false; // 오른쪽 보조 버튼이 눌렸는지 여부
    private bool prevRBPressed = false; // 이전 프레임에서 오른쪽 보조 버튼 상태

    private Renderer previewRenderer; // 미리보기 오브젝트의 렌더러
    private PreviewObjectScript previewScript; // 미리보기 오브젝트의 스크립트

    private Vector3 originalScale; // 오브젝트의 원래 크기
    private Vector3 scaledDownScale; // 축소된 크기

    float OriginalRotate = 0f;

    void Start()
    {

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
        InputDevices.GetDevicesAtXRNode(hand, devices);
        if (devices.Count > 0)
        {
            return devices[0]; // 첫 번째 장치를 반환
        }
        return new InputDevice(); // 장치가 없으면 빈 장치 반환
    }

    void Update()
    {
        // 컨트롤러가 유효한지 확인하고 유효하지 않으면 다시 초기화
        if (!leftController.isValid || !rightController.isValid)
        {
            InitializeControllers();
        }

        // 오른쪽 트리거 버튼 상태 확인
        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed);

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            HandleTriggerPress(); // 트리거 눌림 처리
        }
        prevRTriggerPressed = isRTriggerPressed; // 이전 트리거 상태 업데이트

        if (isCarrying)
        {
            UpdatePreview(); // 오브젝트를 들고 있는 경우 미리보기 위치 업데이트
        }

        // 오른쪽 보조 버튼 상태 확인
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isRBbuttonPressed);
        if (isCarrying && isRBbuttonPressed && !prevRBPressed)
        {
            RotateObject(); // 오브젝트를 들고 있는 경우 회전 처리
        }
        prevRBPressed = isRBbuttonPressed; // 이전 버튼 상태 업데이트
    }

    void HandleTriggerPress()
    {
        if (isCarrying)
        {
            if (previewScript != null && !previewScript.isColliding) // 충돌하지 않을 때만 배치
            {
                RaycastHit hit;
                if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity, placeLayer))
                {
                    Debug.Log("Raycast hit at position: " + hit.point);
                    if (objectInHand != null)
                    {
                        objectInHand.transform.localScale = originalScale; // 원래 크기로 복원
                        Vector3 newPosition = hit.point;
                        objectInHand.transform.position = newPosition; // 오브젝트를 히트 위치로 이동
                        isCarrying = false; // 오브젝트를 들고 있는 상태 해제
                        Destroy(previewObject); // 미리보기 오브젝트 제거
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
                if (hit.collider.gameObject == gameObject) // 현재 게임 오브젝트를 맞췄는지 확인
                {
                    Debug.Log("Hit gameobject");
                    isCarrying = true; // 오브젝트를 들기 시작

                    // 오브젝트를 손에 들기 설정
                    objectInHand = gameObject;
                    this.transform.position = Haaand.transform.position;
                    this.transform.rotation = Quaternion.Euler(0, OriginalRotate, 0);

                    // 원래 크기를 저장하고 축소된 크기 설정
                    originalScale = objectInHand.transform.localScale;
                    scaledDownScale = originalScale * 0.5f; // 절반으로 축소
                    objectInHand.transform.localScale = scaledDownScale; // 축소된 크기 적용

                    // 배치 미리보기 생성
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
        previewObject = Instantiate(previewPrefab); // 미리보기 프리팹 생성
        previewObject.transform.position = transform.position; // 미리보기 위치를 현재 위치로 설정
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
                Vector3 newPosition = hit.point;
                previewObject.transform.position = newPosition; // 레이캐스트 히트 위치로 미리보기 위치 업데이트
                previewObject.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f);
            }
        }
    }
}
