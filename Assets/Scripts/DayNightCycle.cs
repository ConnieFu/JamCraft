using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private const float TIME_MODIFIER = 0.5f;
    private const float HOURS_IN_DAY = 24.0f;

    [SerializeField] private float m_SunlightHours = 16f;

    [Header("Enemies")]
    [SerializeField] private EnemySpawner m_EnemySpawner;
    [SerializeField] private Vector2Int m_MinEnemiesToSpawn = new Vector2Int(1, 3);
    [SerializeField] private Vector2 m_SpawnIntervals = new Vector2(4.0f, 7.0f);
    private float m_SpawnTimer;

    private bool m_IsDaytime = true;
    private float m_CurrentTime = 0.0f;

    private int m_NumberNights = 0; // use this to amp up difficulty as the nights progress

    public void Initialize()
    {
        m_EnemySpawner.Initialize();
    }

    public void Reset()
    {
        m_EnemySpawner.Reset();

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
                    StopCoroutine(m_EnemySpawner.SpawnEnemies(0));
                }
            }
            else
            {
                if (m_IsDaytime)
                {
                    m_NumberNights++;

                    m_IsDaytime = false;
                    GameFlow.Instance.ToggleNight(!m_IsDaytime);
                    m_SpawnTimer = Random.Range(m_SpawnIntervals.x, m_SpawnIntervals.y);
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
        m_SpawnTimer -= Time.deltaTime;
        if (m_SpawnTimer <= 0.0f)
        {
            int numberEnemiesToSpawn = Random.Range(m_MinEnemiesToSpawn.x, m_MinEnemiesToSpawn.y + 1) * (m_NumberNights + 1);

            StartCoroutine(m_EnemySpawner.SpawnEnemies(numberEnemiesToSpawn));
           
            m_SpawnTimer = Random.Range(m_SpawnIntervals.x, m_SpawnIntervals.y);
        } 
    }
}
