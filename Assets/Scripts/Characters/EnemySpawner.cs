using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const string ENEMY_PREAFAB_PATH = "Prefabs/Enemy";

    [SerializeField] private Grid m_Grid;

    [Header("")]
    [SerializeField] private RectInt m_SpawningArea;
   
    private List<Enemy> m_Enemies = new List<Enemy>();

    private void OnGUI()
    {
        if (!GameFlow.Instance.IsPaused)
        {
            if (GUI.Button(new Rect(new Vector2(0f, 300f), new Vector2(100f, 100f)), "SPAWN ENEMIES"))
            {
                SpawnEnemies(2);
            }
        }
    }

    public void Initialize()
    {
        m_Enemies.AddRange(GetComponentsInChildren<Enemy>());
        foreach (Enemy enemy in m_Enemies)
        {
            enemy.Initialize();
        }
    }

    public void Reset()
    {
        foreach(Enemy enemy in m_Enemies)
        {
            enemy.Reset();
        }
    }

    public void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // get random point in spawning area
        Vector3Int spawnCell = GameFlow.Instance.GridManager.GetRandomCell(m_SpawningArea);

        GetAvailableEnemy().OnSpawned(spawnCell, m_Grid);
    }

    private Enemy GetAvailableEnemy()
    {
        Enemy enemy = null;

        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (!m_Enemies[i].IsAlive)
            {
                enemy = m_Enemies[i];
            }
        }

        if (enemy == null)
        {
            enemy = Instantiate<GameObject>((GameObject)Resources.Load(ENEMY_PREAFAB_PATH), transform).GetComponent<Enemy>();
            enemy.Initialize();
            m_Enemies.Add(enemy);
        }

        return enemy;
    }
}
