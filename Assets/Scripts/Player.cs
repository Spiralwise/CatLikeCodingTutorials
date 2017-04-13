using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	MazeCell currentCell;
	MazeDirection currentDirection;

	public void SetLocation (MazeCell cell) {
		if (currentCell != null)
			currentCell.OnPlayerExited ();
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
		currentCell.OnPlayerEntered ();
		Debug.Log ("My cell is " + currentCell.coordinates.x + ", " + currentCell.coordinates.y);
	}

	void Move (MazeDirection direction) {
		MazeCellEdge edge = currentCell.GetEdge (direction);
		if (edge is MazePassage) {
			SetLocation (edge.neighborCell);
		}
	}

	void Rotate (MazeDirection direction) {
		transform.localRotation = direction.ToRotation ();
		currentDirection = direction;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.UpArrow)) // NOTE Should use axes keys
			Move (currentDirection);
		else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow))
			Move (currentDirection.GetOposite());
		else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow))
			Move (currentDirection.GetNextClockwise());
		else if (Input.GetKeyDown (KeyCode.Q) || Input.GetKeyDown (KeyCode.LeftArrow))
			Move (currentDirection.GetNextCounterclockwise());
		else if (Input.GetKeyDown (KeyCode.A))
			Rotate (currentDirection.GetNextCounterclockwise ());
		else if (Input.GetKeyDown (KeyCode.E))
			Rotate (currentDirection.GetNextClockwise ());
	}
}
