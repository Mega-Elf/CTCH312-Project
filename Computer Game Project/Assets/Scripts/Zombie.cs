using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int health = 50;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageTaken)
    {
        health = health - damageTaken;

        if (health <= 0)
        {
            animator.SetTrigger("DEATH");
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }
}
