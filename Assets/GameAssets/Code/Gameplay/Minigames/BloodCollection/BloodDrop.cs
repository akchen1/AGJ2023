using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BloodDrop : MonoBehaviour
{
	private Rigidbody2D rbody;
	private Action<GameObject> onCollected;
	private Action<GameObject> onOutOfBounds;

	public void Initialize(float fallSpeed, Action<GameObject> collected, Action<GameObject> outOfBounds)
	{
		GetComponent<Rigidbody2D>().gravityScale *= fallSpeed;

		onCollected = collected;
		onOutOfBounds = outOfBounds;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Vial")
		{
			onCollected.DynamicInvoke(gameObject);
		}	
	}

	private void OnBecameInvisible()
	{
		onOutOfBounds.DynamicInvoke(gameObject);
	}

}
