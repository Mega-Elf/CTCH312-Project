using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int health = 50;
    private Animator animator;

    private NavMeshAgent navAgent;

    private CapsuleCollider hitBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        hitBox = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (navAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(int damageTaken)
    {
        health = health - damageTaken;

        if (health <= 0)
        {
            hitBox.enabled = false; // turn off hitbox when zombie dies

            int randomNumber = Random.Range(0, 2); // random number 0 or 1, which death effect plays
            if (randomNumber == 0)
            {
                animator.SetTrigger("DEATH_1");
            }
            else
            {
                animator.SetTrigger("DEATH_2");
            }

            // Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }
}
