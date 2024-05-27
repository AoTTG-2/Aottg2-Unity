using UnityEngine;
using System.Collections;

// Just a quick circle movement sample

namespace TTTSamples
{

	public class CircleMoverSample : MonoBehaviour 
	{

		private float moveSpeed=150f;
		private float torque = 0;
		private Rigidbody2D rb;

		void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			torque=Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime;
		}

		void FixedUpdate () 
		{
			rb.AddTorque(-torque);
		}
	}
}