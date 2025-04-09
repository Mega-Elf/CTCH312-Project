using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    string highestRoundSurvivedKey = "HighestRoundSurvived";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void SaveHighestRound(int round)
    {
        PlayerPrefs.SetInt(highestRoundSurvivedKey, round);
    }

    public int LoadHighestRound()
    {
        if (PlayerPrefs.HasKey(highestRoundSurvivedKey))
        {
            return PlayerPrefs.GetInt(highestRoundSurvivedKey);
        }
        else
        {
            return 0;
        }
    }
}
