using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeGenerator : MonoBehaviour {

	public int xSize, ySize, zSize;

	Vector3[] vertices;
	Mesh mesh;

	static int SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles [i] = v00;
		triangles [i + 1] = triangles [i + 4] = v01;
		triangles [i + 2] = triangles [i + 3] = v10;
		triangles [i + 5] = v11;
		return i + 6;
	}

	void Awake () {
		Generate ();
	}

	void Generate () {
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural cube";

		CreateVertices ();
		CreateTriangles ();

//		Vector2[] UV = new Vector2[vertices.Length];
//		Vector4[] tangents = new Vector4[vertices.Length];
//		Vector4 tangent = new Vector4 (1f, 0f, 0f, -1f);

//		mesh.uv = UV;
//		mesh.tangents = tangents;
	}

	void CreateVertices () {
		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;
		int faceVertices = (((xSize - 1) * (ySize - 1)) + ((xSize - 1) * (zSize - 1)) + ((zSize - 1) * (ySize - 1))) * 2;
		vertices = new Vector3 [cornerVertices + edgeVertices + faceVertices];

		int v = 0;
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++)
				vertices [v++] = new Vector3 (x, y, 0);
			for (int z = 1; z <= zSize; z++)
				vertices [v++] = new Vector3 (xSize, y, z);
			for (int x = xSize - 1; x >= 0; x--)
				vertices [v++] = new Vector3 (x, y, zSize);
			for (int z = zSize - 1; z > 0; z--)
				vertices [v++] = new Vector3 (0, y, z);
		}
		for (int z = 1; z < zSize; z++)
			for (int x = 1; x < xSize; x++) {
				vertices [v++] = new Vector3 (x, ySize, z);
				vertices [v+(xSize-1)*(zSize-1)-1] = new Vector3 (x, 0, z);
			}
		mesh.vertices = vertices;
	}

	void CreateTriangles () {
		/* Note: Can do better with dual indices : one fort vertices, one for triangles. */
		int quads = ((xSize * ySize) + (ySize * zSize) + (xSize * zSize)) * 2;
		int[] triangles = new int[quads * 6];

		int i = 0;
		int x;
		int ringLength = (xSize + zSize) * 2;
		for (int y = 0; y < ySize; y++) {
			for (x = ringLength * y; x < ringLength * (y + 1) - 1; x++)
				i = SetQuad (triangles, i, x, x + 1, ringLength + x, ringLength + x + 1);
			i = SetQuad (triangles, i, x, x + 1 - ringLength, ringLength + x, x + 1);
		}
		i = CreateTopFace (triangles, i, ringLength);
		i = CreateBottomFace (triangles, i, ringLength);

		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
	}

	int CreateBottomFace (int[] triangles, int i, int ringLength) {
		int vmid = ringLength * (ySize + 1) + (xSize - 1) * (zSize - 1);
		i = SetQuad (triangles, i, ringLength - 1, vmid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, vmid++)
			i = SetQuad (triangles, i, vmid, vmid + 1, x, x + 1);
		i = SetQuad (triangles, i, vmid, xSize + 1, xSize - 1, xSize);

		vmid = ringLength * (ySize + 1) + (xSize - 1) * (zSize - 1);
		int vmidOtherSide = vmid + xSize - 1;
		int vmax = ringLength - 1;
		int vmin = xSize + 1;
		for (int z = 1; z < zSize - 1; z++, vmax--, vmin++, vmid++, vmidOtherSide++) {
			i = SetQuad (triangles, i, vmax - 1, vmidOtherSide, vmax, vmid);
			for (int x = 1; x < xSize - 1; x++, vmid++, vmidOtherSide++)
				i = SetQuad (triangles, i, vmidOtherSide, vmidOtherSide + 1, vmid, vmid + 1);
			i = SetQuad (triangles, i, vmidOtherSide, vmin + 1, vmid , vmin);
		}
			
		vmidOtherSide = vmax - 2;
		i = SetQuad (triangles, i, vmax - 1, vmidOtherSide, vmax, vmid);
		for (int x = 1; x < xSize - 1; x++, vmid++, vmidOtherSide--)
			i = SetQuad (triangles, i, vmidOtherSide, vmidOtherSide - 1, vmid, vmid + 1);
		i = SetQuad (triangles, i, vmidOtherSide, vmidOtherSide - 1, vmid, vmidOtherSide - 2);

		return i;
	}

	int CreateTopFace (int[] triangles, int i, int ringLength) {
		int x;
		for (x = ringLength * ySize; x < ringLength * ySize + xSize - 1; x++)
			i = SetQuad (triangles, i, x, x + 1, x + ringLength - 1, x + ringLength);
		i = SetQuad (triangles, i, x, x + 1, x + ringLength - 1, x + 2);

		int vmin = ringLength * (ySize + 1) - 1;
		int vmid = vmin + 1;
		int vmax = ringLength * ySize + xSize + 1;
		for (int z = 1; z < zSize - 1; z++, vmin--, vmid++, vmax++) {
			i = SetQuad (triangles, i, vmin, vmid, vmin - 1, vmid + xSize - 1);
			for (x = 1; x < xSize - 1; x++, vmid++)
				i = SetQuad (triangles, i, vmid, vmid + 1, vmid + xSize - 1, vmid + xSize);
			i = SetQuad (triangles, i, vmid, vmax, vmid + xSize - 1, vmax + 1);
		}

		i = SetQuad (triangles, i, vmin, vmid, vmin - 1, vmin - 2);
		vmin -= 2;
		for (x = 1; x < xSize - 1; x++, vmin--, vmid++)
			i = SetQuad (triangles, i, vmid, vmid + 1, vmin, vmin - 1);
		i = SetQuad (triangles, i, vmid, vmax, vmin, vmax + 1);

		return i;
	}

	void OnDrawGizmos () {
		if (vertices != null) {
			Gizmos.color = Color.black;
			for (int i = 0; i < vertices.Length; i++)
				Gizmos.DrawSphere (vertices [i], 0.1f);
		}
	}
}
