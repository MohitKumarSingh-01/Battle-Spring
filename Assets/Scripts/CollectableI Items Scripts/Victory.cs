using UnityEngine;
using System.Collections;

public class Victory : MonoBehaviour 
{
	public bool taken = false;
	public GameObject explosion;

	void OnTriggerEnter2D (Collider2D other)
	{
		if ((other.tag == "Player" ) && (!taken) && (other.gameObject.GetComponent<PlayerControler>().playerCanMove))
		{
			taken = true;

			if (explosion)
			{
				Instantiate(explosion,transform.position,transform.rotation);
			}

			other.gameObject.GetComponent<PlayerControler>().Victory();

			Destroy(this.gameObject);
		}
	}
}
