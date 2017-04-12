﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage {

	public Transform hinge;

	public MazeDoor OtherSideOfDoor {
		get {
			return neighborCell.GetEdge (direction.GetOposite ()) as MazeDoor;
		}
	}

	public override void Initialize (MazeCell thisCell, MazeCell thatCell, MazeDirection direction)
	{
		base.Initialize (thisCell, thatCell, direction);
		MazeDoor opositeDoor = OtherSideOfDoor;
		if (opositeDoor != null) {
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 pos = hinge.localPosition;
			pos.x = -pos.x;
			hinge.localPosition = pos;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild (i);
			if (child != hinge) {
				child.GetComponent<Renderer> ().material = thisCell.room.settings.wallMaterial;
			}
		}
	}
}
