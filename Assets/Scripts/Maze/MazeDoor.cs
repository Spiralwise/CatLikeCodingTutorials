using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage {

	public Transform hinge;

	static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);
	bool isMirrored;

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
			isMirrored = true;
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

	public override void OnPlayerEntered() {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
		//OtherSideOfDoor.homeCell.room.Show ();
	}

	public override void OnPlayerExited() {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
		//OtherSideOfDoor.homeCell.room.Hide ();
	}
}
