﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSpace : MonoBehaviour
{
    public bool isOccupied = false;
    public Custom occupant;
    public GameObject occupantGraphic;
    public int waitSpaceID;
    public static int currentID = 0;

    public void Start()
    {
        //GameManager.Instance.waitSpaces.Add(this);
        occupantGraphic.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
        //waitSpaceID = currentID++;
    }

    public void SetOccupant(Custom c)
    {
        occupant = c;
        c.currentWaitingSpace = this;
        isOccupied = true;
        occupantGraphic.SetActive(true);
    }

    public void RemoveOccupant()
    {
        occupant = null;
        isOccupied = false;
        occupantGraphic.SetActive(false);
    }

}
