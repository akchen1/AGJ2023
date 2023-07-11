using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BloodDrop : MonoBehaviour
{
	private Rigidbody2D rbody;
	private Action<GameObject> onCollected;

	public void Initialize(float fallSpeed, Action<GameObject> collected)
	{
		GetComponent<Rigidbody2D>().gravityScale *= fallSpeed;

		onCollected = collected;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Vial")
		{
			onCollected.Invoke(gameObject);
		}	
	}
}
