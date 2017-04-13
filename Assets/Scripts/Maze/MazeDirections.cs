﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection {
	North,
	West,
	South,
	East
}

public static class MazeDirections {

	public const int Count = 4;

	private static IntVector2[] vectors = {
		new IntVector2 (0, 1),
		new IntVector2 (-1, 0),
		new IntVector2 (0, -1),
		new IntVector2 (1, 0)
	};

	private static MazeDirection[] oposites = {
		MazeDirection.South,
		MazeDirection.East,
		MazeDirection.North,
		MazeDirection.West
	};

	private static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 270f, 0f),
		Quaternion.Euler (0f, 180f, 0f),
		Quaternion.Euler (0f, 90f, 0f)
	};

	public static MazeDirection RandomDirection {
		get {
			return (MazeDirection)Random.Range (0, Count);
		}
	}

	public static MazeDirection GetOposite (this MazeDirection direction) {
		return oposites [(int)direction];
	}

	public static Quaternion ToRotation (this MazeDirection direction) {
		return rotations [(int)direction];
	}

	public static MazeDirection GetNextCounterclockwise (this MazeDirection direction) {
		return (MazeDirection) (((int)direction +1) % Count);
	}

	public static MazeDirection GetNextClockwise (this MazeDirection direction) {
		return (MazeDirection) (((int)direction +Count -1) % Count);
	}

	public static IntVector2 ToIntVector2 (this MazeDirection direction) {
		return vectors [(int)direction];
	}
}
