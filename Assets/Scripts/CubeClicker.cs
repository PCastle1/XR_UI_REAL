using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CubeClicker : MonoBehaviour
{
    public GameObject Controller; // 컨트롤러 게임 오브젝트
    private InputDevice rightController; // 오른쪽 컨트롤러
    private bool isRTriggerPressed = false; // 오른쪽 트리거 버튼이 눌렸는지 여부
    private bool prevRTriggerPressed = false; // 이전 프레임의 오른쪽 트리거 버튼 눌림 여부

    private void Start()
    {
        InitializeControllers(); // 컨트롤러 초기화
    }
    void InitializeControllers()
    {
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
        if (!rightController.isValid)
        {
            InitializeControllers(); // 컨트롤러가 유효하지 않으면 다시 초기화
        }

        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed); // 오른쪽 트리거 버튼 상태 가져오기

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            SceneChange(); // 트리거 버튼이 눌렸다면 처리
        }
        prevRTriggerPressed = isRTriggerPressed; // 이전 프레임의 트리거 버튼 상태 업데이트
    }

    void SceneChange()
    {
            RaycastHit hit;
        // 레이와 충돌한 오브젝트를 확인
        if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit, Mathf.Infinity))
        {
                // 충돌한 오브젝트가 큐브일 경우
                if (hit.collider.gameObject.CompareTag("cube"))
                {
                    // 원하는 동작 수행 (예: 장면 전환)
                    Debug.Log("큐브를 클릭했습니다!");
                    // 이동할 장면의 이름에 따라 다른 동작을 수행할 수 있습니다.
                    SceneManager.LoadScene("UIscene");
                }
            }
    }
}
