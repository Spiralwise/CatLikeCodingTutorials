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
		for (int v = 0; v < HexMetrics.corners.Length; v++) {
			AddTriangle (
				position, 
				position + HexMetrics.corners [v],
				position + HexMetrics.corners [(v + 1) % 6]
			);
			AddTriangleColor (cell.color);
		}
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

	void AddTriangleColor (Color color) {
		for (int i = 0; i < 3; i++)
			colors.Add (color);
	}
}
