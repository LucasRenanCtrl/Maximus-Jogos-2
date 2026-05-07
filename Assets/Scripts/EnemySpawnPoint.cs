using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float minDistance = 3f;
    public float maxDistance = 10f;

    private bool hasSpawned = false;

    void Update()
    {
        if (hasSpawned) return;
        if (player == null || enemyPrefab == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance >= minDistance && distance <= maxDistance)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            hasSpawned = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}