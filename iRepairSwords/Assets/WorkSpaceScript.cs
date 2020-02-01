using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSpaceScript : MonoBehaviour
{
    public bool isOccupied = false;
    public Employ occupant;
    public bool isShowingInfo = false;
    public Transform InfoPanel;
    public GameObject EmployeeProfilePrefab;
    private GameObject spawnedInfo;
    public WorkStationType type = 0;
    public GameObject occupantGraphic;

    public enum WorkStationType
    {
        NEUTRAL,
        PERSONAL,
        TECNICAL,
        LEGAL
    }

    private void Start()
    {
        GameManager.Instance.workspaces.Add(this);
    }

    private void OnMouseDown()
    {
        if(isOccupied)
        {
            if (!isShowingInfo)
            {
                GameObject go = Instantiate(EmployeeProfilePrefab, InfoPanel);
                go.GetComponent<EmployeesProfileHandler>().SetEmployee(occupant);
                spawnedInfo = go;
                isShowingInfo = true;
            }
            else
            {
                Destroy(spawnedInfo);
                isShowingInfo = false;
            }
        }
        else
        {
            if(GameManager.Instance.mouseIsDragging)
            {
                SetOccupant(GameManager.Instance.mousePayLoad);
                GameManager.Instance.MouseDropPickUp();
            }
        }

        
    }

    private void OnMouseExit()
    {
        if((!GameManager.Instance.mouseIsDragging) && Input.GetMouseButton(0))
        {
            if(isOccupied)
            {
                GameManager.Instance.MousePickUp(occupant);
                RemoveOccupant();
            }
        }
    }

    /*private void OnMouseUp()
    {
        if(GameManager.Instance.mouseIsDragging && (!isOccupied))
        {
            SetOccupant(GameManager.Instance.mousePayLoad);
            GameManager.Instance.MouseDropPickUp();
        }
    }*/

    public void SetOccupant(Employ e)
    {
        occupant = e;
        isOccupied = true;
        occupantGraphic.SetActive(true);
        occupant.currentWorkSpace = this;
    }

    public void RemoveOccupant()
    {
        if(isShowingInfo)
        {
            Destroy(spawnedInfo);
            isShowingInfo = false;
        }
        occupant.currentWorkSpace = null;
        occupant = null;
        isOccupied = false;
        occupantGraphic.SetActive(false);
    }
}
