using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {

	public Maze mazePrefab;
	public Player playerPrefab;
	public float generationRateInSec;

	Maze mazeInstance;
	Player playerInstance;

	void Start () {
		StartCoroutine (BeginGame ());
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) 
			RestartGame ();
	}

	IEnumerator BeginGame() {
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		mazeInstance = Instantiate (mazePrefab) as Maze;
		mazeInstance.transform.SetParent (transform);
		yield return StartCoroutine(mazeInstance.Generate (generationRateInSec));
		playerInstance = Instantiate (playerPrefab) as Player;
		playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect (0f, 0f, 0.5f, 0.5f);
	}

	void RestartGame() {
		StopAllCoroutines ();
		if (mazeInstance != null)
			Destroy (mazeInstance.gameObject);
		if (playerInstance != null)
			Destroy (playerInstance.gameObject);
		StartCoroutine (BeginGame ());
	}
}
