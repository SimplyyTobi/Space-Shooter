using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniUFOEnemy : EnemyBase
{
    [SerializeField] private int health = 1;
    [SerializeField] private int scoreValue = 0;

    protected override int Health
    {
        get { return health; }
        set { health = value; }
    }

    protected override int ScoreValue
    {
        get { return scoreValue; }
    }

    protected override void Move()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    protected override void FireLaser()
    {
        
    }
}
