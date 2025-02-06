using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOAttacks : MonoBehaviour
{
    private UFOController ufoController;
    private SpriteRenderer raybeamSprite;

    [SerializeField] private GameObject miniUfoPrefab;

    [Header("Alien Deploy Attack")]
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private int ufoAmountNormal = 3;
    [SerializeField] private int ufoAmountEnraged = 5;
    private BoxCollider2D spawnArea;

    private void Awake()
    {
        ufoController = GetComponent<UFOController>();
        if (ufoController == null)
        {
            Debug.LogError("UFOController (script) on UFOAttacks (script) not found!");
        }

        raybeamSprite = GetComponentInChildren<SpriteRenderer>();   //should get SpriteRenderer of RayBeam child object!
        if (raybeamSprite == null)
        {
            Debug.LogError("Sprite Renderer from Raybeam on UFOAttacks (script) not found!");
        }

        spawnArea = GetComponentInChildren<BoxCollider2D>();        //should get BoxCollider2D of SpawnZone child object!
        if (spawnArea == null)
        {
            Debug.LogError("BoxCollider2D from SpawnZone on UFOAttacks (script) not found!");
        }
    }

    public void DeployAliens()
    {
        int ufosAmount = ufoController.GetIsEnraged() ? ufoAmountEnraged : ufoAmountNormal;

        //Turn Raybeam red
        raybeamSprite.color = Color.red;

        //Spawn Aliens
        SpawnMiniUfos(ufosAmount);

        raybeamSprite.color = Color.white;
    }

    private void SpawnMiniUfos(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomPosition = RandomizeSpawnPosition();
            Instantiate(miniUfoPrefab, randomPosition, Quaternion.identity, enemyContainer);
        }
    }

    private Vector2 RandomizeSpawnPosition()
    {
        Bounds spawnBounds = spawnArea.bounds;

        float randomXPosition = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        float randomYPosition = Random.Range(spawnBounds.min.y, spawnBounds.max.y);

        return new Vector2(randomXPosition, randomYPosition);
    }
}
