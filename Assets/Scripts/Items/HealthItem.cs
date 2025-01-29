using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : ItemBase
{
    [SerializeField] private int healAmount = 20;
    public override void ActivateItem(GameObject player)
    {
        player.GetComponent<PlayerHealth>().Heal(healAmount);
        Destroy(this.gameObject);
    }

    protected override string GetMessage()
    {
        return "Energy restored!";
    }
}
