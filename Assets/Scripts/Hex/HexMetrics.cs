using UnityEngine;

public static class HexMetrics {

	public const float outterRadius = 10.0f;
	public const float innerRadius = outterRadius * 0.866025404f;

	public static Vector3[] corners = {
		new Vector3 (0f, 0f, outterRadius),
		new Vector3 (innerRadius, 0f, 0.5f * outterRadius),
		new Vector3 (innerRadius, 0f, -0.5f * outterRadius),
		new Vector3 (0f, 0f, -outterRadius),
		new Vector3 (-innerRadius, 0f, -0.5f * outterRadius),
		new Vector3 (-innerRadius, 0f, 0.5f * outterRadius)
	};
}

[System.Serializable]
public struct HexCoordinates {

	[SerializeField]
	private int x, y;
	
	public int X { get { return x; } }

	public int Y { get { return y; } }

	public int Z { get { return -X - Y; } }

	public HexCoordinates (int x, int y) {
		this.x = x;
		this.y = y;
	}

	public static HexCoordinates FromOffsetCoordinates (int x, int y) {
		return new HexCoordinates (x - y/2, y);
	}

	public static HexCoordinates FromPosition (Vector3 v) {
		float foundX = v.x / (HexMetrics.innerRadius * 2f);
		float foundZ = -foundX;
		float offset = v.z / (HexMetrics.outterRadius * 3f);
		foundX -= offset;
		foundZ -= offset;
		int iX = Mathf.RoundToInt (foundX);
		int iZ = Mathf.RoundToInt (foundZ);
		int iY = Mathf.RoundToInt (-foundX - foundZ);
		if (iX + iY + iZ != 0)
			Debug.LogWarning ("Inconsistent cubic coordinates");
		return new HexCoordinates (iX, iY);
	}

	public string toString () {
		return "(" + X.ToString () + ", " + Z.ToString () + ", " + Y.ToString () + ")";
	}

	public string ToStringOnSeparateLines () {
		return X.ToString () + "\n" + Z.ToString () + "\n" + Y.ToString ();
	}
}