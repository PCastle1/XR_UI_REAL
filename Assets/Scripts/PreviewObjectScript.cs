using UnityEngine;

public class PreviewObjectScript : MonoBehaviour
{
    public Material redMaterial;
    public Material greenMaterial;

    private Renderer previewRenderer;
    public bool isColliding { get; private set; } // �浹 ���¸� �ܺο��� ������ �� �ֵ��� public���� ����

    void Start()
    {
        previewRenderer = GetComponent<Renderer>();
        previewRenderer.material = greenMaterial; // �ʱ⿡ ��� Material ����
        isColliding = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // 6�� ���̾� Ȯ��
        {
            previewRenderer.material = redMaterial;
            isColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) // 6�� ���̾� Ȯ��
        {
            previewRenderer.material = greenMaterial;
            isColliding = false;
        }
    }
}
