using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {

	public Maze mazePrefab;
	public float generationRateInSec;

	Maze mazeInstance;

	void Start () {
		BeginGame ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) 
			RestartGame ();
	}

	void BeginGame() {
		mazeInstance = Instantiate (mazePrefab) as Maze;
		mazeInstance.transform.SetParent (transform);
		StartCoroutine(mazeInstance.Generate (generationRateInSec));
	}

	void RestartGame() {
		StopAllCoroutines ();
		if (mazeInstance != null)
			Destroy (mazeInstance.gameObject);
		BeginGame ();
	}
}
