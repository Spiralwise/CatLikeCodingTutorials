using UnityEngine;

public class MazeWall : MazeCellEdge {

	public Transform wall;

	public override void Initialize(MazeCell thisCell, MazeCell thatCell, MazeDirection direction) {
		base.Initialize (thisCell, thatCell, direction);
		wall.GetComponent<Renderer> ().material = thisCell.room.settings.wallMaterial;
	}
}
