using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    public GameObject[] HeadParts;
    public GameObject[] UpperBodyParts;
    public GameObject[] LegParts;
    public GameObject[] BagParts;

    public int CurrentHead;
    public int CurrentBody;
    public int CurrentLeg;
    public int CurrentBag;

    // Start is called before the first frame update
    void Start()
    {
        ChangePart(HeadParts, CurrentHead);
        ChangePart(UpperBodyParts, CurrentBody);
        ChangePart(LegParts, CurrentLeg);
        ChangePart(BagParts, CurrentBag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangePart( GameObject[] part, int num)
    {
        for( int i=0; i < part.Length; i++)
        {
            if(i == num)
                part[i].SetActive(true);
            else
                part[i].SetActive(false);
        }
    }
}
