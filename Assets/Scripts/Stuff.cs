using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Stuff : MonoBehaviour {

	public Rigidbody Body { get; private set; }

	MeshRenderer[] meshRenderers;

	void Awake() {
		Body = GetComponent<Rigidbody> ();
		meshRenderers = GetComponentsInChildren<MeshRenderer> ();
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag ("Killzone"))
			Destroy (gameObject);
	}

	public void SetMaterial(Material material) {
		for (int i = 0; i < meshRenderers.Length; i++)
			meshRenderers [i].material = material;
	}
}
