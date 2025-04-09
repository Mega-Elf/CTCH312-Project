using UnityEngine;

public class CabinTriggerBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // player on step 2 "explore the forest", and enters the cabin trigger
        if (UIManager.Instance.currentQuestStep == 2 && other.CompareTag("Player"))
        {
            UIManager.Instance.currentQuestStep++; // go to next step, step 3
            UIManager.Instance.questGuideUI.text = $"Send a message for help!"; // update quest hint
        }
    }
}
