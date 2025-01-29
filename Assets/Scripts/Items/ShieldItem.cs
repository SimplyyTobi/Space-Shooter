using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : ItemBase
{
    public override void ActivateItem(GameObject player)
    {
        player.GetComponent<PlayerHealth>().ActivateShield();
        Destroy(this.gameObject);
    }
    protected override string GetMessage()
    {
        return "Shield activated!";
    }
}
