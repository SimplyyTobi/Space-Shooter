using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    //Enemy Stats
    protected int health;
    protected float moveSpeed;
    protected float minFireRate;
    protected float maxFireRate;
    protected int scoreValue;

    protected float fireRate;
    protected float fireTimer = 0;

    protected float borderXRange = 8f;
    protected float bottomBorder = -6f;
    protected float topBorder = 6.5f;

    protected GameObject laserPrefab;
    protected UIManager uiManager;
    protected Transform playerPos;

    #region Methods

    protected virtual void Awake()
    {
        if (enemyData != null)
        {
            InitializeStats();
        }
        else
        {
            Debug.LogError($"{gameObject.name} is missing an EnemyData reference!");
        }

        if (playerPos == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerPos = playerObject.transform;
            }
            else
            {
                Debug.LogError("PlayerObject not found on Enemy Base (script)!");
            }
        }

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found on Enemy Base (script)!");
        }

        if (laserPrefab == null)
        {
            Debug.LogError("Enemy Base (script) is missing a laserPrefab reference!");
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        Move();
        HandleFiring();
        HandleBorders();
    }

    protected abstract void Move();
    protected abstract void FireLaser();

    protected virtual void HandleFiring()
    {
        if (fireTimer <= 0)
        {
            fireRate = Random.Range(minFireRate, maxFireRate);
            FireLaser();
            fireTimer = fireRate;
        }
        fireTimer -= Time.deltaTime;
    }

    protected virtual void HandleBorders()  //Respawn on top when leaving bottom of screen
    {
        if (transform.position.y <= bottomBorder)
        {
            transform.position = new Vector2(Random.Range(-borderXRange, borderXRange), topBorder);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerHealth>();
        if (other.CompareTag("Player") && player != null)
        {
            if (other != null)
            {
                player.TakeDamage(30);
                TakeDamage();
            }
        }

        else if (other.CompareTag("PlayerLaser"))
        {
            Destroy(other.gameObject);
            uiManager.AddScore(scoreValue);

            TakeDamage();
        }
    }   //Damage by Player and Player Lasers
    
    protected virtual void TakeDamage()
    {
        this.health--;

        if (this.health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected void InitializeStats()
    {
        health = enemyData.health;
        moveSpeed = enemyData.moveSpeed;
        minFireRate = enemyData.minFireRate;
        maxFireRate = enemyData.maxFireRate;
        scoreValue = enemyData.scoreValue;

        laserPrefab = enemyData.laserPrefab;
    }

    #endregion
}