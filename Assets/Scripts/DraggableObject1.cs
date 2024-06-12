using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DraggableObject1 : MonoBehaviour
{
    public LayerMask placeLayer;
    public GameObject previewPrefab; // ��ġ �̸����� ������
    public GameObject Controller; // ��Ʈ�ѷ� ���� ������Ʈ
    public GameObject Haaand; // �� ���� ������Ʈ
    public GameObject Shop; // ���� ���� ������Ʈ
    public Material redMaterial; // �浹 �� ������ ���� Material
    public Material greenMaterial; // �浹 ���� �� ������ ��� Material

    private GameObject objectInHand; // ���� �տ� �鸰 ������Ʈ
    private GameObject previewObject; // ��ġ �̸����� ��ü
    private bool isCarrying = false; // ������Ʈ�� �տ� ��� �ִ��� ����
    private InputDevice leftController; // ���� ��Ʈ�ѷ�
    private InputDevice rightController; // ������ ��Ʈ�ѷ�
    private bool isRTriggerPressed = false; // ������ Ʈ���� ��ư�� ���ȴ��� ����
    private bool prevRTriggerPressed = false; // ���� �������� ������ Ʈ���� ��ư ���� ����
    private bool isRBbuttonPressed = false; // ������ B ��ư�� ���ȴ��� ����
    private bool prevRBPressed = false; // ���� �������� ������ B ��ư ���� ����
    BoxCollider boxCollider;


    private Renderer previewRenderer; // �̸����� ������Ʈ�� ������
    private PreviewObjectScript previewScript; // �̸����� ������Ʈ�� ��ũ��Ʈ ����

    private Vector3 originalScale; // ������Ʈ�� ���� ũ��
    private Vector3 scaledDownScale; // ���� ũ��

    float OriginalRotate = 0f;


    void Start()
    {
        OriginalRotate = 0f;
        boxCollider = GetComponent<BoxCollider>();
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
        InputDevices.GetDevicesAtXRNode(hand, devices); // Ư�� �տ� ����� ��ġ ��������
        if (devices.Count > 0)
        {
            return devices[0]; // ��ġ�� �ִٸ� ù ��° ��ġ ��ȯ
        }
        return new InputDevice(); // ��ġ�� ���ٸ� �� ��ġ ��ȯ
    }

    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            InitializeControllers(); // ��Ʈ�ѷ��� ��ȿ���� ������ �ٽ� �ʱ�ȭ
        }

        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed); // ������ Ʈ���� ��ư ���� ��������

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            HandleTriggerPress(); // Ʈ���� ��ư�� ���ȴٸ� ó��
        }
        prevRTriggerPressed = isRTriggerPressed; // ���� �������� Ʈ���� ��ư ���� ������Ʈ

        if (isCarrying)
        {
            UpdatePreview(); // ������Ʈ�� ��� �ִٸ� �̸����� ������Ʈ
        }

        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isRBbuttonPressed); // ������ B ��ư ���� ��������
        if (isCarrying && isRBbuttonPressed && !prevRBPressed)
        {
            RotateObject(); // ������Ʈ�� ��� �ְ� B ��ư�� ���ȴٸ� ȸ��
        }
        prevRBPressed = isRBbuttonPressed; // ���� �������� B ��ư ���� ������Ʈ
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
                        float height = boxCollider.size.y;
                        Debug.Log(hit.point);
                        objectInHand.transform.localScale = originalScale; // ũ�⸦ ������� �����ϴ�.
                        Vector3 newPosition = hit.point + new Vector3(0, (objectInHand.transform.localScale.y * height)/2, 0);
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
                    this.transform.rotation = Quaternion.Euler(0, OriginalRotate, 0);
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
        previewObject = Instantiate(previewPrefab); // �̸����� �������� �����մϴ�.
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
                float height = boxCollider.size.y;
                Vector3 newPosition = hit.point + new Vector3(0, (previewObject.transform.localScale.y * height) / 2, 0);
                previewObject.transform.position = newPosition;
                previewObject.transform.rotation = Quaternion.Euler(0f, OriginalRotate, 0f);
            }
        }
    }
}
