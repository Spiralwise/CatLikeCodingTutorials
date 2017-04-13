using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {

	public MazeCell homeCell, neighborCell;
	public MazeDirection direction;

	public virtual void Initialize (MazeCell cell, MazeCell thatCell, MazeDirection direction) {
		this.homeCell = cell;
		this.neighborCell = thatCell;
		this.direction = direction;
		cell.SetEdge (direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation ();
	}

	public virtual void OnPlayerEntered() {}

	public virtual void OnPlayerExited() {}
}
