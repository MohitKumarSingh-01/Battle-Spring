using UnityEngine;
using System.Collections;
public class DeathZone : MonoBehaviour
{
    public bool destroyNonPlayerObjects = true;

    // Handle gameobjects collider with a deathzone object
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControler>().FallDeath();
        }
        else if (destroyNonPlayerObjects)
        {
            Destroy(other.gameObject);
        }
    }
}