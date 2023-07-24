using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyConstants : MonoBehaviour
{
    [Header("Scene Name")]
    public string SceneName;
    [Header("Health Settings")]
    public float InitialHealth = 5f;
    public float RegenPerFlit = 3f;
    public bool stopRegenOnDamage = false;
    public float regenTime = 1f;
    public float invulnerableTime = 1f;

    [Header("Progression Settings")]
    public int initialTrailCount = 4;
    public float xpPerKill = 1;
    public float firstLevelXp = 2;
    public float xpIncreaseFactor = 2;
    public float maxXpPerLevel = 16;

    [Header("Knockback Settings")]
    public float enemyKnockbackDist = 3f;
    public float enemyKnockbackMovTime = 0.5f;
    public float enemyKnockbackRotTime = 0.5f;
    public float enemyKnockbackRotDistMax = 900f;

    [Header("Enemy Spawning Settings")]
    public int MaxEnemies = 20;
    public int AmountToSpawnWhenCleared = 5;
    public float MinSplitTimer = 4f;
    public float MaxSplitTimer = 7f;
    public int SpawnMoreThreshold = 0;

// messing with these finely tuned constants feels like a bad idea
// BUT they're already referenced so uh. oop.
    [System.NonSerialized]
    public float PSpace = 3f;
    [System.NonSerialized]
    public float MinPSpaceDistWeight = 0.2f;

    [System.NonSerialized]
    public float TrailAvoidanceWeight = 1;
    [System.NonSerialized]
    public float WallAvoidanceWeight = 10;
    [System.NonSerialized]
    public float FlitAvoidanceWeight = 1;
    [System.NonSerialized]

    public float MaxSpeed = 3f;

}
