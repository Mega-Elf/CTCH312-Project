using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerHealth = 100;

    public void TakeDamage(int damageTaken)
    {
        playerHealth -= damageTaken; // player takes damage

        if (playerHealth <= 0) // player dies
        {
            print("Player has died");
        }
        else // player doesn't die
        {
            print("Player has been hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
