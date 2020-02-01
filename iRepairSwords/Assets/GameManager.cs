using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public float customerFrequency = 5.0f;
    public float frequencyInfluence = 1.0f;
    private float customerTimer = 0.0f;
    public float patienceInfluence = 1.0f;
    public List<Custom> customers = new List<Custom>();
    public List<Employ> employees = new List<Employ>();
    public List<Employ> potentialEmployees = new List<Employ>();
    public List<WorkSpaceScript> workSpaces = new List<WorkSpaceScript>();
    public WaitSpace[] waitSpaces;
    public GameState currentState = 0;

    public float currentMoney = 1000000.0f;
    public float weekInterval = 5.0f;
    private float weekTimer = 0.0f;
    private int currentWeek = 0;
    public int refreshCandidatesInterval = 4;

    public bool mouseIsDragging = false;
    public Employ mousePayLoad = null;

    public enum GameState {
        MENU,
        PAUSED,
        PLAY
    };

    protected GameManager()
    {
        
    }
    
    void Start()
    {
        RefreshEmployeeCandidates();
        GameObject.Find("CurrentMoneyTextField").GetComponent<TMPro.TextMeshProUGUI>().text = "$ " + currentMoney.ToString("c2");
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WaitSpace");
        waitSpaces = new WaitSpace[gos.Length];
        foreach(GameObject go in gos)
        {
            waitSpaces[go.GetComponent<WaitSpace>().waitSpaceID] = go.GetComponent<WaitSpace>();
        }
    }

    public void MousePickUp(Employ e)
    {
        mouseIsDragging = true;
        mousePayLoad = e;
    }

    public void MouseDropPickUp()
    {
        mouseIsDragging = false;
        mousePayLoad = null;
    }

    //Refresh potential employees for hire
    public void RefreshEmployeeCandidates()
    {
        potentialEmployees.Clear();
        for(int i = 0; i<3; i++)
        {
            potentialEmployees.Add(new Employ());
        }
    }

    #region Hire And Fire Employees
    public bool HireEmployee(Employ e)
    {
        foreach(WorkSpaceScript ws in workSpaces)
        {
            //Check for available space
            if((!ws.isOccupied) && ws.type == WorkSpaceScript.WorkStationType.NEUTRAL)
            {
                ws.SetOccupant(e);
                e.isHired = true;
                employees.Add(e);
                potentialEmployees.Remove(e);
                return true;
            }
        }
        return false;
    }



    public void FireEmployee(Employ e)
    {
        e.currentWorkSpace.RemoveOccupant();
        employees.Remove(e);
    }
    #endregion


    public float AddWages()
    {
        float _f = 0.0f;
        foreach(Employ e in employees)
        {
            _f += e.wage;
        }
        return _f;
    }

    public void RemoveCustomer(Custom c)
    {
        c.currentWaitingSpace.RemoveOccupant();
        customers.Remove(c);
        for(int i=0; i<waitSpaces.Length-1; i++)
        {
            if((!waitSpaces[i].isOccupied) && waitSpaces[i+1].isOccupied)
            {
                waitSpaces[i].SetOccupant(waitSpaces[i + 1].occupant);
                waitSpaces[i + 1].RemoveOccupant();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (currentState == GameState.PLAY)
        {
            //Customers Spawn
            customerTimer += Time.deltaTime * frequencyInfluence;
            if (customerTimer > customerFrequency)
            {
                Debug.Log("New Customer!");
                foreach(WaitSpace ws in waitSpaces)
                {
                    if(!ws.isOccupied)
                    {
                        Custom cu = new Custom();
                        ws.SetOccupant(cu);
                        customers.Add(cu);
                        ws.isOccupied = true;
                        break;
                    }
                }
                customerTimer = 0.0f;
            }

            //Customers Patience
            for (int i = 0; i < customers.Count; i++)
            {
                customers[i].patience -= Time.deltaTime * patienceInfluence;
                if (customers[i].patience <= 0.0f)
                {
                    Debug.Log("Frustrated!");
                    RemoveCustomer(customers[i]);

                }
            }

            //Week Counter
            weekTimer += Time.deltaTime;
            if(weekTimer>=weekInterval)
            {
                GameObject.Find("CurrentWeekTextField").GetComponent<TMPro.TextMeshProUGUI>().text = "Current Week: " + (++currentWeek);
                weekTimer = 0.0f;
                currentMoney -= AddWages();
                GameObject.Find("CurrentMoneyTextField").GetComponent<TMPro.TextMeshProUGUI>().text = "$ " + currentMoney.ToString("c2");
                if(currentWeek % refreshCandidatesInterval == 0)
                {
                    RefreshEmployeeCandidates();
                }
            }

            //Workstation Progress
            {
                foreach(WorkSpaceScript ws in workSpaces)
                {
                    if(ws.isOccupied)
                    {
                        switch(ws.type)
                        {
                            case WorkSpaceScript.WorkStationType.PERSONAL:
                                break;
                            case WorkSpaceScript.WorkStationType.TECNICAL:
                                break;
                            case WorkSpaceScript.WorkStationType.LEGAL:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }




        }
    }
}
