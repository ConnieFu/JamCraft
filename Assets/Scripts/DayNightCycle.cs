using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: This class can be optimized better to support difficulty scaling. Won't do that for the jam though...keeping it simple :D
public class DayNightCycle : MonoBehaviour
{
    private const float TIME_MODIFIER = 0.5f;
    private const float HOURS_IN_DAY = 24.0f;

    [SerializeField] private float m_SunlightHours = 16f;

    [Header("Enemies")]
    [SerializeField] private EnemySpawner m_EnemySpawner;
    [SerializeField] private Vector2Int m_MinEnemiesToSpawn = new Vector2Int(1, 3);
    [SerializeField] private Vector2 m_EnemySpawnIntervals = new Vector2(4.0f, 7.0f);
    private float m_EnemySpawnTimer;

    [Header("Resources")]
    [SerializeField] private ResourceSpawner m_ResourceSpawner;
    [SerializeField] private Vector2Int m_NumberResourcesToSpawn = new Vector2Int(1, 3);
    [SerializeField] private Vector2 m_ResourceSpawnIntervals = new Vector2(15.0f, 20.0f);
    private float m_ResourceSpawnTimer;

    private bool m_IsDaytime = true;
    private float m_CurrentTime = 0.0f;

    private int m_NumberNights = 0; // use this to amp up difficulty as the nights progress

    public void Initialize()
    {
        m_EnemySpawner.Initialize();
        m_ResourceSpawner.Initialize();
    }

    public void Reset()
    {
        m_EnemySpawner.Reset();
        m_ResourceSpawner.Reset();

        GameFlow.Instance.ToggleNight(false);
        m_IsDaytime = false;
        m_CurrentTime = 0.0f;

        m_NumberNights = 0;
    }

    private void FixedUpdate()
    {
        if (!GameFlow.Instance.IsPaused)
        {
            m_CurrentTime += (Time.deltaTime * TIME_MODIFIER);

            if (m_CurrentTime <= m_SunlightHours)
            {
                if (!m_IsDaytime)
                {
                    m_IsDaytime = true;
                    GameFlow.Instance.ToggleNight(!m_IsDaytime);
                    m_ResourceSpawnTimer = Random.Range(m_ResourceSpawnIntervals.x, m_ResourceSpawnIntervals.y);
                }
                SpawnResources();
            }
            else
            {
                if (m_IsDaytime)
                {
                    m_NumberNights++;

                    m_IsDaytime = false;
                    GameFlow.Instance.ToggleNight(!m_IsDaytime);
                    m_EnemySpawnTimer = Random.Range(m_EnemySpawnIntervals.x, m_EnemySpawnIntervals.y);
                }
                SpawnEnemies();
            }

            if (m_CurrentTime >= HOURS_IN_DAY)
            {
                m_CurrentTime = 0.0f;
            }
        }
    }

    private void SpawnEnemies()
    {
        m_EnemySpawnTimer -= Time.deltaTime;
        if (m_EnemySpawnTimer <= 0.0f)
        {
            int numberEnemiesToSpawn = (Random.Range(m_MinEnemiesToSpawn.x, m_MinEnemiesToSpawn.y + 1) + (m_NumberNights)) / 2;

            StartCoroutine(m_EnemySpawner.SpawnEnemies(numberEnemiesToSpawn));
           
            m_EnemySpawnTimer = Random.Range(m_EnemySpawnIntervals.x, m_EnemySpawnIntervals.y);
        } 
    }

    private void SpawnResources()
    {
        m_ResourceSpawnTimer -= Time.deltaTime;
        if (m_ResourceSpawnTimer <= 0.0f)
        {
            int numberResourceTypesToSpawn = Random.Range(1, 3);

            for (int i = 0; i < numberResourceTypesToSpawn; i++)
            {
                GameConstants.eEnergyType type = (GameConstants.eEnergyType)Random.Range(0, System.Enum.GetValues(typeof(GameConstants.eEnergyType)).Length);
                int numberResources = Random.Range(m_NumberResourcesToSpawn.x, m_NumberResourcesToSpawn.y + 1);
                StartCoroutine(m_ResourceSpawner.SpawnResources(type, numberResources));
            }

            m_ResourceSpawnTimer = Random.Range(m_ResourceSpawnIntervals.x, m_ResourceSpawnIntervals.y);
        }
    }
}
