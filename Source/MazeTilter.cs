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
		private float _pitchSpeed;
		private float _yawSpeed;
		private float _rollSpeed;

		private void Start()
		{
			// Here you can add code that needs to be called when script is created
		}

		private void Update()
		{
			// Here you can add code that needs to be called every frame
			if (Input.GetKey(Keys.ArrowUp))
			{
				_rollSpeed -= Speed;
			}
			if (Input.GetKey(Keys.ArrowDown))
			{
				_rollSpeed += Speed;
			}
			if (Input.GetKey(Keys.ArrowLeft))
			{
				_pitchSpeed += Speed;
			}
			if (Input.GetKey(Keys.ArrowRight))
			{
				_pitchSpeed -= Speed;
			}

			GeneratedMaze.AngularDamping = Damping;
		}

		private Quaternion _orientation;

		private void FixedUpdate()
		{
			GeneratedMaze.AngularVelocity += new Vector3(_pitchSpeed, _yawSpeed, _rollSpeed) * Time.DeltaTime;
			_pitchSpeed = _pitchSpeed - _pitchSpeed * Damping * Time.DeltaTime;
			_yawSpeed = _yawSpeed - _yawSpeed * Damping * Time.DeltaTime;
			_rollSpeed = _rollSpeed - _rollSpeed * Damping * Time.DeltaTime;

			Maze.LocalOrientation = GeneratedMaze.LocalOrientation;
		}
	}
}