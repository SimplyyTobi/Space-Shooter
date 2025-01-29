using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : ItemBase
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 7f;

    public override void ActivateItem(GameObject player)
    {
        player.GetComponent<PlayerController>().TriggerSpeedBoost(speedMultiplier, duration);
        Destroy(this.gameObject);
    }

    protected override string GetMessage()
    {
        return "Speed Boost!";
    }
}
