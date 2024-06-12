using UnityEngine;

public class PreviewObjectScript : MonoBehaviour
{
    public Material redMaterial; // ������ Material
    public Material greenMaterial; // �ʷϻ� Material

    private Renderer previewRenderer; // Renderer ������Ʈ ����
    public bool isColliding { get; private set; } // �浹 ���¸� �ܺο��� ������ �� �ֵ��� public���� ����

    void Start()
    {
        previewRenderer = GetComponent<Renderer>(); // Renderer ������Ʈ�� ������
        previewRenderer.material = greenMaterial; // �ʱ⿡ �ʷϻ� Material ����
        isColliding = false; // �ʱ� �浹 ���¸� false�� ����
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // �浹�� ��ü�� ���̾ 6������ Ȯ��
        {
            previewRenderer.material = redMaterial; // ������ Material ����
            isColliding = true; // �浹 ���¸� true�� ����
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) // �浹�� ��ü�� ���̾ 6������ Ȯ��
        {
            previewRenderer.material = greenMaterial; // �ʷϻ� Material ����
            isColliding = false; // �浹 ���¸� false�� ����
        }
    }
}
