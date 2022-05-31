using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerCount : MonoBehaviour
{
    public TMP_Text text;
    public float count = 0;
    public GameObject CountCanvas;
    // Start is called before the first frame update
    void Start()
    {
        count = 0; 
        DisplayCount();
    }

    public void IncreaseCount()
    {
        count = count + 1;
        CountCanvas.SetActive(false); 
        DisplayCount();
    }

    void DisplayCount()
    {
        text.text = count.ToString() + " / 5";
    }
}
