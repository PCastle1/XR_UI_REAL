using UnityEngine;

public class PreviewObjectScript : MonoBehaviour
{
    public Material redMaterial;
    public Material greenMaterial;

    private Renderer previewRenderer;
    public bool isColliding { get; private set; } // 충돌 상태를 외부에서 접근할 수 있도록 public으로 설정

    void Start()
    {
        previewRenderer = GetComponent<Renderer>();
        previewRenderer.material = greenMaterial; // 초기에 녹색 Material 적용
        isColliding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // 6번 레이어 확인
        {
            previewRenderer.material = redMaterial;
            isColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) // 6번 레이어 확인
        {
            previewRenderer.material = greenMaterial;
            isColliding = false;
        }
    }
}
