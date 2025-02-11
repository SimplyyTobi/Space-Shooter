using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class UFOController : MonoBehaviour, IBoss
{
    public event Action OnBossDefeated;

    private SpriteRenderer spriteRenderer;
    private UFOAttacks ufoAttacks;

    [SerializeField] private Sprite[] ufoSprites;
    private int currentSpriteIndex;

    [Header("Boss Settings")]
    [SerializeField] private int health = 25;   //something divisible by five, since boss UI has five UI states (lights of UFO)
    private int healthBeforeNextSprite;
    private int healthPerSprite;
    [SerializeField] private float moveSpeedNormal = 5f;
    [SerializeField] private float moveSpeedEnraged = 10f;
    [SerializeField] private float movementRangeX = 3f;
    private float moveSpeed;
    [SerializeField] private float attackIntervalNormal = 5f;
    [SerializeField] private float attackIntervalEnraged = 3f;
    private float attackInterval;

    [SerializeField] private bool isEnraged = false;
    [SerializeField] private bool isAlive = true;
    private bool isInvulnerable;

    [Header("Intro Sequence")]
    [SerializeField] private float stayPointY = 3.75f;
    [SerializeField] private float introMoveSpeed = 0.7f;
    [SerializeField] private float introWait = 2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer on UFOController (script) not found!");
        }

        ufoAttacks = GetComponent<UFOAttacks>();
        if (ufoAttacks == null)
        {
            Debug.LogError("UFOAttacks (script) on UFOController (script) not found!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isInvulnerable = true;
        currentSpriteIndex = 0;
        healthPerSprite = health / 5;
        healthBeforeNextSprite = healthPerSprite;

        StartCoroutine(IntroMovement());
    }

    private IEnumerator IntroMovement()
    {
        Vector3 targetPoint = new Vector3(0, stayPointY, transform.position.z);

        while (Vector3.Distance(transform.position, targetPoint) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, introMoveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(introWait);

        isInvulnerable = false;
        StartCoroutine(MovementLoop());
        StartCoroutine(AttackLoop());
    }

    private IEnumerator MovementLoop()
    {
        //Moves left and right between to y-positions
        Vector3 direction = Vector3.right;

        while (isAlive)
        {
            moveSpeed = isEnraged ? moveSpeedEnraged : moveSpeedNormal;

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (transform.position.x >= movementRangeX)
            {
                direction = Vector3.left;
            }

            else if (transform.position.x <= -movementRangeX)
            {
                direction = Vector3.right;
            }

            yield return null;
        }
    }

    private IEnumerator AttackLoop()
    {
        while (isAlive)
        {
            attackInterval = isEnraged ? attackIntervalEnraged : attackIntervalNormal;

            RandomAttack();

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void RandomAttack()
    {
        int attackIndex = UnityEngine.Random.Range(0, 3);   //Three different attacks

        switch (attackIndex)
        {
            //ToDo
            case 0:
                ufoAttacks.DeployAliens();
                break;

        }
    }

    private void TakeDamage()
    {
        if (isInvulnerable) return;             //Cannot take damage

        health--;                               //Boss health overall
        if (health <= 0)
        {
            Die();
        }

        if (health <= 10 && !isEnraged)         //for now hardcoded
        {
            isEnraged = true;
        }

        healthBeforeNextSprite--;               //Counter until next sprite change
        if (healthBeforeNextSprite == 0)
        {
            currentSpriteIndex++;               //Increase sprite index
            ChangeSprite(currentSpriteIndex);

            healthBeforeNextSprite = healthPerSprite;
        }
    }

    private void Die()
    {
        isAlive = false;
        OnBossDefeated?.Invoke();

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerLaser"))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }

    private void ChangeSprite(int index)
    {
        if (currentSpriteIndex < ufoSprites.Length)
        {
            spriteRenderer.sprite = ufoSprites[index];
        }
    }

    public bool GetIsEnraged()
    {
        return isEnraged;
    }
}
