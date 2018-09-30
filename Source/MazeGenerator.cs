using System;
using System.Threading;
using FlaxEngine;
using FlaxEngine.Utilities;

namespace AMaze
{
	// Used this tutorial http://weblog.jamisbuck.org/2011/1/12/maze-generation-recursive-division-algorithm

	public class MazeGenerator : Script
	{
		private static Random _rng = new Random();

		private Wall[,] _maze;

		private Int2 _resolution = new Int2(2);

		private Actor _mazeActor;

		private enum Orientation
		{
			Horizontal,
			Vertical
		}

		[Flags]
		private enum Wall
		{
			South = 1,
			East = 2
		}

		public Vector2 CellSize { get; set; }

		public Int2 Resolution
		{
			get => _resolution;
			set
			{
				value = Int2.Max(value, new Int2(2));
				_resolution = value;
			}
		}

		public Prefab MazeWall;

		private void Start()
		{
			_maze = new Wall[Resolution.X, Resolution.Y];

			_mazeActor = Actor;
			//_mazeActor = Actor.AddChild<EmptyActor>();
			//_mazeActor.HideFlags = HideFlags.DontSave;

			SubdivideMaze(_maze, new Int2(0, 0), Resolution, ChooseOrientation(Resolution));
			//DisplayMaze(_maze);

			/*new Thread(() =>
			{
				SubdivideMaze(_maze, new Int2(0, 0), Resolution, ChooseOrientation(Resolution));
			}).Start();*/
		}

		private void SubdivideMaze(Wall[,] maze, Int2 position, Int2 mazeSize, Orientation orientation)
		{
			// We can't subdivide it anymore
			if (mazeSize.X < 2 || mazeSize.Y < 2) return;

			bool isHorizontal = orientation == Orientation.Horizontal;

			int randomWallIndex = _rng.Next((isHorizontal ? mazeSize.Y : mazeSize.X) - 2);
			Int2 wallPosition = position + new Int2(
					isHorizontal ? 0 : randomWallIndex,
					isHorizontal ? randomWallIndex : 0
				);

			int randomPassageIndex = _rng.Next(isHorizontal ? mazeSize.X : mazeSize.Y);
			Int2 passagePosition = wallPosition + new Int2(
				   isHorizontal ? randomPassageIndex : 0,
				   isHorizontal ? 0 : randomPassageIndex
			   );

			Int2 wallDirection = isHorizontal ? new Int2(1, 0) : new Int2(0, 1);

			int wallLength = isHorizontal ? mazeSize.X : mazeSize.Y;

			Wall perpendicularToWall = isHorizontal ? Wall.South : Wall.East;

			for (int i = 0; i < wallLength; i++)
			{
				if (wallPosition != passagePosition)
				{
					maze[wallPosition.X, wallPosition.Y] |= perpendicularToWall;
					DisplayWall(wallPosition, perpendicularToWall);
				}
				wallPosition += wallDirection;
			}

			Int2 newPosition = position;
			Int2 newSize = isHorizontal ?
				new Int2(mazeSize.X, wallPosition.Y - position.Y + 1) :
				new Int2(wallPosition.X - position.X + 1, mazeSize.Y);

			SubdivideMaze(maze, newPosition, newSize, ChooseOrientation(newSize));

			newPosition = isHorizontal ?
				new Int2(position.X, wallPosition.Y + 1) :
				new Int2(wallPosition.X + 1, position.Y);

			newSize = isHorizontal ?
				new Int2(mazeSize.X, position.Y + mazeSize.Y - wallPosition.Y - 1) :
				new Int2(position.X + mazeSize.X - wallPosition.X - 1, mazeSize.Y);

			SubdivideMaze(maze, newPosition, newSize, ChooseOrientation(newSize));
		}

		private Orientation ChooseOrientation(Int2 size)
		{
			float k = (size.X / (float)size.Y);

			float randomValue = k * _rng.NextFloat();

			if (randomValue < 0.5)
			{
				return Orientation.Horizontal;
			}
			else
			{
				return Orientation.Vertical;
			}
		}

		private static readonly Quaternion East = Quaternion.RotationY(Mathf.PiOverTwo);

		private void DisplayWall(Int2 position, Wall wall)
		{
			Vector2 wallPos = new Vector2(position.X * CellSize.X, position.Y * CellSize.Y);

			if (wall.HasFlag(Wall.South))
			{
				var wallActor = PrefabManager.SpawnPrefab(MazeWall, _mazeActor);
				wallActor.HideFlags = HideFlags.DontSave;
				wallActor.LocalPosition = new Vector3(wallPos.X, 0, wallPos.Y + CellSize.Y * 0.5f);
			}

			if (wall.HasFlag(Wall.East))
			{
				var wallActor = PrefabManager.SpawnPrefab(MazeWall, _mazeActor);
				wallActor.HideFlags = HideFlags.DontSave;
				wallActor.LocalPosition = new Vector3(wallPos.X + CellSize.X * 0.5f, 0, wallPos.Y);
				wallActor.LocalOrientation = East;
			}
		}

		private void DisplayMaze(Wall[,] maze)
		{
			if (!MazeWall) return;

			for (int x = 0; x < Resolution.X; x++)
			{
				for (int y = 0; y < Resolution.Y; y++)
				{
					DisplayWall(new Int2(x, y), maze[x, y]);
				}
			}
		}
	}
}