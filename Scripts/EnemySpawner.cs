using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private int spawns = 0;
    private int spawnLimit = 0;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (spawns <= spawnLimit)
        {
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            spawns++;
            yield return new WaitForSeconds(5.0f);
        }
    }
}
