using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject SelectionPanel;
    public Button StartButton;
    public Button AButton;
    public Button BButton;
    public Button CompleteButton;

    private string selectedScene;

    void Start()
    {
        StartButton.onClick.AddListener(ShowSelectionPanel);
        AButton.onClick.AddListener(() => SelectScene("Atype"));
        BButton.onClick.AddListener(() => SelectScene("Btype"));
        CompleteButton.onClick.AddListener(LoadSelectedScene);
    }

    public void ShowSelectionPanel()
    {
        StartPanel.SetActive(false);
        SelectionPanel.SetActive(true);
    }

    void SelectScene(string sceneName)
    {
        selectedScene = sceneName;
        Debug.Log("Selected Scene: " + selectedScene);
    }

    void LoadSelectedScene()
    {
        if (!string.IsNullOrEmpty(selectedScene))
        {
            SceneManager.LoadScene(selectedScene);
        }
        else
        {
            Debug.LogWarning("No scene selected!");
        }
    }
}