using UnityEngine;
using UnityEngine.AI;

public class ZombieRunningState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent navAgent;

    public float maxRunSpeed = 6.5f;
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

        if (UIManager.Instance.currentRound < 13) // rounds 6 to 12
        {
            // r6 = 5, r7 = 5.25, r8 = 5.5, r9 = 5.75, r10 = 6, r11 = 6.25, r12 = 6.5
            navAgent.speed = 5f + 0.25f * (UIManager.Instance.currentRound - 6);
            // play running animation faster to match move speed increase
            animator.speed = 1f + 0.1f * (UIManager.Instance.currentRound - 6);
        }
        else // rounds 13+
        {
            navAgent.speed = maxRunSpeed; // 6.5
            animator.speed = 1.6f;
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
            int rand = Random.Range(0, zombie.zombieSoundClips.Length);
            zombieChannel.clip = zombie.zombieSoundClips[rand];

            // give a random delay, from 1 to 4 seconds, to the sound that is played
            float randFloat = Random.Range(1, 4);
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
        // zombieChannel.Stop(); // stop current sound, so it can play next sound
        animator.speed = startingAnimSpeed; // set animator speed back to what is was previously
    }
}
