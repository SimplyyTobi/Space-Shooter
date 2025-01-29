using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private PlayerPopUpText popUpText;

    private float bottomBorder = -6.5f;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            popUpText = player.GetComponent<PlayerPopUpText>();
            if (popUpText == null)
            {
                Debug.LogError("PlayerPopUpText (script) not found on player");
            }
        }
    }

    public virtual void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

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
            ActivateItem(other.gameObject);

            if (popUpText != null)
            {
                popUpText.DisplayMessage(GetMessage());
            }
        }
    }

    public abstract void ActivateItem(GameObject player);

    protected abstract string GetMessage();

}
