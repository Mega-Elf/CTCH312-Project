using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; set; }

    public int currentZombiesPerRound;
    public int maxZombiesOnMap = 20;

    public float spawnDelay = 0.5f; // delay between zombie spawns during a round

    public float roundDelay = 10f; // seconds between end of round and start of next round

    public bool inbetweenRounds;
    public float inbetweenRoundsCounter = 0; // for testing and UI

    public List<Zombie> remainingZombiesAlive;
    public GameObject zombiePrefab;

    public GameObject[] zombieSpawners;
    public Transform player;
    public bool spawnSuccessful;

    [Header("Zombie Boss")]
    public GameObject zombieBossSpawner;
    public GameObject zombieBossPrefab;
    public bool bossSpawned = false;
    public Zombie bossZombieScript;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentZombiesPerRound = Convert.ToInt16(Math.Floor(2.25 * UIManager.Instance.currentRound + 4));

        UIManager.Instance.currentRound++; // increase round from 0 to 1, start game
        GlobalReferences.Instance.roundNumber = UIManager.Instance.currentRound; // set highest round survived in global ref

        StartNextRound();
    }

    // Update is called once per frame
    void Update()
    {
        // get all dead zombies
        List<Zombie> zombiesToRemove = new List<Zombie>();
        foreach (Zombie zombie in remainingZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        // remove all dead zombies
        foreach (Zombie zombie in zombiesToRemove)
        {
            remainingZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        // Start between round delay if all zombies are dead
        if (remainingZombiesAlive.Count == 0 && inbetweenRounds == false)
        {
            // Start delay for next round
            StartCoroutine(RoundDelay());
        }

        // Run the delay counter
        if (inbetweenRounds)
        {
            inbetweenRoundsCounter -= Time.deltaTime;
        }
        else
        {
            inbetweenRoundsCounter = roundDelay;
        }

        // if player on step 5 "Vanquish 'The Fallen' and escape", and boss hasn't spawmed yet
        if (UIManager.Instance.currentQuestStep == 5 && bossSpawned == false)
        {
            bossSpawned = true;
            SpawnBoss(); // spawn the boss
        }

        // still on step 5, but has defeated the boss
        if (UIManager.Instance.currentQuestStep == 5 && bossZombieScript.zombieHealth <= 0f)
        {
            UIManager.Instance.bossHealthBar.SetActive(false); // turn off boss health bar
            UIManager.Instance.currentQuestStep++; // go to next step, no more steps this just ends quest
            UIManager.Instance.scoreCount += 10000; // increase player score for killing the boss
            UIManager.Instance.questGuideUI.text = $"You Win!\nNow see how long you can survive!"; // update quest hint
        }
    }

    private IEnumerator RoundDelay()
    {
        inbetweenRounds = true;

        UIManager.Instance.currentRound++; // increase round before the delay
        GlobalReferences.Instance.roundNumber = UIManager.Instance.currentRound; // set highest round survived in global ref

        yield return new WaitForSeconds(roundDelay);

        inbetweenRounds = false;

        IncreaseZombiesPerRound();

        StartNextRound();
    }

    private void IncreaseZombiesPerRound()
    {
        // plotted COD spawns per round in Desmos, and created a similar formula but with slower scaling
        currentZombiesPerRound = Convert.ToInt16(Math.Floor(2.25 * UIManager.Instance.currentRound + 4));
    }

    private void StartNextRound()
    {
        remainingZombiesAlive.Clear();

        StartCoroutine(SpawnRound());
    }

    private IEnumerator SpawnRound()
    {
        // --- Random Spawns ---
        for (int i = 0; i < currentZombiesPerRound; i++)
        {
            while (spawnSuccessful == false) // keep trying to spawn zombie
            {
                if (remainingZombiesAlive.Count < maxZombiesOnMap) // if room to spawn more, then spawn
                {
                    // choose random spawn point to spawn zombie at
                    int rand = Random.Range(0, zombieSpawners.Length);
                    GameObject spawner = zombieSpawners[rand];

                    Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                    Vector3 spawnPosition = spawner.transform.position + spawnOffset;
                
                    // Instantiate the zombie
                    var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

                    // get zombie script
                    Zombie zombieScript = zombie.GetComponent<Zombie>();

                    // track this zombie
                    remainingZombiesAlive.Add(zombieScript);

                    spawnSuccessful = true;

                    yield return new WaitForSeconds(spawnDelay);
                }
                else // no room to spawn more zombie at the moment
                {
                    print("The map has the max amount of zombie spawns right now. kill some to spawn more.");
                    yield return new WaitForSeconds(2f);
                }
            }

            spawnSuccessful = false; // reset spawn bool
        }
    }

    private void SpawnBoss()
    {
        GameObject bossSpawner = zombieBossSpawner;
        Vector3 bossSpawnPosition = bossSpawner.transform.position;

        // Instantiate the zombie boss
        var zombieBoss = Instantiate(zombieBossPrefab, bossSpawnPosition, Quaternion.identity);

        // get boss zombie script
        bossZombieScript = zombieBoss.GetComponent<Zombie>();

        UIManager.Instance.bossHealthBar.SetActive(true); // turn on boss health bar
    }
}
