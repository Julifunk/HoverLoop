using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControllerVertical : MonoBehaviour
{

	public float Speed = 15f;

	public float StartPosition;
	public float EndPosition;

	private float _direction = 1f;

	private void Update()
	{
		if ( transform.position.y < StartPosition)
		{
			_direction = 1;
		} else if ( transform.position.y > EndPosition )
		{
			_direction = -1;
		}
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		transform.Translate(0,Speed * Time.deltaTime * _direction ,0);
	}
	
    
}
