using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour {

	public int xSize, ySize;

	Vector3[] vertices;
	Mesh mesh;

	void Awake () {
		StartCoroutine (Generate ());
	}

	IEnumerator Generate () {
		WaitForSeconds wiat = new WaitForSeconds (0.05f);
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural grid";
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] UV = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4 (1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= ySize; y++)
			for (int x = 0; x <= xSize; x++, i++) {
				vertices [i] = new Vector3 (x, y);
				UV [i] = new Vector2 ((float)x/xSize*2, (float)y/ySize);
				tangents [i] = tangent;
			}
		mesh.vertices = vertices;
		mesh.uv = UV;
		mesh.tangents = tangents;

		/* Note: Can do better with dual indices : one fort vertices, one for triangles. */
		int[] triangles = new int[(xSize+1)*(ySize+1)*2*3];
		for (int i = 0, y = 0; y < ySize; y++)
			for (int x = 0; x < xSize; x++, i+=6) {
				triangles [i] = x + y * (xSize + 1);
				triangles [i + 1] = triangles [i + 4] = x + (y + 1) * (xSize + 1);
				triangles [i + 2] = triangles [i + 3] = x + y * (xSize + 1) + 1;
				triangles [i + 5] = x + (y + 1) * (xSize + 1) + 1;
				mesh.triangles = triangles;
				yield return wiat;
			}
		mesh.RecalculateNormals ();
	}

	void OnDrawGizmos () {
//		if (vertices != null) {
//			Gizmos.color = Color.black;
//			for (int i = 0; i < vertices.Length; i++)
//				Gizmos.DrawSphere (vertices [i], 0.1f);
//		}
	}
}
