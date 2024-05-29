using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class VRUIButtonClick : MonoBehaviour
{
    public XRNode rightHand; // 오른손 컨트롤러를 지정합니다.
    public Button uiButton; // UI 버튼을 지정합니다.

    private InputDevice rightController;

    private void Start()
    {
        rightController = InputDevices.GetDeviceAtXRNode(rightHand);
    }

    private void Update()
    {
        if (rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            uiButton.onClick.Invoke(); // 트리거 버튼이 눌리면 버튼 클릭 이벤트를 호출합니다.
            Debug.Log(uiButton);
        }
    }
}