using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int zombieHealth = 50;
    private Animator animator;

    private NavMeshAgent navAgent;
    public Transform player;

    // For holding all zombie part hitBoxes
    private BoxCollider[] zombieHitBoxes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        zombieHitBoxes = GetComponentsInChildren<BoxCollider>(); // get all hitboxes for the zombie at the start
    }

    public void TakeDamage(int damageTaken)
    {
        zombieHealth -= damageTaken; // zombie takes damage

        if (zombieHealth <= 0) // zombie dies
        {
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

            StartCoroutine(DestroyZombie(gameObject, 3f)); // remove zombie from scene in 3 secs
        }
        else // zombie doesn't die
        {
            animator.SetTrigger("DAMAGE");
        }
    }

    private IEnumerator DestroyZombie(GameObject zombie, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(zombie);
    }
}
