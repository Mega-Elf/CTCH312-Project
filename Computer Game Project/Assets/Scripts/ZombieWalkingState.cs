using UnityEngine;
using UnityEngine.AI;

public class ZombieWalkingState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float maxWalkSpeed = 4.5f;
    public float zombieAttackRange = 1.75f;

    private AudioSource zombieChannel; // separate audio channels for each zombie
    private Zombie zombie;

    public float startingAnimSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();

        startingAnimSpeed = animator.speed; // store current animator speed

        if (UIManager.Instance.currentRound < 6) // rounds 1 to 5
        {
            // r1 = 2.5, r2 = 3, r3 = 3.5, etc.
            navAgent.speed = 2f + 0.5f * UIManager.Instance.currentRound;
            // play walking animation faster to match move speed increase
            animator.speed = 2f + 0.2f * (UIManager.Instance.currentRound - 1);
        }

        zombie = animator.GetComponent<Zombie>();
        zombieChannel = animator.GetComponent<AudioSource>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (zombieChannel.isPlaying == false)
        {
            // assign a random zombie sound to play
            int randInt = Random.Range(0, zombie.zombieSoundClips.Length);
            zombieChannel.clip = zombie.zombieSoundClips[randInt];

            // give a random delay, from 1 to 5 seconds, to the sound that is played
            float randFloat = Random.Range(1, 5);
            zombieChannel.PlayDelayed(randFloat);
        }

        navAgent.destination = player.position; // zombie moves towards player

        // Transition to Running State
        if (UIManager.Instance.currentRound >= 6) // if round 6+, start running
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
        zombieChannel.Stop(); // stop current sound, so it can play attack sound
        animator.speed = startingAnimSpeed; // set animator speed back to what is was previously
    }
}
