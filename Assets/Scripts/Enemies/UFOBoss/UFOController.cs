using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private UFOAttacks ufoAttacks;

    [SerializeField] private Sprite[] ufoSprites;
    private int currentSpriteIndex;

    [Header("Boss Settings")]
    [SerializeField] private int health = 25;   //something divisible by five, since boss UI has five UI states (lights of UFO)
    private int healthBeforeNextSprite;
    private int healthDividedByFive;
    [SerializeField] private float moveSpeedNormal = 5f;
    [SerializeField] private float moveSpeedEnraged = 10f;
    [SerializeField] private float movementRangeX = 3f;
    private float moveSpeed;
    [SerializeField] private float attackIntervalNormal = 5f;
    [SerializeField] private float attackIntervalEnraged = 3f;
    private float attackInterval;

    [SerializeField] private bool isEnraged = false;
    [SerializeField] private bool isAlive = true;

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
        currentSpriteIndex = 0;
        healthDividedByFive = health / 5;
        healthBeforeNextSprite = healthDividedByFive;

        StartCoroutine(StartMovement());
        StartCoroutine(AttackLoop());
    }

    private void TakeDamage()
    {
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

            healthBeforeNextSprite = healthDividedByFive;
        }
    }

    private void ChangeSprite(int index)
    {
        if (currentSpriteIndex < ufoSprites.Length)
        {
            spriteRenderer.sprite = ufoSprites[index];
        }
    }

    private void Die()
    {
        isAlive = false;
        Debug.Log("Boss defeated!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerLaser"))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }

    private IEnumerator StartMovement()
    {
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
        int attackIndex = Random.Range(0, 3);   //Three different attacks

        switch(attackIndex)
        {
            //ToDo
            case 0:
                ufoAttacks.DeployAliens();
                break;

        }
    }

    public bool GetIsEnraged()
    {
        return isEnraged;
    }
}
