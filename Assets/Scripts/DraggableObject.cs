using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DraggableObject : MonoBehaviour
{
    public LayerMask placeLayer; // ��ġ ������ ���� ���̾� ����ũ
    public GameObject previewPrefab; // ��ġ �̸����� ������
    public GameObject Controller; // VR ��Ʈ�ѷ��� ���� ����
    public GameObject Haaand; // VR �� ������Ʈ�� ���� ����
    public GameObject Shop; // ���� ������Ʈ�� ���� ���� (���� ������ ����)
    public Material redMaterial; // ��ġ�� �Ұ����� �� ������ ���� Material
    public Material greenMaterial; // ��ġ�� ������ �� ������ ��� Material

    private GameObject objectInHand; // ���� �տ� ��� �ִ� ������Ʈ
    private GameObject previewObject; // ��ġ �̸����� ������Ʈ
    private bool isCarrying = false; // ������Ʈ�� ��� �ִ��� ���θ� ��Ÿ���� �÷���
    private InputDevice leftController; // ���� ��Ʈ�ѷ� �Է� ��ġ
    private InputDevice rightController; // ������ ��Ʈ�ѷ� �Է� ��ġ
    private bool isRTriggerPressed = false; // ������ Ʈ���� ��ư�� ���ȴ��� ����
    private bool prevRTriggerPressed = false; // ���� �����ӿ��� ������ Ʈ���� ��ư ����
    private bool isRBbuttonPressed = false; // ������ ���� ��ư�� ���ȴ��� ����
    private bool prevRBPressed = false; // ���� �����ӿ��� ������ ���� ��ư ����

    private Renderer previewRenderer; // �̸����� ������Ʈ�� ������
    private PreviewObjectScript previewScript; // �̸����� ������Ʈ�� ��ũ��Ʈ

    private Vector3 originalScale; // ������Ʈ�� ���� ũ��
    private Vector3 scaledDownScale; // ��ҵ� ũ��

    float OriginalRotate = 0f;

    void Start()
    {

        InitializeControllers(); // ��Ʈ�ѷ� �ʱ�ȭ
    }

    void InitializeControllers()
    {
        leftController = GetController(XRNode.LeftHand); // ���� ��Ʈ�ѷ� ��������
        rightController = GetController(XRNode.RightHand); // ������ ��Ʈ�ѷ� ��������
    }

    InputDevice GetController(XRNode hand)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(hand, devices);
        if (devices.Count > 0)
        {
            return devices[0]; // ù ��° ��ġ�� ��ȯ
        }
        return new InputDevice(); // ��ġ�� ������ �� ��ġ ��ȯ
    }

    void Update()
    {
        // ��Ʈ�ѷ��� ��ȿ���� Ȯ���ϰ� ��ȿ���� ������ �ٽ� �ʱ�ȭ
        if (!leftController.isValid || !rightController.isValid)
        {
            InitializeControllers();
        }

        // ������ Ʈ���� ��ư ���� Ȯ��
        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed);

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            HandleTriggerPress(); // Ʈ���� ���� ó��
        }
        prevRTriggerPressed = isRTriggerPressed; // ���� Ʈ���� ���� ������Ʈ

        if (isCarrying)
        {
            UpdatePreview(); // ������Ʈ�� ��� �ִ� ��� �̸����� ��ġ ������Ʈ
        }

        // ������ ���� ��ư ���� Ȯ��
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isRBbuttonPressed);
        if (isCarrying && isRBbuttonPressed && !prevRBPressed)
        {
            RotateObject(); // ������Ʈ�� ��� �ִ� ��� ȸ�� ó��
        }
        prevRBPressed = isRBbuttonPressed; // ���� ��ư ���� ������Ʈ
    }

    void HandleTriggerPress()
    {
        if (isCarrying)
        {
            if (previewScript != null && !previewScript.isColliding) // �浹���� ���� ���� ��ġ
            {
                RaycastHit hit;
                if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity, placeLayer))
                {
                    Debug.Log("Raycast hit at position: " + hit.point);
                    if (objectInHand != null)
                    {
                        objectInHand.transform.localScale = originalScale; // ���� ũ��� ����
                        Vector3 newPosition = hit.point;
                        objectInHand.transform.position = newPosition; // ������Ʈ�� ��Ʈ ��ġ�� �̵�
                        isCarrying = false; // ������Ʈ�� ��� �ִ� ���� ����
                        Destroy(previewObject); // �̸����� ������Ʈ ����
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
                if (hit.collider.gameObject == gameObject) // ���� ���� ������Ʈ�� ������� Ȯ��
                {
                    Debug.Log("Hit gameobject");
                    isCarrying = true; // ������Ʈ�� ��� ����

                    // ������Ʈ�� �տ� ��� ����
                    objectInHand = gameObject;
                    this.transform.position = Haaand.transform.position;
                    this.transform.rotation = Quaternion.Euler(0, OriginalRotate, 0);

                    // ���� ũ�⸦ �����ϰ� ��ҵ� ũ�� ����
                    originalScale = objectInHand.transform.localScale;
                    scaledDownScale = originalScale * 0.5f; // �������� ���
                    objectInHand.transform.localScale = scaledDownScale; // ��ҵ� ũ�� ����

                    // ��ġ �̸����� ����
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
            objectInHand.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f); // ������Ʈ�� Y ���� �������� 90�� ȸ���մϴ�.
            if (previewObject != null)
            {
                previewObject.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f); // �̸����� ������Ʈ�� �����ϰ� ȸ����ŵ�ϴ�.
            }
        }
    }

    void CreatePreview()
    {
        previewObject = Instantiate(previewPrefab); // �̸����� ������ ����
        previewObject.transform.position = transform.position; // �̸����� ��ġ�� ���� ��ġ�� ����
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
                Vector3 newPosition = hit.point;
                previewObject.transform.position = newPosition; // ����ĳ��Ʈ ��Ʈ ��ġ�� �̸����� ��ġ ������Ʈ
                previewObject.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f);
            }
        }
    }
}
