using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : EnemyBase
{
    [SerializeField] private int health = 1;
    [SerializeField] private int scoreValue = 20;
    private float stayPointY = 3.5f;

    protected override int Health
    {
        get { return health; }
        set { health = value; }
    }
    protected override int ScoreValue
    {
        get { return scoreValue; }
    }

    private void Awake()
    {
        stayPointY = Random.Range(3.5f, 4f);
    }
    protected override void Move()
    {
        if (transform.position.y > stayPointY)
        {
            //Movement relative to Space.World! Object is rotated by 180° so local direction would now be up instead of down!
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            moveSpeed = 0;
        }

        AimForTarget(Player);
    }

    protected override void FireLaser()
    {
        Quaternion laserRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180));

        GameObject enemyLaserInstance = Instantiate(laserPrefab, transform.position, laserRotation);

        LaserBehaviour laserScript = enemyLaserInstance.GetComponent<LaserBehaviour>();
        if (laserScript != null)
        {
            laserScript.SetIsEnemyLaser(true);
        }
    }
    
    private void AimForTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotation - 90));
        }
    }
}
