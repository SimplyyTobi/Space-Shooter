using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] ufoSprites;
    private int currentSpriteIndex;

    [Header("Boss Settings")]
    [SerializeField] private int health = 25;   //something divisible by five, since boss UI has five UI states (lights of UFO)
    private int healthBeforeNextSprite;
    private int healthDividedByFive;
    [SerializeField] private float moveSpeedNormal = 5f;
    [SerializeField] private float moveSpeedEnraged = 10f;
    [SerializeField] private float attackIntervalNormal = 5f;
    [SerializeField] private float attackIntervalEnraged = 3f;

    private bool isEnraged = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer on UFOController (script) not found!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpriteIndex = 0;
        healthDividedByFive = health / 5;
        healthBeforeNextSprite = healthDividedByFive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Take Damage")]
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
}
