using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private float topBorder = 6.5f;
    private float bottomBorder = -6.5f;

    private bool isEnemyLaser;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = isEnemyLaser ? "EnemyLaser" : "PlayerLaser";
    }

    // Update is called once per frame
    void Update()
    {
        #region Laser Movement
        Vector3 direction = isEnemyLaser ? Vector3.down : Vector3.up;

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        #endregion

        #region Border
        if (transform.position.y > topBorder || transform.position.y < bottomBorder)
        {
            Destroy(this.gameObject);
        }
        

        #endregion
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isEnemyLaser)
        {
            if (other != null)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(10);
            }

            Destroy(this.gameObject);
        }

        if (other.CompareTag("Asteroid"))
        {
            if (other != null)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void SetIsEnemyLaser(bool belongsToEnemy)
    {
        isEnemyLaser = belongsToEnemy ? true : false;
    }
}
