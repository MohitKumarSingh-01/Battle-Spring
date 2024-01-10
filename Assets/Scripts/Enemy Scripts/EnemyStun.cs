﻿using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour 
{
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			this.GetComponentInParent<Enemy>().Stunned();
		}
	}
}