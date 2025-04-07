using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public int playerHealth = 150;

    public GameObject playerDamageScreen;

    public bool isDead;

    public void TakeDamage(int damageTaken)
    {
        playerHealth -= damageTaken; // player takes damage

        UIManager.Instance.playerHealthUI.text = $"{playerHealth}";

        if (playerHealth <= 0 && !isDead) // player takes lethal damage, and wasn't already dead
        {
            isDead = true;
            print("Player has died");
            PlayerDeath();
            SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

            SoundManager.Instance.PlayerChannel.clip = SoundManager.Instance.gameOverMusic;
            SoundManager.Instance.PlayerChannel.Play();
        }
        else // player doesn't die
        {
            print("Player has been hit");
            StartCoroutine(PlayerDamageScreenEffect());
            SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerDamaged);

            // // assign a random death sound to play
            // int rand = Random.Range(0, SoundManager.Instance.playerDamagedClips.Length);
            // SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerDamagedClips[rand]); // sound overlaps
        }
    }

    private void PlayerDeath()
    {
        // stop player movement
        GetComponent<FirstPersonController>().enabled = false;

        // dying animation
        GetComponentInChildren<Animator>().enabled = true;

        // disable crosshair and player hud
        UIManager.Instance.crosshairCanvas.SetActive(false);
        UIManager.Instance.playerHUDCanvas.SetActive(false);

        // disable audio sources 
        SoundManager.Instance.ShootingChannel.volume = 0f; // mutes shooting audio
        SoundManager.Instance.ReloadingChannel.volume = 0f; // mutes reloading audio
        SoundManager.Instance.dryFireSoundGlock18.volume = 0f; // mutes glock dry fire audio
        SoundManager.Instance.dryFireSoundAK47.volume = 0f; // mutes ak dry fire audio

        // fade to black screen
        GetComponent<FadeToBlack>().StartFade();

        // display game over text
        StartCoroutine(ShowGameEndScreenUI());
    }

    private IEnumerator ShowGameEndScreenUI()
    {
        yield return new WaitForSeconds(3f); // current screen fade time is 3 seconds
        UIManager.Instance.gameEndScreenCanvas.SetActive(true);

        int roundsSurvived = GlobalReferences.Instance.roundNumber - 1; // highest round reached, ex. reaching round 10 means you survived 9 rounds

        if (roundsSurvived > SaveLoadManager.Instance.LoadHighestRound()) // only save rounds survived if it is higher
        {
            SaveLoadManager.Instance.SaveHighestRound(roundsSurvived);
        }
    }

    private IEnumerator PlayerDamageScreenEffect()
    {
        if (playerDamageScreen.activeInHierarchy == false)
        {
            playerDamageScreen.SetActive(true);
        }

        // image fade effect
        var image = playerDamageScreen.GetComponentInChildren<Image>();
 
        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 1f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
 
            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
 
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
 
            yield return null; // Wait for the next frame.
        }
        // end of image fade effect

        if (playerDamageScreen.activeInHierarchy)
        {
            playerDamageScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (!isDead)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}
