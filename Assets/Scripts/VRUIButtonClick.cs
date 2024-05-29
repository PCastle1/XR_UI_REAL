using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRUIButtonClick : MonoBehaviour
{
    public float maxRayDistance = 10.0f; // Ray가 도달할 최대 거리
    public LayerMask uiLayerMask; // UI 요소들이 있는 레이어

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) // "Fire1"은 Unity 입력 매니저에서 설정한 버튼 이름입니다. VR 컨트롤러 버튼에 맞게 변경해주세요.
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance, uiLayerMask))
            {
                Button button = hit.transform.GetComponent<Button>();
                if (button)
                {
                    button.onClick.Invoke(); // 버튼이 클릭되었을 때 실행할 함수를 호출합니다.
                }
            }
        }
    }
}