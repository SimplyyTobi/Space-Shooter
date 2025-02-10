using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniUFOEnemy : EnemyBase
{
    protected override void Move()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    protected override void FireLaser()
    {

    }
}
