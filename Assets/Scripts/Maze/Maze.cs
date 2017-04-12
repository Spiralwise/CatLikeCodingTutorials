using System.Collections;
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

	MazeCell[,] cells;

	public MazeCell GetCell(IntVector2 coordinates) {
		return cells [coordinates.x, coordinates.y];
	}

	public IEnumerator Generate(float generationRate) {
		WaitForSeconds delay = new WaitForSeconds (generationRate);
		cells = new MazeCell[size.x, size.y];
		List<MazeCell> activeCells = new List<MazeCell>();
		activeCells.Add (CreateCell (RandomCoordinates));
		while (activeCells.Count > 0) {
			yield return delay;
			ContinuousGeneration (activeCells);
		}
	}

	void ContinuousGeneration(List<MazeCell> activeCells) {
		MazeCell activeCell = activeCells [activeCells.Count - 1];
		if (activeCell.IsFullyInitialized) {
			activeCells.RemoveAt (activeCells.Count - 1);
		} else {
			MazeDirection direction = activeCell.RandomUninitializedDirection;
			IntVector2 coordinates = activeCell.coordinates + direction.ToIntVector2 ();
			Debug.Log ("From: " + activeCell.coordinates.x + ", " + activeCell.coordinates.y + " To: " + coordinates.x + ", " + coordinates.y);
			if (ContainsCoordinates (coordinates)) {
				MazeCell neighbor = GetCell (coordinates);
				if (neighbor == null) {
					neighbor = CreateCell (coordinates);
					CreatePassage (activeCell, neighbor, direction);
					activeCells.Add (neighbor);
				} else {
					CreateWall (activeCell, neighbor, direction);
				}
			} else {
				CreateWall (activeCell, null, direction);
			}
		}
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
		passage.Initialize (thatCell, thisCell, direction.GetOposite());
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
