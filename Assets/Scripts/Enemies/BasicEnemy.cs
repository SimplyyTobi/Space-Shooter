using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    [SerializeField] private int health = 1;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float moveSpeedIncrease = 1f;
    [SerializeField] private float moveSpeedMax = 7f;

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
        Vector3 laserOffset = new Vector3(0, -0.8f, 0);
        GameObject enemyLaserInstance = Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);

        LaserBehaviour laserScript = enemyLaserInstance.GetComponent<LaserBehaviour>();
        if (laserScript != null)
        {
            laserScript.SetIsEnemyLaser(true);
        }
    }

    protected override void HandleBorders()
    {
        //Override: Enemy gets faster when going out of range and reappearing
        if (transform.position.y <= bottomBorder)
        {
            transform.position = new Vector2(Random.Range(-borderXRange, borderXRange), topBorder);

            if (moveSpeed < moveSpeedMax)
            {
                moveSpeed += moveSpeedIncrease;
            }
        }
    }
}
