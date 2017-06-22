using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;
	public HexGrid grid;

	Color activeColor;

	void Awake () {
		SelectColor (0);
	}

	void Update () {
		if (Input.GetMouseButton (1) && !EventSystem.current.IsPointerOverGameObject ())
			HandleInput ();
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (inputRay, out hit)) {
			//Debug.Log ("This is hit!");
			grid.ColorCell (hit.point, activeColor);
		}
	}

	public void SelectColor (int index) {
		activeColor = colors [index];
	}
}
