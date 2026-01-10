using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemigos")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<float> enemyCooldowns;

    [Header("Spawn distance")]
    [SerializeField] private float maxSpawnDistance = 50f;


    private Transform[] spawnPoints; 
    private List<float> timers;  
    private Transform player;

    void Awake()
    {
        var spawnPointsParent = transform;

        //cogemos todos los hijos menos el padre
        Transform[] all = spawnPointsParent.GetComponentsInChildren<Transform>();
        spawnPoints = new Transform[all.Length - 1];
        Array.Copy(all, 1, spawnPoints, 0, spawnPoints.Length);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        timers = new List<float>(enemyCooldowns);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            timers[i] -= dt;
            if (timers[i] <= 0f)
            {
                SpawnEnemy(enemyPrefabs[i]);
                timers[i] = enemyCooldowns[i];
            }
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        var withDistances = spawnPoints
            .Select(sp => new { point = sp, dist = Vector3.Distance(sp.position, player.position) });

        var closePoints = withDistances
            .Where(x => x.dist <= maxSpawnDistance)
            .ToList();

        Transform chosen;

        if (closePoints.Count > 0)
        {
            chosen = closePoints[UnityEngine.Random.Range(0, closePoints.Count)].point;
        }
        else
        {
            //si no hay puntos cercanos, elegimos el mas cercano
            chosen = withDistances
                .OrderBy(x => x.dist)
                .First()
                .point;
        }

        Instantiate(prefab, chosen.position, chosen.rotation);
    }
}
