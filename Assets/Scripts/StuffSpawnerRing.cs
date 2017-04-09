using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffSpawnerRing : MonoBehaviour {

	public int numberOfSpawners;
	public float radius, tiltAngle;
	public StuffSpawner prefabSpawner;

	void Awake() {
		for (int i = 0; i < numberOfSpawners; i++)
			CreateSpawner (i);
	}

	void CreateSpawner(int idx) {
		Transform rotater = new GameObject ("Rotater").transform;
		rotater.transform.SetParent (transform, false);
		rotater.localRotation = Quaternion.Euler (0f, idx * 360f / numberOfSpawners, 0f);
		StuffSpawner spawner = Instantiate<StuffSpawner> (prefabSpawner);
		spawner.transform.SetParent (rotater, false);
		spawner.transform.localPosition = new Vector3 (0f, 0f, radius);
		spawner.transform.localRotation = Quaternion.Euler (tiltAngle, 0f, 0f);
	}
}
