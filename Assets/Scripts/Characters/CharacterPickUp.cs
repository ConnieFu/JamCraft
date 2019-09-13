using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPickUp : MonoBehaviour
{
    [SerializeField] private EnergyManager m_EnergyManager;

    [Header("Energy Particles")]
    [SerializeField] private ParticleSystem m_DefaultPickUpParticles;
    [SerializeField] private ParticleSystem m_FirePickUpParticles;
    [SerializeField] private ParticleSystem m_WaterPickUpParticles;
    [SerializeField] private ParticleSystem m_EartPickUpParticles;
    [SerializeField] private ParticleSystem m_AirPickUpParticles;

    private Dictionary<GameConstants.eEnergyType, ParticleSystem> m_ParticlesDictionary = new Dictionary<GameConstants.eEnergyType, ParticleSystem>();

    public EnergyManager EnergyManager
    {
        get
        {
            return m_EnergyManager;
        }
    }

    public void Initialize()
    {
        //m_EnergyManager.Initialize();

        m_ParticlesDictionary.Add(GameConstants.eEnergyType.DEFAULT, m_DefaultPickUpParticles);
        m_ParticlesDictionary.Add(GameConstants.eEnergyType.FIRE, m_FirePickUpParticles);
        m_ParticlesDictionary.Add(GameConstants.eEnergyType.WATER, m_WaterPickUpParticles);
        m_ParticlesDictionary.Add(GameConstants.eEnergyType.EARTH, m_EartPickUpParticles);
        m_ParticlesDictionary.Add(GameConstants.eEnergyType.AIR, m_AirPickUpParticles);
    }

    public void OnResourcePickedUp(GameConstants.eEnergyType energy)
    {
		m_EnergyManager.AddEnergy(energy);

        if (m_ParticlesDictionary.ContainsKey(energy))
        {
            m_ParticlesDictionary[energy].Emit(1);
        }
    }
}
