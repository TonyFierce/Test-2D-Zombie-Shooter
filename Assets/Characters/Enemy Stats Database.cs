using System;
using UnityEngine;

public class EnemyStatsDatabase : MonoBehaviour
{
    public EnemyStats[] stats;
    public static EnemyStatsDatabase selfInstance;

    private void Awake()
    {
        selfInstance = this;
    }

    public static EnemyStats GetEnemyStats(int enemyID)
    {
        return selfInstance.stats[enemyID];
    }

}

[Serializable]
public class EnemyStats
{
    public int maxHealth = 15;
    public float moveSpeed = 2;
    public float attackSpeed = 1;
    public string enemyName = "Enemy";
    public float spawnHeight = 0;
    public float sizeScale = 1;
    public int spawnChanceWeight = 50;
    public int ammoDropMin = 1;
    public int ammoDropMax = 3;
}