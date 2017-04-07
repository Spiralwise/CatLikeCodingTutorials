using UnityEngine;
using System;

public class ClockAnimator : MonoBehaviour {

	private const float
		hoursToDegrees = 360f / 12f,
		minutesToDegres = 360f / 60f,
		secondsToDegres = 360f / 60f;

	public Transform hours, minutes, seconds;
	public bool analog;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (analog) {
			TimeSpan timespan = DateTime.Now.TimeOfDay;
			hours.localRotation = Quaternion.Euler (0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
			minutes.localRotation = Quaternion.Euler (0f, 0f, (float)timespan.TotalMinutes * -minutesToDegres);
			seconds.localRotation = Quaternion.Euler (0f, 0f, (float)timespan.TotalSeconds * -secondsToDegres);
		} else {
			DateTime time = DateTime.Now;
			hours.localRotation = Quaternion.Euler (0f, 0f, time.Hour * -hoursToDegrees);
			minutes.localRotation = Quaternion.Euler (0f, 0f, time.Minute * -minutesToDegres);
			seconds.localRotation = Quaternion.Euler (0f, 0f, time.Second * -secondsToDegres);
		}
	}
}
