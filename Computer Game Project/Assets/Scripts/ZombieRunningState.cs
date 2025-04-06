using UnityEngine;
using UnityEngine.AI;

public class ZombieRunningState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float runSpeed = 4f;
    public float zombieAttackRange = 1.75f;

    private AudioSource zombieChannel; // separate audio channels for each zombie
    private Zombie zombie;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();

        navAgent.speed = runSpeed;

        zombie = animator.GetComponent<Zombie>();
        zombieChannel = animator.GetComponent<AudioSource>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (zombieChannel.isPlaying == false)
        {
            // assign a random zombie sound to play
            int rand = Random.Range(0, zombie.zombieSoundClips.Length);
            zombieChannel.clip = zombie.zombieSoundClips[rand];

            // give a random delay, from 1 to 3 seconds, to the sound that is played
            float randFloat = Random.Range(1, 3);
            zombieChannel.PlayDelayed(randFloat);
        }

        navAgent.destination = player.position; // zombie moves towards player

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
        zombieChannel.Stop(); // stop current sound, so it can play attack sound
    }
}
