using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Enemy enemyPrefab;
    public float spawnRate = 1f;
    public float duration;
    
    float spawnTimer;
    float timer;
    System.Random random;

    void Start() {
        random = new System.Random();
        spawnTimer = 0f;
        timer = duration;
    }

    void Update() {
            spawnTimer -= Time.deltaTime;
            timer -= Time.deltaTime;
            if(spawnTimer <= 0f && timer > 0f) {
                Vector3 displacement = new Vector3((float)random.NextDouble() * transform.localScale.x, 0f, (float)random.NextDouble() * transform.localScale.z);
                Instantiate(enemyPrefab, transform.position + displacement,
                    Quaternion.identity * Quaternion.AngleAxis((float)random.NextDouble(), Vector3.up));
                spawnTimer = spawnRate;
            }

    }

}
