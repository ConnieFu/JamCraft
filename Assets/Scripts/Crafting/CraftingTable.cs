using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : PlayerInteractableBase
{
    public override void OnInteracted(CharacterController controller)
    {
        GameFlow.Instance.ShowCraftingMenu();
    }
}
