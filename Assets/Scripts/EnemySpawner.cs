using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referências")]
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Ativação")]
    public float activationDistance = 8f;
    public bool spawnOnlyOnce = true;

    [Header("Respawn")]
    public bool canRespawn = false;
    public float respawnTime = 10f;

    [Header("Patrulha do inimigo")]
    public Transform patrolPointA;
    public Transform patrolPointB;

    private GameObject currentEnemy;
    private bool hasSpawned;
    private float respawnCounter;

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Se ainda não existe inimigo vivo nesse ponto
        if (currentEnemy == null)
        {
            // Se o spawn for só uma vez e já aconteceu, para aqui
            if (spawnOnlyOnce && hasSpawned) return;

            // Respawn opcional
            if (hasSpawned && canRespawn)
            {
                respawnCounter -= Time.deltaTime;
                if (respawnCounter > 0f) return;
            }

            // Jogador chegou perto: nasce o inimigo
            if (distance <= activationDistance)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        hasSpawned = true;
        respawnCounter = respawnTime;

        // Exemplo: enviar pontos de patrulha para o script do inimigo
        Inimigo patrol = currentEnemy.GetComponent<Inimigo>();
        if (patrol != null)
        {
            patrol.pointA = patrolPointA;
            patrol.pointB = patrolPointB;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        if (patrolPointA != null && patrolPointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(patrolPointA.position, patrolPointB.position);
            Gizmos.DrawSphere(patrolPointA.position, 0.15f);
            Gizmos.DrawSphere(patrolPointB.position, 0.15f);
        }
    }
}

internal class EnemyPatrol
{
    internal Transform pointB;
    internal Transform pointA;
}