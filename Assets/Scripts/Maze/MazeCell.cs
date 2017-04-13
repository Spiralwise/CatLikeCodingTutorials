using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;
	public MazeRoom room;

	int initializedEdges;

	MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	public MazeCell() {
		initializedEdges = 0;
	}

	public void Initialize(MazeRoom room) {
		room.Add (this);
		transform.GetChild (0).GetComponent<Renderer> ().material = room.settings.floorMaterial;
	}

	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges [(int)direction];
	}

	public void SetEdge(MazeDirection direction, MazeCellEdge edge) {
		if (edges [(int)direction] == null)
			initializedEdges++;
		edges [(int)direction] = edge;
	}

	public bool IsFullyInitialized {
		get {
			return initializedEdges == MazeDirections.Count;
		}
	}

	public MazeDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range (0, MazeDirections.Count - initializedEdges);
			for (int i = 0; i < MazeDirections.Count; i++) {
				if (edges[i] == null) {
					if (skips > 0)
						skips--;
					else
						return (MazeDirection)i;
				}
			}
			throw new System.InvalidOperationException ("MazeCell has no uninitalized direction left.");
		}
	}

	public void OnPlayerEntered() {
		for (int i = 0; i < edges.Length; i++)
			edges[i].OnPlayerEntered ();
	}

	public void OnPlayerExited() {
		for (int i = 0; i < edges.Length; i++)
			edges[i].OnPlayerExited ();
	}
}
