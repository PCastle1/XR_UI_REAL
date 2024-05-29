using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class VRUIButtonClick : MonoBehaviour
{
    public XRNode rightHand; // ������ ��Ʈ�ѷ��� �����մϴ�.
    public Button uiButton; // UI ��ư�� �����մϴ�.

    private InputDevice rightController;

    private void Start()
    {
        rightController = InputDevices.GetDeviceAtXRNode(rightHand);
    }

    private void Update()
    {
        if (rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            uiButton.onClick.Invoke(); // Ʈ���� ��ư�� ������ ��ư Ŭ�� �̺�Ʈ�� ȣ���մϴ�.
            Debug.Log(uiButton);
        }
    }
}