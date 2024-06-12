using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CubeClicker : MonoBehaviour
{
    public GameObject Controller; // ��Ʈ�ѷ� ���� ������Ʈ
    private InputDevice rightController; // ������ ��Ʈ�ѷ�
    private bool isRTriggerPressed = false; // ������ Ʈ���� ��ư�� ���ȴ��� ����
    private bool prevRTriggerPressed = false; // ���� �������� ������ Ʈ���� ��ư ���� ����

    private void Start()
    {
        InitializeControllers(); // ��Ʈ�ѷ� �ʱ�ȭ
    }
    void InitializeControllers()
    {
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
        if (!rightController.isValid)
        {
            InitializeControllers(); // ��Ʈ�ѷ��� ��ȿ���� ������ �ٽ� �ʱ�ȭ
        }

        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed); // ������ Ʈ���� ��ư ���� ��������

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            SceneChange(); // Ʈ���� ��ư�� ���ȴٸ� ó��
        }
        prevRTriggerPressed = isRTriggerPressed; // ���� �������� Ʈ���� ��ư ���� ������Ʈ
    }

    void SceneChange()
    {
            RaycastHit hit;
        // ���̿� �浹�� ������Ʈ�� Ȯ��
        if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity))
        {
                // �浹�� ������Ʈ�� ť���� ���
                if (hit.collider.gameObject.CompareTag("cube"))
                {
                    // ���ϴ� ���� ���� (��: ��� ��ȯ)
                    Debug.Log("ť�긦 Ŭ���߽��ϴ�!");
                    // �̵��� ����� �̸��� ���� �ٸ� ������ ������ �� �ֽ��ϴ�.
                    SceneManager.LoadScene("UIscene");
                }
            }
    }
}
