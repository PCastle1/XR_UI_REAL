using UnityEngine;

public class PreviewObjectScript : MonoBehaviour
{
    public Material redMaterial; // 빨간색 Material
    public Material greenMaterial; // 초록색 Material

    private Renderer previewRenderer; // Renderer 컴포넌트 참조
    public bool isColliding { get; private set; } // 충돌 상태를 외부에서 접근할 수 있도록 public으로 설정

    void Start()
    {
        previewRenderer = GetComponent<Renderer>(); // Renderer 컴포넌트를 가져옴
        previewRenderer.material = greenMaterial; // 초기에 초록색 Material 적용
        isColliding = false; // 초기 충돌 상태를 false로 설정
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // 충돌한 객체의 레이어가 6번인지 확인
        {
            previewRenderer.material = redMaterial; // 빨간색 Material 적용
            isColliding = true; // 충돌 상태를 true로 설정
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) // 충돌한 객체의 레이어가 6번인지 확인
        {
            previewRenderer.material = greenMaterial; // 초록색 Material 적용
            isColliding = false; // 충돌 상태를 false로 설정
        }
    }
}
