using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DraggableObject1 : MonoBehaviour
{
    public LayerMask placeLayer;
    public GameObject previewPrefab; // ��ġ �̸����� ������
    public GameObject Controller;
    public GameObject Haaand;
    public GameObject Shop;
    public Material redMaterial; // �浹 �� ������ ���� Material
    public Material greenMaterial; // �浹 ���� �� ������ ��� Material

    private GameObject objectInHand; // ���� �տ� �鸰 ������Ʈ
    private GameObject previewObject; // ��ġ �̸����� ��ü
    private bool isCarrying = false;
    private InputDevice leftController;
    private InputDevice rightController;
    private bool isRTriggerPressed = false;
    private bool prevRTriggerPressed = false;
    private bool isRBbuttonPressed = false;
    private bool prevRBPressed = false;

    private Renderer previewRenderer;
    private PreviewObjectScript previewScript; // �̸����� ������Ʈ�� ��ũ��Ʈ ����

    private Vector3 originalScale; // ������Ʈ�� ���� ũ��
    private Vector3 scaledDownScale; // ���� ũ��

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
            if (previewScript != null && !previewScript.isColliding) // �浹 ���� �ƴ� ���� ��ġ
            {
                RaycastHit hit;
                if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity, placeLayer))
                {
                    Debug.Log("Raycast hit at position: " + hit.point);
                    if (objectInHand != null)
                    {
                        Debug.Log(hit.point);
                        objectInHand.transform.localScale = originalScale; // ũ�⸦ ������� �����ϴ�.
                        Vector3 newPosition = hit.point + new Vector3(0, previewObject.transform.localScale.y, 0);
                        Debug.Log("Moving object to position: " + newPosition);
                        objectInHand.transform.position = newPosition;
                        isCarrying = false; // �̵� �� �տ� ��� �ִ� ���¸� �����մϴ�.
                        Destroy(previewObject); // ��ġ �̸����� ����
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
                if (hit.collider.gameObject == gameObject) // ����: �浹 �������� Ȯ��
                {
                    Debug.Log("Hit gameobject");
                    isCarrying = true;

                    // �տ� ������Ʈ�� �鵵�� �����մϴ�.
                    objectInHand = gameObject;
                    this.transform.position = Haaand.transform.position;

                    // ���� ũ�⸦ �����ϰ� ���� ũ�⸦ �����մϴ�.
                    originalScale = objectInHand.transform.localScale;
                    scaledDownScale = originalScale * 0.5f; // ũ�⸦ �������� ���Դϴ�.
                    objectInHand.transform.localScale = scaledDownScale; // ������Ʈ�� ũ�⸦ ���Դϴ�.

                    // ��ġ �̸����⸦ �����մϴ�.
                    CreatePreview();
                }
            }
        }
    }

    void RotateObject()
    {
        if (objectInHand != null)
        {
            objectInHand.transform.Rotate(0f, 90f, 0f, Space.World); // ������Ʈ�� Y ���� �������� 90�� ȸ���մϴ�.
            if (previewObject != null)
            {
                previewObject.transform.Rotate(0f, 90f, 0f, Space.World); // �̸����� ������Ʈ�� �����ϰ� ȸ����ŵ�ϴ�.
            }
        }
    }

    void CreatePreview()
    {
        previewObject = Instantiate(previewPrefab); // �̸����� �������� �����մϴ�.
       // previewObject.transform.SetParent(Shop.transform);
        previewObject.transform.position = transform.position; // �̸����� ��ġ�� ���� ��ġ�� �����մϴ�.
        previewRenderer = previewObject.GetComponent<Renderer>(); // �̸����� ������Ʈ�� ������ ��������
        previewRenderer.material = greenMaterial; // �ʱ⿡�� ��� Material ����

        previewScript = previewObject.GetComponent<PreviewObjectScript>(); // �̸����� ������Ʈ�� ��ũ��Ʈ ��������
    }

    void UpdatePreview()
    {
        if (previewObject != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity, placeLayer))
            {
                Vector3 newPosition = hit.point + new Vector3(0, previewObject.transform.localScale.y, 0);
                previewObject.transform.position = newPosition;
            }
        }
    }
}
