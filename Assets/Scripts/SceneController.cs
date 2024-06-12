using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject StartPanel; // ���� ȭ�� �г�
    public GameObject SelectionPanel; // ���� ȭ�� �г�
    public Button StartButton; // ���� ��ư
    public Button AButton; // A Ÿ�� ��ư
    public Button BButton; // B Ÿ�� ��ư
    public Button CompleteButton; // �Ϸ� ��ư

    private string selectedScene; // ���õ� �� �̸��� ������ ����

    void Start()
    {
        // ���� ��ư Ŭ�� �� ShowSelectionPanel �޼��� ȣ��
        StartButton.onClick.AddListener(ShowSelectionPanel);
        // A ��ư Ŭ�� �� SelectScene �޼��带 ȣ���Ͽ� "Atype" �� ����
        AButton.onClick.AddListener(() => SelectScene("Atype"));
        // B ��ư Ŭ�� �� SelectScene �޼��带 ȣ���Ͽ� "Btype" �� ����
        BButton.onClick.AddListener(() => SelectScene("Btype"));
        // �Ϸ� ��ư Ŭ�� �� LoadSelectedScene �޼��� ȣ��
        CompleteButton.onClick.AddListener(LoadSelectedScene);
    }

    // ���� ȭ�� �г��� �����ִ� �޼���
    void ShowSelectionPanel()
    {
        StartPanel.SetActive(false); // ���� ȭ�� �г� ����
        SelectionPanel.SetActive(true); // ���� ȭ�� �г� ����
    }

    // ���õ� �� �̸��� �����ϴ� �޼���
    void SelectScene(string sceneName)
    {
        selectedScene = sceneName; // ���õ� �� �̸� ����
        Debug.Log("Selected Scene: " + selectedScene); // ����� �α� ���
    }

    // ���õ� ���� �ε��ϴ� �޼���
    void LoadSelectedScene()
    {
        if (!string.IsNullOrEmpty(selectedScene)) // ���� ���õǾ����� Ȯ��
        {
            SceneManager.LoadScene(selectedScene); // ���õ� �� �ε�
        }
        else
        {
            Debug.LogWarning("No scene selected!"); // ���� ���õ��� �ʾ��� ��� ��� �α� ���
        }
    }
}
