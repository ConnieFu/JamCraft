using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : PlayerInteractableBase
{
    public override void OnTouchTapped()
    {
        GameFlow.Instance.ShowCraftingMenu();
    }
}
