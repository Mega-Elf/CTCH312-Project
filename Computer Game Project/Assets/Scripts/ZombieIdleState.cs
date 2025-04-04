using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transition to the Walking State
        if (UIManager.Instance.roundCount >= 2) // test-- if at least round 2
        {
            animator.SetBool("isWalking", true);
        }
    }
}
