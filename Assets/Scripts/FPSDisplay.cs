using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour {

	[System.Serializable]
	private struct FPSColor {
		public Color color;
		public int minimumFPS;
	}

	public Text averageFPSLabel, highestFPSLabel, lowestFPSLabel;

	[SerializeField]
	private FPSColor[] coloring;

	FPSCounter fpsCounter;

	void Awake() {
		fpsCounter = GetComponent<FPSCounter> ();
	}

	void Update() {
		Display (averageFPSLabel, fpsCounter.AverageFPS);
		Display (highestFPSLabel, fpsCounter.HighestFPS);
		Display (lowestFPSLabel, fpsCounter.LowestFPS);
	}

	void Display(Text label, int fps) {
		label.text = Mathf.Clamp (fps, 0, 99).ToString ();
		for (int i = 0; i < coloring.Length; i++) {
			if (fps >= coloring[i].minimumFPS) {
				if (i == 0) {
					label.color = coloring [0].color;
				} else {
					float t = (float)(coloring [i - 1].minimumFPS - fps) / (coloring [i - 1].minimumFPS - coloring [i].minimumFPS);
					label.color = Color.Lerp (coloring [i - 1].color, coloring [i].color, t);
				} 
				break;
			}
		}
	}
}
