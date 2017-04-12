using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRoom : ScriptableObject {

	public int roomID;
	public MazeRoomSettings settings;

	List<MazeCell> cells = new List<MazeCell>();

	public void Add (MazeCell cell) {
		cell.room = this;
		cells.Add (cell);
	}
}
