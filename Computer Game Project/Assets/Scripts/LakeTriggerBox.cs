using UnityEngine;

public class LakeTriggerBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // player on step 4 "Approach the lake", and enters the lake trigger
        if (UIManager.Instance.currentQuestStep == 4 && other.CompareTag("Player"))
        {
            UIManager.Instance.currentQuestStep++; // go to next step, step 5
            UIManager.Instance.questGuideUI.text = $"Vanquish \"The Fallen\" and escape the forest."; // update quest hint
        }
    }
}
