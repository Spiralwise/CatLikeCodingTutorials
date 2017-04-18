using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	public int sensivity = 100;

	void Update () {
		if (Input.GetMouseButton(0))
			transform.RotateAround (Vector3.zero, Vector3.up, Input.GetAxis("Mouse X") * sensivity * Time.deltaTime);
		transform.Translate (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel") * sensivity * Time.deltaTime);
	}
}
