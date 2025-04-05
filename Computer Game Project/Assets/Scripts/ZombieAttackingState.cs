using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float moveSpeedWhenAttacking = 2f; // reduce speed when attacking, to not push the player so much
    public float zombieAttackRange = 1.75f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();

        navAgent.speed = moveSpeedWhenAttacking;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.destination = player.position; // zombie moves towards player, keeps zombie looking at player

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // Transition to Attacking State
        if (distanceFromPlayer > zombieAttackRange) // test-- if zombie in range to attack
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
