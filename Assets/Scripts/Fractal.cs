using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

	public Mesh[] meshes;
	public Material material;
	public float maxRotationSpeed;
	public float maxTwist;
	public int maxDepth;
	public float spawnProbability;
	public float minGrowthDelay = 0.1f;
	public float maxGrowthDelay = 0.5f;
	public float childScale;

	private float rotationSpeed;
	private int depth;
	private Material[,] materials;

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f)
	};

	void Start () {
		if (materials == null) {
			InitializeMaterials ();
			Debug.LogWarning ("Materials null");
		}
		rotationSpeed = Random.Range (-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate (Random.Range (-maxTwist, maxTwist), 0f, 0f);
		gameObject.AddComponent<MeshFilter> ().mesh = meshes[Random.Range(0, meshes.Length)];
		gameObject.AddComponent<MeshRenderer> ().material = material;
		GetComponent<MeshRenderer> ().material = materials[depth, Random.Range(0, 2)];
		if (depth < maxDepth) {
			StartCoroutine (CreateChildren());
		}
	}

	IEnumerator CreateChildren() {
		for (int i = 0; i < childDirections.Length; i++) {
			if (Random.value < spawnProbability) {
				yield return new WaitForSeconds (Random.Range (minGrowthDelay, maxGrowthDelay));
				new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize (this, i);
			}
		}
	}

	void Initialize (Fractal parent, int idx) {
		meshes = parent.meshes;
		material = parent.material;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;
		maxDepth = parent.maxDepth;
		spawnProbability = parent.spawnProbability;
		childScale = parent.childScale;
		minGrowthDelay = parent.minGrowthDelay;
		maxGrowthDelay = parent.maxGrowthDelay;
		depth = parent.depth + 1;
		materials = parent.materials;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[idx] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[idx];
	}

	void InitializeMaterials () {
		materials = new Material[maxDepth + 1, 2];
		for (int i = 0; i <= maxDepth-1; i++) {
			float t = (float)i / (maxDepth - 1);
			t *= t;
			materials[i, 0] = new Material (material);
			materials [i, 0].color = Color.Lerp (Color.white, Color.yellow, t);
			materials [i, 1] = new Material (material);
			materials [i, 1].color = Color.Lerp (Color.white, Color.cyan, t);
		}
		materials [maxDepth, 0] = new Material (material);
		materials [maxDepth, 0].color = Color.magenta;
		materials [maxDepth, 1] = new Material (material);
		materials [maxDepth, 1].color = Color.red;
	}

	void Update() {
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
