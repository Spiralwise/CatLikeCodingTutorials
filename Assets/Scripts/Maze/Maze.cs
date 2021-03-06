﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public IntVector2 size;
	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazePassage doorPrefab;
	[Range(0f, 1.0f)]
	public float doorProbabilty;
	public MazeWall[] wallPrefabs;
	public MazeRoomSettings[] roomSettings;

	MazeCell[,] cells;
	List<MazeRoom> rooms = new List<MazeRoom>();

	public MazeCell GetCell(IntVector2 coordinates) {
		return cells [coordinates.x, coordinates.y];
	}

	public IEnumerator Generate(float generationRate) {
		WaitForSeconds delay = new WaitForSeconds (generationRate);
		cells = new MazeCell[size.x, size.y];
		List<MazeCell> activeCells = new List<MazeCell>();
		MazeCell firstCell = CreateCell (RandomCoordinates);
		firstCell.Initialize (CreateRoom (-1));
		activeCells.Add (firstCell);
		while (activeCells.Count > 0) {
			yield return delay;
			ContinuousGeneration (activeCells);
		}
		//for (int i = 0; i < rooms.Count; i++)
		//	rooms [i].Hide ();
	}

	void ContinuousGeneration(List<MazeCell> activeCells) {
		MazeCell activeCell = activeCells [activeCells.Count - 1];
		if (activeCell.IsFullyInitialized) {
			activeCells.RemoveAt (activeCells.Count - 1);
		} else {
			MazeDirection direction = activeCell.RandomUninitializedDirection;
			IntVector2 coordinates = activeCell.coordinates + direction.ToIntVector2 ();
			if (ContainsCoordinates (coordinates)) {
				MazeCell neighbor = GetCell (coordinates);
				if (neighbor == null) {
					neighbor = CreateCell (coordinates);
					CreatePassage (activeCell, neighbor, direction);
					activeCells.Add (neighbor);
				} else if (activeCell.room.roomID == neighbor.room.roomID) {
					CreatePassageInSameRoom (activeCell, neighbor, direction);
				} else {
					CreateWall (activeCell, neighbor, direction);
				}
			} else {
				CreateWall (activeCell, null, direction);
			}
		}
	}

	MazeRoom CreateRoom(int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom> ();
		newRoom.roomID = Random.Range (0, roomSettings.Length);
		if (newRoom.roomID == indexToExclude)
			newRoom.roomID = (newRoom.roomID + 1) % roomSettings.Length;
		newRoom.settings = roomSettings [newRoom.roomID];
		rooms.Add (newRoom);
		return newRoom;
	}

	MazeCell CreateCell(IntVector2 coordinates) {
		MazeCell bufferCell = Instantiate (cellPrefab) as MazeCell;
		cells [coordinates.x, coordinates.y] = bufferCell;
		bufferCell.name = "Cell " + coordinates.x + "," + coordinates.y;
		bufferCell.coordinates = coordinates;
		bufferCell.transform.SetParent (transform);
		bufferCell.transform.localPosition = new Vector3 (coordinates.x - size.x*0.5f + 0.5f, 0f, coordinates.y - size.y*0.5f + 0.5f);
		return bufferCell;
	}

	void CreatePassage(MazeCell thisCell, MazeCell thatCell, MazeDirection direction) {
		MazePassage prefab = Random.value < doorProbabilty ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate (prefab) as MazePassage;
		passage.Initialize (thisCell, thatCell, direction);
		passage = Instantiate (prefab) as MazePassage;
		if (passage is MazeDoor)
			thatCell.Initialize (CreateRoom (thisCell.room.roomID));
		else
			thatCell.Initialize (thisCell.room);
		passage.Initialize (thatCell, thisCell, direction.GetOposite());
	}

	void CreatePassageInSameRoom(MazeCell thisCell, MazeCell thatCell, MazeDirection direction) {
		MazePassage passage = Instantiate (passagePrefab) as MazePassage;
		passage.Initialize (thisCell, thatCell, direction);
		passage = Instantiate (passagePrefab) as MazePassage;
		thatCell.Initialize (thisCell.room);
	}

	void CreateWall(MazeCell thisCell, MazeCell thatCell, MazeDirection direction) {
		MazeWall wall = Instantiate (wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize (thisCell, thatCell, direction);
		if (thatCell != null) {
			wall = Instantiate (wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
			wall.Initialize (thatCell, thisCell, direction.GetOposite());
		}
	}

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2 (Random.Range (0, size.x), Random.Range (0, size.y));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
	}
}
