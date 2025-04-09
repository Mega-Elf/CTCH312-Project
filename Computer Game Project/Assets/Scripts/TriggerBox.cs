using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // player on step 3 and enters the trigger
        if (UIManager.Instance.currentQuestStep == 3 && other.CompareTag("Player"))
        {
            UIManager.Instance.currentQuestStep++; // go to next step, step 4
            UIManager.Instance.questGuideUI.text = $"Vanquish \"The Fallen\" and escape the forest."; // update quest hint
        }
    }
}
