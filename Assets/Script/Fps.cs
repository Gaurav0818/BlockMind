using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
	public TMP_Text text;
	string label = "";
	float count;

	IEnumerator Start()
	{
		GUI.depth = 2;
		while (true)
		{
			if (Time.timeScale == 1)
			{
				yield return new WaitForSeconds(0.1f);
				count = (1 / Time.deltaTime);
				label = "FPS :" + (Mathf.Round(count));
				text.text = label.ToString();
			}
			else
			{
				label = "Pause";
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(5, 40, 100, 25), label);
	}
}
