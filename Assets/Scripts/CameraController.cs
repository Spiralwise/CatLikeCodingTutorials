using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	public int sensivity = 100;
	public GameObject cameraTarget;

	Vector3 target;

	void Start () {
		if (cameraTarget != null)
			target = cameraTarget.transform.position;
		else
			target = Vector3.zero;
		transform.LookAt (target);
	}

	void Update () {
		if (Input.GetMouseButton(0))
			transform.RotateAround (target, Vector3.up, Input.GetAxis("Mouse X") * sensivity * Time.deltaTime);
		transform.Translate (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel") * sensivity * Time.deltaTime);
	}
}
