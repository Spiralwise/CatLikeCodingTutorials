using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh mesh;
	MeshCollider collider;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colors;

	public void Awake () {
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		collider = gameObject.AddComponent<MeshCollider> ();
		mesh.name = "Hex Mesh";
		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		colors = new List<Color> ();
	}
		
	public void Triangulate (HexCell[] cells) {
		mesh.Clear ();
		vertices.Clear ();
		triangles.Clear ();
		colors.Clear ();
		for (int c = 0; c < cells.Length; c++)
			Triangulate (cells[c]);
		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.colors = colors.ToArray ();
		mesh.RecalculateNormals ();
		collider.sharedMesh = mesh;
	}

	void Triangulate (HexCell cell) {
		Vector3 position = cell.transform.position;
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
			Triangulate (d, cell);
	}

	void Triangulate (HexDirection direction, HexCell cell) {
		Vector3 center = cell.transform.position;
		Vector3 v1 = center + HexMetrics.GetFirstSolidCorner (direction);
		Vector3 v2 = center + HexMetrics.GetSecondSolidCorner (direction);

		AddTriangle (
			center,
			v1,
			v2
		);
		AddTriangleColor (
			cell.color,
			cell.color,
			cell.color
		);
		if (direction <= HexDirection.SE)
			TriangulateConnection (direction, cell, v1, v2);
	}

	void AddTriangle (Vector3 a, Vector3 b, Vector3 c) {
		int index = vertices.Count;
		vertices.Add (a);
		vertices.Add (b);
		vertices.Add (c);
		triangles.Add (index);
		triangles.Add (index + 1);
		triangles.Add (index + 2);
	}

	void AddTriangleColor (Color c1, Color c2, Color c3) {
		colors.Add (c1);
		colors.Add (c2);
		colors.Add (c3);
	}

	void AddTriangleColor (Color c) {
		for (int i = 0; i < 3; i++)
			colors.Add (c);
	}

	void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		int index = vertices.Count;
		vertices.Add (v1);
		vertices.Add (v2);
		vertices.Add (v3);
		vertices.Add (v4);
		triangles.Add (index);
		triangles.Add (index + 2);
		triangles.Add (index + 1);
		triangles.Add (index + 1);
		triangles.Add (index + 2);
		triangles.Add (index + 3);
	}

	void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		colors.Add (c1);
		colors.Add (c2);
		colors.Add (c3);
		colors.Add (c4);
	}

	void AddQuadColor (Color c1, Color c2) {
		colors.Add (c1);
		colors.Add (c1);
		colors.Add (c2);
		colors.Add (c2);
	}

	void TriangulateConnection (HexDirection direction, HexCell cell, Vector3 v1, Vector3 v2) {
		HexCell neighbor = cell.GetNeighbor (direction);
		if (neighbor == null)
			return;
		Vector3 bridge = HexMetrics.GetBridge (direction);
		Vector3 v3 = v1 + bridge;
		Vector3 v4 = v2 + bridge;
		AddQuad (v1, v2, v3, v4);
		AddQuadColor (cell.color, neighbor.color);

		HexCell nextNeighbor = cell.GetNeighbor (direction.Next ());
		if (direction <= HexDirection.E && nextNeighbor != null) {
			AddTriangle (v2, v4, v2 + HexMetrics.GetBridge(direction.Next()));
			AddTriangleColor (cell.color, neighbor.color, nextNeighbor.color);
		}
	}
}
