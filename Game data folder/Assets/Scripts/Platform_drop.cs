using System.Collections;
using UnityEngine;

public class PlatformDropper : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    [SerializeField] private Collider2D playerCollider; // Change to Collider2D to handle all types of colliders

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D[] platformColliders = currentOneWayPlatform.GetComponents<Collider2D>();

        foreach (Collider2D collider in platformColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, collider);
        }

        yield return new WaitForSeconds(0.5f); // Increase the wait time if necessary

        foreach (Collider2D collider in platformColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, collider, false);
        }
    }

}
