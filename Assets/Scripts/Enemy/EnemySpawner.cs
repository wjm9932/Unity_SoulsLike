using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public QuestInfoSO quest;

    [SerializeField]
    TargetEnemiesInfo[] info;
    [SerializeField]
    private string targetNavMeshArea;
    [SerializeField]
    private float spawnRadius = 5f;

    [System.Serializable]
    private struct TargetEnemiesInfo
    {
        public GameObject targetEnemy;
        public int targetCount;
    }

    private int areaMask;
    private List<Enemy> enemies = new List<Enemy>();
    // Start is called before the first frame update
    private void Awake()
    {
        areaMask = NavMesh.GetAreaFromName(targetNavMeshArea);
    }

    private void OnEnable()
    {
        if(quest != null)
        {
            QuestManager.Instance.onFinishQuest += EndWave;
        }
    }

    private void OnDisable()
    {
        if (quest != null)
        {
            QuestManager.Instance.onFinishQuest -= EndWave;
        }
    }
    void Start()
    {
        for (int i = 0; i < info.Length; i++)
        {
            for (int j = 0; j < info[i].targetCount; j++)
            {
                SpawnEnemy(info[i].targetEnemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = GetSpawnPosition();

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetNavMeshArea(areaMask);
        enemies.Add(enemy);

        enemy.onDeath += () => { enemies.Remove(enemy); };
        enemy.onDeath += () => { StartCoroutine(SpawnEnemyAfterDelay(enemyPrefab, 5f)); };
    }

    private Vector3 GetSpawnPosition()
    {
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1 << areaMask) == true)
        {
            return hit.position; 
        }
        else
        {
            return transform.position;
        }
    }

    private IEnumerator SpawnEnemyAfterDelay(GameObject targetEnemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy(targetEnemy); 
    }
    private void EndWave(Quest quest)
    {
        if(quest.info.id == this.quest.id)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Destroy(enemies[i].gameObject);
            }
        }
    }
}
