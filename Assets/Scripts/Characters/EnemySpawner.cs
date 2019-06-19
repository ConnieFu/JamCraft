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
        if (GUI.Button(new Rect(new Vector2(0f,300f), new Vector2(100f, 100f)), "SPAWN ENEMY"))
        {
            SpawnEnemy();
        }
    }

    public void Initialize()
    {
        m_Enemies.AddRange(GetComponentsInChildren<Enemy>());
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
                m_Enemies.Remove(enemy);
            }
        }

        if (enemy == null)
        {
            enemy = Instantiate<GameObject>((GameObject)Resources.Load(ENEMY_PREAFAB_PATH), transform).GetComponent<Enemy>();
        }

        return enemy;
    }
}
