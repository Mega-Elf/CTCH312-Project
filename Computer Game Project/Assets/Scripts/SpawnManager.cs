using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; set; }

    public int initialZombiesPerRound = 3;
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
        currentZombiesPerRound = initialZombiesPerRound;

        UIManager.Instance.currentRound++; // increase round from 0 to 1, start game
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
    }

    private IEnumerator RoundDelay()
    {
        inbetweenRounds = true;

        UIManager.Instance.currentRound++; // increase round before the delay

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
}
