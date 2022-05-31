using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    public GameObject[] HeadParts;
    public GameObject[] UpperBodyParts;
    public GameObject[] LegParts;
    public GameObject[] BagParts;


    public BodyParts player;

    public int CurrentHead;
    public int CurrentBody;
    public int CurrentLeg;
    public int CurrentBag;    
    
    public int newHead;
    public int newBody;
    public int newLeg;
    public int newBag;

    public bool canChange;



    public GameObject frontView;
    public GameObject backView;



    public int partSelected = 0;    // 0 for head , 1 for Upper BodyParts , 2 for Leg Parts , 3 for Bag Parts

    private void OnEnable()
    {
        if(player != null)
        {
            newHead = player.CurrentHead;
            newBody = player.CurrentBody;
            newLeg = player.CurrentLeg;
            newBag = player.CurrentBag;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangePart(HeadParts, CurrentHead);
        ChangePart(UpperBodyParts, CurrentBody);
        ChangePart(LegParts, CurrentLeg);
        ChangePart(BagParts, CurrentBag);

        newHead = CurrentHead;
        newBody = CurrentBody;
        newLeg = CurrentLeg;
        newBag = CurrentBag;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canChange)
        {
            canChange = false;
            ChangePart(HeadParts, CurrentHead);
            ChangePart(UpperBodyParts, CurrentBody);
            ChangePart(LegParts, CurrentLeg);
            ChangePart(BagParts, CurrentBag);
        }
    }

    public void SelectBodyPart(int num)
    {
        partSelected = num;
    }

    int IncreaseCount(GameObject[] part, int num)
    {
        if (num == part.Length-1)
            num = 0;
        else
            num += 1;

        for (int i = 0; i < part.Length; i++)
        {
            if (i == num)
                part[i].SetActive(true);
            else
                part[i].SetActive(false);
        }
        return num;

    }
    int DecreaseCount(GameObject[] part, int num)
    {
        if (num == 0)
            num = part.Length-1;
        else
            num -= 1;

        for (int i = 0; i < part.Length; i++)
        {
            if (i == num)
                part[i].SetActive(true);
            else
                part[i].SetActive(false);
        }

        return num;
    }

    public void IncreasePartNo( )
    {
        if(partSelected == 0)
        {
            newHead = IncreaseCount(HeadParts, newHead);
        }
        else if (partSelected == 1)
        {
            newBody = IncreaseCount(UpperBodyParts, newBody);
        }
        else if (partSelected == 2)
        {
            newLeg = IncreaseCount(LegParts, newLeg);
        }
        else if (partSelected == 3)
        {
            newBag = IncreaseCount(BagParts, newBag);
        }
    }





    public void DecreasePartNo( )
    {
        if (partSelected == 0)
        {
            newHead = DecreaseCount(HeadParts, newHead);
        }
        else if (partSelected == 1)
        {
            newBody = DecreaseCount(UpperBodyParts, newBody);
        }
        else if (partSelected == 2)
        {
            newLeg = DecreaseCount(LegParts, newLeg);
        }
        else if (partSelected == 3)
        {
            newBag = DecreaseCount(BagParts, newBag);
        }
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

    public void Saved()
    {
        player.CurrentHead = newHead;
        player.CurrentBody = newBody;
        player.CurrentLeg = newLeg;
        player.CurrentBag = newBag;

        player.canChange = true;

    }

    public void ChangeView()
    {
        if(frontView.activeSelf)
        {
            frontView.SetActive(false);
            backView.SetActive(true);
        }
        else
        {
            frontView.SetActive(true);
            backView.SetActive(false);
        }
    }

}
