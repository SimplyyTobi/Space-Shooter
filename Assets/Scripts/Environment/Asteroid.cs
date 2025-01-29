using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private float rotationDirection;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSpeedMax = 20f;
    [SerializeField] private float rotationSpeedMin = 50f;

    private float bottomBorder = -6.5f;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeRotation();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        //Not using Translate() for the movement here, since it is relative to the object's local coordinate system which constantly changes the direction due to constant rotation
        //Instead I simply constantly update the object's position
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        transform.Rotate(Vector3.forward * rotationDirection * rotationSpeed * Time.deltaTime);
        #endregion

        #region Border
        if (transform.position.y < bottomBorder)
        {
            Destroy(this.gameObject);
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(30);
                Destroy(this.gameObject);
            }
        }
    }

    private void RandomizeRotation()
    {
        //Randomize Rotation Direction (-1 or 1)
        rotationDirection = Random.Range(0, 2) * 2 - 1;

        //Randomize Rotation Speed
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }
}
