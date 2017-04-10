﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges [(int)direction];
	}

	public void SetEdge(MazeDirection direction, MazeCellEdge edge) {
		edges [(int)direction] = edge;
	}
}