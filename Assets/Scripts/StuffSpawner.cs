using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffSpawner : MonoBehaviour {

	public FloatRange spawnTime, scaleBound, randomVelocity, randomAngularVelocity;
	public Stuff[] stuffPrefabs;
	public Material stuffMaterial;
	public float velocity;

	float timer;
	float currentSpawnTime;

	void Start() {
		currentSpawnTime = spawnTime.RandomInRange;
		timer = 0f;
	}

	void FixedUpdate() {
		timer += Time.deltaTime;
		if (timer >= currentSpawnTime) {
			timer -= currentSpawnTime;
			currentSpawnTime = spawnTime.RandomInRange;
			SpawnStuff ();
		}
	}

	void SpawnStuff() {
		Stuff prefab = stuffPrefabs [Random.Range (0, stuffPrefabs.Length)];
		Stuff stuff = Instantiate<Stuff> (prefab, transform.position, Random.rotation);
		stuff.transform.localScale = Vector3.one * scaleBound.RandomInRange;
		stuff.Body.velocity = velocity * transform.up + Random.onUnitSphere * randomVelocity.RandomInRange;
		stuff.Body.angularVelocity = Random.onUnitSphere * randomAngularVelocity.RandomInRange;
		stuff.SetMaterial (stuffMaterial);
	}
}
