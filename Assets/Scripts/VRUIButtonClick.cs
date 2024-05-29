using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRUIButtonClick : MonoBehaviour
{
    public float maxRayDistance = 10.0f; // Ray�� ������ �ִ� �Ÿ�
    public LayerMask uiLayerMask; // UI ��ҵ��� �ִ� ���̾�

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) // "Fire1"�� Unity �Է� �Ŵ������� ������ ��ư �̸��Դϴ�. VR ��Ʈ�ѷ� ��ư�� �°� �������ּ���.
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance, uiLayerMask))
            {
                Button button = hit.transform.GetComponent<Button>();
                if (button)
                {
                    button.onClick.Invoke(); // ��ư�� Ŭ���Ǿ��� �� ������ �Լ��� ȣ���մϴ�.
                }
            }
        }
    }
}