using System;
using System.Collections.Generic;
using FlaxEngine;

namespace AMaze
{
	public class MazeTilter : Script
	{
		public Actor Maze;
		public RigidBody GeneratedMaze;
		public float Damping = 0.1f;
		public float Speed = 1f;
		private float _pitch;
		private float _yaw;
		private float _roll;

		private void Start()
		{
			// Here you can add code that needs to be called when script is created
		}

		private void Update()
		{
			// Here you can add code that needs to be called every frame
			if (Input.GetKey(Keys.ArrowUp))
			{
				_roll -= Speed;
			}
			if (Input.GetKey(Keys.ArrowDown))
			{
				_roll += Speed;
			}
			if (Input.GetKey(Keys.ArrowLeft))
			{
				_pitch += Speed;
			}
			if (Input.GetKey(Keys.ArrowRight))
			{
				_pitch -= Speed;
			}

			GeneratedMaze.AngularDamping = Damping;
		}

		private void FixedUpdate()
		{
			GeneratedMaze.AngularVelocity = new Vector3(_pitch, _yaw, _roll) * Time.DeltaTime;
			/*_pitch = _pitch - _pitch * Damping * Time.DeltaTime;
			_yaw = _yaw - _yaw * Damping * Time.DeltaTime;
			_roll = _roll - _roll * Damping * Time.DeltaTime;*/

			Maze.LocalOrientation = GeneratedMaze.LocalOrientation;
		}
	}
}