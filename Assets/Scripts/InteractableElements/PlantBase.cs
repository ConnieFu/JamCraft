using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBase : PlayerInteractableBase
{
    [SerializeField] private Transform m_BaseAnchor;
    [SerializeField] private PlantController m_PlantController;

    private FlowerComponent m_FlowerComponent;
    public FlowerComponent FlowerComponent
    {
        get
        {
            return m_FlowerComponent;
        }
    }

    private StemComponent m_StemComponent;
    private RootComponent m_RootComponent;

    protected bool m_IsBeingHeld = false;
    public bool IsBeingHeld
    {
        get
        {
            return m_IsBeingHeld;
        }
    }

    public override void OnTouchHold(Vector2 position)
    {
        if (!m_IsBeingHeld) // player picks up plant
        {
            m_CellXY = null;
            m_IsBeingHeld = true;

            ChangeRendererSortingLayers(GameConstants.CHARACTER_LAYER_NAME); // change this to a front layer
        }

        transform.position = position;
    }

    public override void OnEnemyHit()
    { 
        m_CurrentHits++;

        if (m_CurrentHits >= m_NumberHits)
        {
            // play death animation
            DestroySelf();
        }
    }

    public void Initialize(GameConstants.eEnergyType flower, GameConstants.eEnergyType stem, GameConstants.eEnergyType bse)
    {
        if (!m_IsBeingHeld)
        {
            base.Initialize();
        }

        // use the energy types to zombie together the final plant
        m_FlowerComponent = Instantiate<GameObject>((GameObject)Resources.Load(string.Format(GameConstants.PLANT_COMPONENT_PREFAB_PATH, GameConstants.eSlot.FLOWER, flower)), m_BaseAnchor).GetComponent<FlowerComponent>();
        m_StemComponent = Instantiate<GameObject>((GameObject)Resources.Load(string.Format(GameConstants.PLANT_COMPONENT_PREFAB_PATH, GameConstants.eSlot.STEM, stem)), m_BaseAnchor).GetComponent<StemComponent>();
        m_RootComponent = Instantiate<GameObject>((GameObject)Resources.Load(string.Format(GameConstants.PLANT_COMPONENT_PREFAB_PATH, GameConstants.eSlot.ROOT, bse)), m_BaseAnchor).GetComponent<RootComponent>();
        CreatePlant();
    }

    public override void Reset()
    {
        base.Reset();
        DestroySelf();
    }

    // TODO: maybe have specific z depths for each row of tiles so layering is easier
    // TODO: Handle placing the plant only on spaces it can be placed (ie. find the nearest placable tile)
    public override void OnTouchEnd(Vector2 position, bool wasTapped)
    {
        m_IsBeingHeld = false;
        ChangeRendererSortingLayers(GameConstants.INTERACTABLE_LAYER_NAME);

        m_CellXY = GameFlow.Instance.GridManager.WorldPosToCell(position);

        GameFlow.Instance.GridManager.InteractableTilemap.AddInteractableObject(this);
        transform.parent = GameFlow.Instance.GridManager.InteractableTilemap.PlantAnchor;
        transform.position = GameFlow.Instance.GridManager.GetCellWorldPos(m_CellXY.Value);
    }

    // change the sorting layer so the objects are seen nicely in the scene
    private void ChangeRendererSortingLayers(string layerName)
    {
        m_FlowerComponent.ChangeRenderingLayerInfo(layerName);
        m_StemComponent.ChangeRenderingLayerInfo(layerName);
        m_RootComponent.ChangeRenderingLayerInfo(layerName);
    }

    private void CreatePlant()
    {
        m_RootComponent.transform.localPosition = Vector3.zero;
        m_StemComponent.transform.position = m_RootComponent.StemAnchor.transform.position;
        m_FlowerComponent.transform.position = m_StemComponent.FlowerAnchor.transform.position;

        PlantController.sPlantData plantInfo = new PlantController.sPlantData();
        // set damagetype
        plantInfo.damageType = m_FlowerComponent.DamageType;
        // set damage amount and modifiers
        plantInfo.damageAmt = (m_FlowerComponent.DamageAmount + m_StemComponent.DamageAmount + m_RootComponent.DamageAmount) * m_RootComponent.DamageModifier;
        // set attack speed
        plantInfo.attackSpeed = m_FlowerComponent.AttackSpeed * m_RootComponent.AttackSpeedModifier;
        // set can attack
        plantInfo.canAttack = m_StemComponent.CanAttack;
        // set is aoe
        plantInfo.isAOE = m_StemComponent.IsAOE;
        // set health and modifiers
        plantInfo.health = (m_FlowerComponent.HealthAmount + m_StemComponent.HealthAmount + m_RootComponent.HealthAmount) * m_RootComponent.HealthModifier;
        m_NumberHits = plantInfo.health;
        // set range
        plantInfo.attackRange = m_StemComponent.AttackRange;
        // set attack start position
        plantInfo.attackPosition = m_FlowerComponent.AttackStartAnchor;

        m_PlantController.SetPlantInfo(plantInfo, this);
    }
}
