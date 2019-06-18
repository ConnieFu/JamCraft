using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBase : PlayerInteractableBase
{
    [SerializeField] private Transform m_BaseAnchor;

    private List<SpriteRenderer> m_Renderers = new List<SpriteRenderer>();

    private FlowerComponent m_FlowerComponent;
    private StemComponent m_StemComponent;
    private BaseComponent m_BaseComponent;

    protected bool m_IsBeingHeld = false;

    public override void OnInteracted(CharacterController controller)
    {
        if (string.Compare(controller.tag, GameConstants.PLAYER_TAG) == 0)
        {
            if (!m_IsBeingHeld) // player picks up plant
            {
                controller.PickUpPlant(this);
                m_CellXY = null;
                m_IsBeingHeld = true;

                ChangeRendererSortingLayers(GameConstants.PLAYER_LAYER_NAME);
            }
        }
        else
        {
            m_CurrentHits++;

            if (m_CurrentHits >= m_NumberHits)
            {
                // play death animation
                DestroySelf();
            }
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
        m_BaseComponent = Instantiate<GameObject>((GameObject)Resources.Load(string.Format(GameConstants.PLANT_COMPONENT_PREFAB_PATH, GameConstants.eSlot.BASE, bse)), m_BaseAnchor).GetComponent<BaseComponent>();
        CreatePlant();
    }

    public void PlacePlant(Vector3Int cell)
    {
        m_IsBeingHeld = false;

        ChangeRendererSortingLayers(GameConstants.INTERACTABLE_LAYER_NAME);

        m_CellXY = cell;

        GameFlow.Instance.InteractableTilemap.AddInteractableObject(this);
        transform.parent = GameFlow.Instance.InteractableTilemap.PlantAnchor;
        transform.position = GameFlow.Instance.Grid.GetCellCenterWorld(cell);
    }

    private void ChangeRendererSortingLayers(string layerName)
    {
        m_FlowerComponent.ChangeRenderingLayerInfo(layerName, 2);
        m_StemComponent.ChangeRenderingLayerInfo(layerName, 1);
        m_BaseComponent.ChangeRenderingLayerInfo(layerName, 0);
    }

    private void CreatePlant()
    {
        m_BaseComponent.transform.localPosition = Vector3.zero;
        m_StemComponent.transform.position = m_BaseComponent.StemAnchor.transform.position;
        m_FlowerComponent.transform.position = m_StemComponent.FlowerAnchor.transform.position;
    }
}
