using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public SpriteRenderer dummySprite;
    public NPCController enemyPrefab;
    public int[] spawnableEnemies; // Array of enemy IDs this spawner can spawn

    private void Awake()
    {
        dummySprite.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemyAtIntervals());
    }

    private IEnumerator SpawnEnemyAtIntervals()
    {
        // Spawn the first enemy instantly
        SpawnEnemy();

        // Continue spawning enemies at intervals
        while (true)
        {
            // Wait for a random time between 3 and 10 seconds
            float waitTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(waitTime);

            SpawnEnemy();
        }
    }


    private void SpawnEnemy()
    {
        // Select an enemy ID based on weighted random chance
        int selectedEnemyID = GetWeightedRandomEnemyID();

        // Get the selected enemy stats from the database
        EnemyStats selectedEnemyStats = EnemyStatsDatabase.GetEnemyStats(selectedEnemyID);

        NPCController newEnemy = Instantiate(enemyPrefab, transform.position + new Vector3(0, selectedEnemyStats.spawnHeight, 0), Quaternion.identity, LevelManager.selfTransform);

        newEnemy.creatureID = selectedEnemyID;
        newEnemy.npcCharacter.creatureID = selectedEnemyID;
    }

    private int GetWeightedRandomEnemyID()
    {
        // Sum the spawnChanceWeight values for the spawnable enemies
        int totalWeight = 0;
        foreach (int enemyID in spawnableEnemies)
        {
            totalWeight += EnemyStatsDatabase.GetEnemyStats(enemyID).spawnChanceWeight;
        }

        // Generate a random number between 0 and the total weight
        int randomValue = Random.Range(0, totalWeight);

        // Select the enemy based on the random number
        int cumulativeWeight = 0;
        foreach (int enemyID in spawnableEnemies)
        {
            cumulativeWeight += EnemyStatsDatabase.GetEnemyStats(enemyID).spawnChanceWeight;
            if (randomValue < cumulativeWeight)
            {
                return enemyID;
            }
        }

        // Fallback in case something goes wrong
        return spawnableEnemies[0];
    }
}
