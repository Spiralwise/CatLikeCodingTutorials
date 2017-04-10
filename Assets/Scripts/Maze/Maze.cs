using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public IntVector2 size;
	public MazeCell cellPrefab;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

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
		MazeDirection direction = MazeDirections.RandomDirection;
		IntVector2 coordinates = activeCell.coordinates + direction.ToIntVector2 ();
		Debug.Log ("Look at " + coordinates.x + ", " + coordinates.y);
		if (ContainsCoordinates (coordinates)) {
			MazeCell neighbor = GetCell (coordinates);
			if (neighbor == null) {
				neighbor = CreateCell (coordinates);
				CreatePassage (activeCell, neighbor, direction);
				activeCells.Add (neighbor);
				Debug.Log ("NO NEIGHTBOR");
			} else {
				CreateWall (activeCell, neighbor, direction);
				activeCells.RemoveAt (activeCells.Count - 1);
				Debug.Log ("NEIGHBOR");
			}
		} else {
			Debug.Log ("OUT OF RANGE");
			CreateWall (activeCell, null, direction);
			activeCells.RemoveAt (activeCells.Count - 1);
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
		MazePassage passage = Instantiate (passagePrefab) as MazePassage;
		passage.Initialize (thisCell, thatCell, direction);
		passage = Instantiate (passagePrefab) as MazePassage;
		passage.Initialize (thatCell, thisCell, direction.GetOposite());
	}

	void CreateWall(MazeCell thisCell, MazeCell thatCell, MazeDirection direction) {
		MazeWall wall = Instantiate (wallPrefab) as MazeWall;
		wall.Initialize (thisCell, thatCell, direction);
		if (thatCell != null) {
			wall = Instantiate (wallPrefab) as MazeWall;
			wall.Initialize (thatCell, thisCell, direction);
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
