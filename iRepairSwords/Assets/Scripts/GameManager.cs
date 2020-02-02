using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public float customerFrequency = 5.0f;
    public float frequencyInfluence = 1.0f;
    private float customerTimer = 0.0f;
    public float patienceInfluence = 1.0f;
    public List<Custom> customers = new List<Custom>();
    public List<Employ> employees = new List<Employ>();
    public List<Employ> potentialEmployees = new List<Employ>();
    public List<WorkSpaceScript> workSpaces = new List<WorkSpaceScript>();
    public List<Custom> acceptedRepairs = new List<Custom>();
    public List<Custom> acceptedReplaces = new List<Custom>();
    public WaitSpace[] waitSpaces;
    public GameState currentState = 0;
    public MoneyDisplayHandler moneyDisplay;

    public float currentRepairProgress, currentReplaceProgress;
    public float currentAttackStrength = 1.0f;
    public float currentAttackCounter = 100.0f;
    public float maxAttackCounter = 100.0f;
    public float nextStrengthChange = 5.0f;

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

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Reset()
    {
    }

    void Start()
    {
        Reset();
        RefreshEmployeeCandidates();
        moneyDisplay = GameObject.Find("CurrentMoneyTextField").GetComponent<MoneyDisplayHandler>();
        //GameObject.Find("CurrentMoneyTextField").GetComponent<TMPro.TextMeshProUGUI>().text = "$ " + currentMoney.ToString("c2");
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
        if(acceptedRepairs.Count > 0 && acceptedRepairs[0] == c)
        {
            currentRepairProgress = 0.0f;
        }
        else if (acceptedReplaces.Count > 0 && acceptedReplaces[0] == c)
        {
            currentReplaceProgress = 0.0f;
        }
        acceptedRepairs.Remove(c);
        acceptedReplaces.Remove(c);
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


    /*public void UpdateMoneyCounter()
    {
        GameObject.Find("CurrentMoneyTextField").GetComponent<TMPro.TextMeshProUGUI>().text = "$ " + currentMoney.ToString("c2");
    }*/

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
                //UpdateMoneyCounter();
                if(currentWeek % refreshCandidatesInterval == 0)
                {
                    RefreshEmployeeCandidates();
                }
            }

            //Legal Attacks
            nextStrengthChange -= Time.deltaTime;
            if(nextStrengthChange<=0)
            {
                nextStrengthChange = Random.Range(5.0f, 15.0f);
                currentAttackStrength = Random.Range(0.0f, 31.0f);
            }
            currentAttackCounter -= Time.deltaTime * currentAttackStrength;
            if(currentAttackCounter<=0.0f)
            {
                currentMoney -= 100000.0f;
                //UpdateMoneyCounter();
                currentAttackCounter = 100.0f;
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
                                if(acceptedReplaces.Count>0)
                                {
                                    currentReplaceProgress += Time.deltaTime*ws.occupant.skills[0];
                                    if (currentReplaceProgress >= acceptedReplaces[0].quest.sellTime)
                                    {
                                        currentMoney += acceptedReplaces[0].quest.replacePrice;
                                        //UpdateMoneyCounter();
                                        RemoveCustomer(acceptedReplaces[0]);
                                    }
                                }
                                break;
                            case WorkSpaceScript.WorkStationType.TECNICAL:
                                if (acceptedRepairs.Count > 0)
                                {
                                    currentRepairProgress += Time.deltaTime*ws.occupant.skills[1];
                                    if (currentRepairProgress >= acceptedRepairs[0].quest.repairTime)
                                    {
                                        currentMoney += acceptedRepairs[0].quest.repairPrice;
                                        //UpdateMoneyCounter();
                                        RemoveCustomer(acceptedRepairs[0]);
                                    }
                                }
                                break;
                            case WorkSpaceScript.WorkStationType.LEGAL:
                                currentAttackCounter = Mathf.Min(currentAttackCounter + Time.deltaTime * ws.occupant.skills[2], maxAttackCounter);
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
