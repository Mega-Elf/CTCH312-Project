using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highestRoundUI;

    string newGameScene = "MainGame";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the highest round text
        int highestRound = SaveLoadManager.Instance.LoadHighestRound();
        highestRoundUI.text = $"Highest Round Survived: {highestRound}";
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
