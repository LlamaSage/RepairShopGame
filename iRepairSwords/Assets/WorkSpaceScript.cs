using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSpaceScript : MonoBehaviour
{
    public bool isOccupied = false;
    public Employ occupant;
    public static bool isShowingInfo = false;
    public Transform InfoPanel;
    public GameObject EmployeeProfilePrefab;
    private static GameObject spawnedInfo;
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
        GameManager.Instance.workSpaces.Add(this);
        occupantGraphic.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
        InfoPanel = GameObject.Find("InfoPanel").transform;
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
                if(spawnedInfo.GetComponent<EmployeesProfileHandler>().employee == occupant)
                {
                    Destroy(spawnedInfo.gameObject);
                    spawnedInfo = null;
                    isShowingInfo = false;
                }
                else
                {
                    Destroy(spawnedInfo.gameObject);
                    GameObject go = Instantiate(EmployeeProfilePrefab, InfoPanel);
                    go.GetComponent<EmployeesProfileHandler>().SetEmployee(occupant);
                    spawnedInfo = go;
                }
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
            Destroy(spawnedInfo.gameObject);
            spawnedInfo = null;
            isShowingInfo = false;
        }
        occupant.currentWorkSpace = null;
        occupant = null;
        isOccupied = false;
        occupantGraphic.SetActive(false);
    }
}
