using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float moveSpeedWhenAttacking = 2f; // reduce speed when attacking, to not push the player so much
    public float zombieAttackExitRange = 2f; // higher range to exit than enter to fix animation issue

    private AudioSource zombieChannel; // separate audio channels for each zombie
    private Zombie zombie;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();

        navAgent.speed = moveSpeedWhenAttacking;

        zombie = animator.GetComponent<Zombie>();
        zombieChannel = animator.GetComponent<AudioSource>();

        zombieChannel.Stop(); // stop current sound, so it can play next sound
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (zombieChannel.isPlaying == false)
        {
            // assign a random zombie sound to play
            int rand = Random.Range(0, zombie.attackSoundClips.Length);
            zombieChannel.clip = zombie.attackSoundClips[rand];

            // give a random delay, from 0.25 to 1.25 seconds, to the sound that is played
            float randFloat = Random.Range(0.25f, 1.25f);
            zombieChannel.PlayDelayed(randFloat);
        }

        navAgent.destination = player.position; // zombie moves towards player, keeps zombie looking at player

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // Transition to Attacking State
        if (distanceFromPlayer > zombieAttackExitRange) // test-- if zombie in range to attack
        {
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // zombieChannel.Stop(); // stop current sound, so it can play next sound
    }
}
