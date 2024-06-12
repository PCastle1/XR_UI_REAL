using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject StartPanel; // 시작 화면 패널
    public GameObject SelectionPanel; // 선택 화면 패널
    public Button StartButton; // 시작 버튼
    public Button AButton; // A 타입 버튼
    public Button BButton; // B 타입 버튼
    public Button CompleteButton; // 완료 버튼

    private string selectedScene; // 선택된 씬 이름을 저장할 변수

    void Start()
    {
        // 시작 버튼 클릭 시 ShowSelectionPanel 메서드 호출
        StartButton.onClick.AddListener(ShowSelectionPanel);
        // A 버튼 클릭 시 SelectScene 메서드를 호출하여 "Atype" 씬 선택
        AButton.onClick.AddListener(() => SelectScene("Atype"));
        // B 버튼 클릭 시 SelectScene 메서드를 호출하여 "Btype" 씬 선택
        BButton.onClick.AddListener(() => SelectScene("Btype"));
        // 완료 버튼 클릭 시 LoadSelectedScene 메서드 호출
        CompleteButton.onClick.AddListener(LoadSelectedScene);
    }

    // 선택 화면 패널을 보여주는 메서드
    void ShowSelectionPanel()
    {
        StartPanel.SetActive(false); // 시작 화면 패널 숨김
        SelectionPanel.SetActive(true); // 선택 화면 패널 보임
    }

    // 선택된 씬 이름을 설정하는 메서드
    void SelectScene(string sceneName)
    {
        selectedScene = sceneName; // 선택된 씬 이름 저장
        Debug.Log("Selected Scene: " + selectedScene); // 디버그 로그 출력
    }

    // 선택된 씬을 로드하는 메서드
    void LoadSelectedScene()
    {
        if (!string.IsNullOrEmpty(selectedScene)) // 씬이 선택되었는지 확인
        {
            SceneManager.LoadScene(selectedScene); // 선택된 씬 로드
        }
        else
        {
            Debug.LogWarning("No scene selected!"); // 씬이 선택되지 않았을 경우 경고 로그 출력
        }
    }
}
