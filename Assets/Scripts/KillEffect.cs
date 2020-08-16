using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillEffect : MonoBehaviour {
	public Text text;

	void Start() {
		transform.localScale = Vector3.zero;
	}

	public void PopArt(int killCount) {
		if (killCount == 2)
			text.text = "Double Kill";
		else if (killCount == 3)
			text.text = "Triple Kill";
		else if (killCount == 4)
			text.text = "Mega Kill";
		else if (killCount == 5)
			text.text = "PSYCHO";
		else if (killCount == 6)
			text.text = "MANIACO";
		else
			return;

		StopAllCoroutines ();
		StartCoroutine (DisplayArt ());
	}

	IEnumerator DisplayArt() {
		transform.localScale = Vector3.zero;

		for (int i = 0; i < 10; i++) {
			transform.localScale += Vector3.one * 0.06f;
			yield return new WaitForFixedUpdate ();
		}

		yield return new WaitForSecondsRealtime (2);

		for (int i = 0; i < 10; i++) {
			transform.localScale -= Vector3.one * 0.06f;
			yield return new WaitForFixedUpdate ();
		}
	}
}