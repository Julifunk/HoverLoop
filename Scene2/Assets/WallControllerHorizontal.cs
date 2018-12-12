using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControllerHorizontal : MonoBehaviour
{

	public float Speed = 15f;

	public float StartPosition;
	public float EndPosition;

	private float _direction = 1f;

	private void Update()
	{
		if ( transform.position.x < StartPosition || transform.position.x > EndPosition)
		{
			_direction *= -1;
		}
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		transform.Translate(Speed * Time.deltaTime * _direction,0 ,0);
	}
	
    
}
