using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform parent;
    public QuestInfoSO quest;

    [SerializeField]
    private GameObject enemyPrefab;


    [SerializeField]
    private string targetNavMeshArea;

    [SerializeField]
    private float spawnRadius = 5f;
    private int areaMask;
    [SerializeField]
    private int targetSpawnCount = 3;
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
        for(int i = 0; i < targetSpawnCount; i++)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSpawnPosition();

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, parent).GetComponent<Enemy>();
        enemy.SetNavMeshArea(areaMask);
        enemies.Add(enemy);

        enemy.onDeath += () => { enemies.Remove(enemy); };
        enemy.onDeath += () => { StartCoroutine(SpawnEnemyAfterDelay(5f)); };
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
            Debug.LogError("cannot find valid position");
            return transform.position;
        }
    }

    private IEnumerator SpawnEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy(); 
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
