using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundedCubeGenerator : CubeGenerator {

	public int roundness;

	Vector3[] normals;

	override protected void Generate () {
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural rounded cube";

		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;
		int faceVertices = (((xSize - 1) * (ySize - 1)) + ((xSize - 1) * (zSize - 1)) + ((zSize - 1) * (ySize - 1))) * 2;
		normals = new Vector3 [cornerVertices + edgeVertices + faceVertices];

		CreateVertices ();
		base.CreateTriangles ();
		CreateColliders ();
	}

	override protected void SetVertex (int idx, int x, int y, int z) {
		base.SetVertex (idx, x, y, z);

		Vector3 inner = vertices [idx];
		if (x < roundness)
			inner.x = roundness;
		else if (x > xSize - roundness)
			inner.x = xSize - roundness;
		if (y < roundness)
			inner.y = roundness;
		else if (y > ySize - roundness)
			inner.y = ySize - roundness;
		if (z < roundness)
			inner.z = roundness;
		else if (z > zSize - roundness)
			inner.z = zSize - roundness;
		
		normals [idx] = (vertices [idx] - inner).normalized;
		vertices [idx] = inner + normals [idx] * roundness;
	}

	override protected void CreateVertices () {
		base.CreateVertices ();

		mesh.normals = normals;
	}

	void CreateColliders () {
		AddBoxCollider (xSize, ySize - roundness * 2, xSize - roundness * 2);
		AddBoxCollider (xSize - roundness * 2, ySize, xSize - roundness * 2);
		AddBoxCollider (xSize - roundness * 2, ySize - roundness * 2, xSize);

		Vector3 min = Vector3.one * roundness;
		Vector3 half = new Vector3 (xSize, ySize, zSize) * 0.5f;
		Vector3 max = new Vector3 (xSize, ySize, zSize) - min;

		AddCapsuleCollider (0, half.x, min.y, min.z);
		AddCapsuleCollider (0, half.x, min.y, max.z);
		AddCapsuleCollider (0, half.x, max.y, min.z);
		AddCapsuleCollider (0, half.x, max.y, max.z);

		AddCapsuleCollider (1, min.x, half.y, min.z);
		AddCapsuleCollider (1, min.x, half.y, max.z);
		AddCapsuleCollider (1, max.x, half.y, min.z);
		AddCapsuleCollider (1, max.x, half.y, max.z);

		AddCapsuleCollider (2, min.x, min.y, half.z);
		AddCapsuleCollider (2, min.x, max.y, half.z);
		AddCapsuleCollider (2, max.x, min.y, half.z);
		AddCapsuleCollider (2, max.x, max.y, half.z);
	}

	void AddBoxCollider (float x, float y, float z) {
		BoxCollider c = gameObject.AddComponent<BoxCollider> ();
		c.size = new Vector3 (x, y, z);
	}

	void AddCapsuleCollider (int direction, float x, float y, float z) {
		CapsuleCollider c = gameObject.AddComponent<CapsuleCollider> ();
		c.center = new Vector3 (x, y, z);
		c.direction = direction;
		c.radius = roundness;
		c.height = c.center [direction] * 2f;
	}

	void OnDrawGizmos () {
		/*if (vertices != null) {
			Gizmos.color = Color.black;
			for (int i = 0; i < vertices.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawSphere (vertices [i], 0.1f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay (vertices [i], normals [i]);
			}
		}*/
	}
}
