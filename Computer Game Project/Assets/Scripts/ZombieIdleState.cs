using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transition to the Walking State
        if (UIManager.Instance.currentRound >= 1 && UIManager.Instance.currentRound <= 5) // if round 1 to 5
        {
            animator.SetBool("isWalking", true);
        }

        // Transition to the Running State
        if (UIManager.Instance.currentRound >= 6) // if round 6+
        {
            animator.SetBool("isRunning", true);
        }
    }
}
