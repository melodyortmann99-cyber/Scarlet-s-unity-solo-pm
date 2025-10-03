using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyTarget;

    public float spawnTimer;

    public bool hasSpawned = false;

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned)
        {
            Instantiate(enemyTarget, transform.position, transform.rotation);
            hasSpawned = true;
            StartCoroutine("spawnCooldown");
        }
    }

    IEnumerator spawnCooldown()
    {
        yield return new WaitForSeconds(spawnTimer);

        hasSpawned = false;
    }
}
