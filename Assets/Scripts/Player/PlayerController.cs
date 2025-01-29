using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 7f;  //serves as base speed
    private float defaultSpeed;                     //used to store base speed

    [SerializeField] private float fireRate = 1.5f;
    private float fireTimer = 0;

    private float borderXRange = 9.4f;
    private float topBorder = 2f;
    private float bottomBorder = -4.5f;
    private const float BORDER_OFFSET = 0.5f;

    private bool isAlive = true;

    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += HandlePlayerDeath;
        }

        bool isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            #region Player Movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector3(horizontalInput, verticalInput);
            direction.Normalize();

            transform.Translate(direction * moveSpeed * Time.deltaTime);

            #endregion

            #region Fire Laser
            if (EventSystem.current.IsPointerOverGameObject())  //prevents firing laser when clicking a UI-Button (e.g. 'Resume')
            {
                return;
            }

            fireTimer += Time.deltaTime;
            if (fireTimer > fireRate && Input.GetMouseButtonDown(0)) //Left-Click
            {
                FireLaser();
            }

            #endregion
        }

        #region Player Border
        if (transform.position.x > borderXRange)
        {
            transform.position = new Vector2(-borderXRange + BORDER_OFFSET, transform.position.y);
        }
        if (transform.position.x < -borderXRange)
        {
            transform.position = new Vector2(borderXRange - BORDER_OFFSET, transform.position.y);
        }
        if (transform.position.y <= bottomBorder)
        {
            transform.position = new Vector2(transform.position.x, bottomBorder);
        }
        if (transform.position.y >= topBorder)
        {
            transform.position = new Vector2(transform.position.x, topBorder);
        }

        #endregion
    }

    private void FireLaser()
    {
        Vector3 firePointOffset = new Vector3(0, 0.8f, 0);

        GameObject playerLaserInstance = Instantiate(laserPrefab, transform.position + firePointOffset, Quaternion.identity);

        fireTimer = 0;
    }

    private void HandlePlayerDeath()
    {
        isAlive = false;
    }

    #region Speed Boost
    public void TriggerSpeedBoost(float speedMultiplier, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(speedMultiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float speedMultiplier, float duration)
    {
        defaultSpeed = moveSpeed;
        moveSpeed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = defaultSpeed;
    }

    #endregion
}
