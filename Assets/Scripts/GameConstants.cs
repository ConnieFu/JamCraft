using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const string MAIN_MENU_SCENE_NAME = "MainMenu";
    public const string PAUSE_MENU_SCENE_NAME = "PauseMenu";
    public const string GAME_OVER_MENU_SCENE_NAME = "GameOverMenu";
    public const string CRAFTING_MENU_NAME = "CraftingMenu";

    public const string ENERGY_PREFAB_PATH = "Prefabs/CraftingEnergy/{0}Energy";
    public const string STATIC_ENERGY_PREFAB_PATH = "Prefabs/CraftingEnergy/EnergyStatic/{0}Energy";

	public const string BASE_PLANT_PREFAB_PATH = "Prefabs/PlantComponents/PlantPrefab";
	public const string PLANT_COMPONENT_PREFAB_PATH = "Prefabs/PlantComponents/{0}/{1}_{0}";

    public const string PLAYER_PICKUP_COLLIDER_TAG = "PlayerPickUp";
    public const string PLAYER_TAG = "Player";

    public const string INTERACTABLE_LAYER_NAME = "Interactable";
    public const string CHARACTER_LAYER_NAME = "Character";

    public const float PROJECTILE_SPEED = 0.25f;
    public const float PROJECTILE_HIT_DISTANCE = 0.25f;
    public const float ENERGY_WEAKNESS_DAMAGE_MODIFIER = 2.0f;

    public static Vector3Int TEMP_NODE_POSITON = new Vector3Int(-1, 4, 0);

    public enum eEnergyType
    {
        DEFAULT = 0,
        FIRE,
        WATER,
        EARTH,
        AIR
    }

    public static eEnergyType GetEnergyWeakness(eEnergyType type)
    {
        switch(type)
        {
            case eEnergyType.FIRE:
                return eEnergyType.WATER;
            case eEnergyType.WATER:
                return eEnergyType.FIRE;
            case eEnergyType.EARTH:
                return eEnergyType.AIR;
            case eEnergyType.AIR:
                return eEnergyType.EARTH;
            default:
                return eEnergyType.DEFAULT;
        }
    }

    public enum eSlot
    {
        FLOWER = 0,
        STEM,
        ROOT
    }

    public static List<Vector3Int> FOUR_DIRECTIONS = new List<Vector3Int>()
    {
        Vector3Int.up,
        Vector3Int.left,
        Vector3Int.down,
        Vector3Int.right
    };
}
