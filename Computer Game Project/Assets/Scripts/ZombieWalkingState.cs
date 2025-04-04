using UnityEngine;
using UnityEngine.AI;

public class ZombieWalkingState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float walkSpeed = 2f;
    public float zombieAttackRange = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();

        navAgent.speed = walkSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.destination = player.position; // zombie moves towards player

        // Transition to Running State
        if (UIManager.Instance.roundCount >= 3) // test-- if round 3
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // Transition to Attacking State
        if (distanceFromPlayer <= zombieAttackRange) // test-- if zombie in range to attack
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.destination = animator.transform.position; // stop moving
    }
}
