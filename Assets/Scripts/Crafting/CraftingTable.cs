using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : PlayerInteractableBase
{
    public override void OnInteracted(CharacterBase controller)
    {
        GameFlow.Instance.ShowCraftingMenu();
    }
}
