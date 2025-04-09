using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Zombie : MonoBehaviour
{
    [SerializeField] public float zombieHealth = 50;
    public bool isDead;
    private Animator animator;

    private NavMeshAgent navAgent;
    public GameObject player;

    // For holding all zombie part hitBoxes
    private BoxCollider[] zombieHitBoxes;

    // For dealing damage to player
    public ZombieHand zombieHandLeft;
    public ZombieHand zombieHandRight;
    public int zombieDamage;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource zombieChannel; // separate audio channels for each zombie
    [SerializeField] public AudioClip[] damageSoundClips;
    [SerializeField] public AudioClip[] deathSoundClips;
    [SerializeField] public AudioClip[] attackSoundClips;
    [SerializeField] public AudioClip[] zombieSoundClips; // regular zombie sounds, play when walking or running

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        zombieHitBoxes = GetComponentsInChildren<BoxCollider>(); // get all hitboxes for the zombie at the start

        zombieHandLeft.damage = zombieDamage; // set damage of zombie
        zombieHandRight.damage = zombieDamage; // set damage of zombie
    }

    public void Update()
    {
        if (player.GetComponent<Player>().isDead == true) // if player is dead
        {
            zombieChannel.volume = 0f; // mute zombie audio source
        }
    }

    public void TakeDamage(float damageTaken, GameObject bodyPartHit)
    {
        zombieHealth -= damageTaken; // zombie takes damage

        if (zombieHealth <= 0) // zombie dies
        {
            if (bodyPartHit.gameObject.CompareTag("Zombie")) // bodyshot kill
            {
                UIManager.Instance.scoreCount += 100;
            }

            if (bodyPartHit.gameObject.CompareTag("ZombieHead")) // headshot kill
            {
                UIManager.Instance.scoreCount += 125;
            }

            isDead = true;
            UIManager.Instance.killCount++; // increase kill count

            navAgent.destination = navAgent.transform.position; // stop zombie from moving

            foreach (BoxCollider hitBox in zombieHitBoxes)
            {
                hitBox.enabled = false; // turn off all hitboxes
            }

            int randomNumber = Random.Range(0, 2); // random number 0 or 1, which death effect plays
            if (randomNumber == 0)
            {
                animator.SetTrigger("DEATH_1");
            }
            else
            {
                animator.SetTrigger("DEATH_2");
            }

            zombieChannel.Stop(); // stops current audio

            // assign a random death sound to play
            int rand = Random.Range(0, deathSoundClips.Length);
            zombieChannel.PlayOneShot(deathSoundClips[rand]); // sound overlaps, but it should be fine as it only plays once

            StartCoroutine(DestroyZombie(gameObject, 3f)); // remove zombie from scene in 3 seconds
        }
        else // zombie doesn't die
        {
            UIManager.Instance.scoreCount += 10;

            animator.SetTrigger("DAMAGE");

            zombieChannel.Stop(); // stops current audio

            // assign a random damage sound to play
            int rand = Random.Range(0, damageSoundClips.Length);
            zombieChannel.clip = damageSoundClips[rand];
            zombieChannel.Play(); // sound doesn't overlap

            // zombieChannel.PlayOneShot(damageSoundClips[rand]); // sound overlaps
            // SoundManager.Instance.PlayRandomSoundFXClip(damageSoundClips, transform, 0.25f); // Sound Method 2, doesn't overlap
        }
    }

    private IEnumerator DestroyZombie(GameObject zombie, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(zombie);
    }
}
