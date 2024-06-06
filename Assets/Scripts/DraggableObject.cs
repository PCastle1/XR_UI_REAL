using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DraggableObject : MonoBehaviour
{
    public LayerMask placeLayer;
    public GameObject previewPrefab; // 설치 미리보기 프리팹
    public GameObject Controller;
    public Material redMaterial; // 충돌 시 적용할 적색 Material
    public Material greenMaterial; // 충돌 해제 시 적용할 녹색 Material

    private GameObject objectInHand; // 현재 손에 들린 오브젝트
    private GameObject previewObject; // 설치 미리보기 객체
    private bool isCarrying = false;
    private InputDevice leftController;
    private InputDevice rightController;
    private bool isRTriggerPressed = false;
    private bool prevRTriggerPressed = false;
    private bool isRBbuttonPressed = false;
    private bool prevRBPressed = false;

    private Renderer previewRenderer;
    private PreviewObjectScript previewScript; // 미리보기 오브젝트의 스크립트 참조

    private Vector3 originalScale; // 오브젝트의 원래 크기
    private Vector3 scaledDownScale; // 줄인 크기

    void Start()
    {
        InitializeControllers();
    }

    void InitializeControllers()
    {
        leftController = GetController(XRNode.LeftHand);
        rightController = GetController(XRNode.RightHand);
    }

    InputDevice GetController(XRNode hand)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(hand, devices);
        if (devices.Count > 0)
        {
            return devices[0];
        }
        return new InputDevice();
    }

    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            InitializeControllers();
        }

        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed);

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            HandleTriggerPress();
        }
        prevRTriggerPressed = isRTriggerPressed;

        if (isCarrying)
        {
            UpdatePreview();
        }

        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isRBbuttonPressed);
        if (isCarrying && isRBbuttonPressed && !prevRBPressed)
        {
            RotateObject();
        }
        prevRBPressed = isRBbuttonPressed;
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
                        this.transform.SetParent(null);
                        Vector3 newPosition = hit.point + new Vector3(0, this.transform.localScale.y/3, 0);
                        Debug.Log("Moving object to position: " + newPosition);
                        objectInHand.transform.position = newPosition;
                        isCarrying = false; // 이동 후 손에 들고 있는 상태를 해제합니다.
                        objectInHand.transform.localScale = originalScale; // 크기를 원래대로 돌립니다.
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
                    this.transform.SetParent(Controller.transform);

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
            objectInHand.transform.Rotate(0f, 90f, 0f, Space.World); // 오브젝트를 Y 축을 기준으로 90도 회전합니다.
            if (previewObject != null)
            {
                previewObject.transform.Rotate(0f, 90f, 0f, Space.World); // 미리보기 오브젝트도 동일하게 회전시킵니다.
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
                Vector3 newPosition = hit.point + new Vector3(0, previewObject.transform.localScale.y / 3, 0);
                previewObject.transform.position = newPosition;
            }
        }
    }
}
