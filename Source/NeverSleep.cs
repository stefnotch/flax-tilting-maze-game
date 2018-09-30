using System;
using System.Collections.Generic;
using FlaxEngine;

namespace AMaze
{
	public class NeverSleep : Script
	{
		private RigidBody _rigidBody;

		private void Start()
		{
			if (Actor is RigidBody body)
			{
				_rigidBody = body;
				_rigidBody.SleepThreshold = -1f;
			}
		}
	}
}