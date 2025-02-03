using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected GameObject laserPrefab;
    [SerializeField] private Transform playerPos;

    [Header("Enemy Settings")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float minFireRate = 2f;
    [SerializeField] protected float maxFireRate = 5f;
    protected float fireRate;
    protected float fireTimer = 0;

    protected Transform Player => playerPos;
    protected abstract int Health { get; set; }
    protected abstract int ScoreValue { get; }

    protected float borderXRange = 8f;
    protected float bottomBorder = -6f;
    protected float topBorder = 6.5f;

    protected UIManager uiManager;

    #region Methods

    protected virtual void Awake()
    {
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

    protected virtual void HandleBorders()
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
            uiManager.AddScore(ScoreValue);

            TakeDamage();
        }
    }
    
    protected virtual void TakeDamage()
    {
        this.Health--;

        if (this.Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    #endregion
}