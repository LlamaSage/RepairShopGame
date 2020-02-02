using UnityEngine;

public class Employ
{
    public string employeeID;
    public int[] skills = { 0, 0, 0 };
    public float wage = 10.0f;
    private static int currentID = 0;
    public bool isHired = false;
    public WorkSpaceScript currentWorkSpace;

    public enum Job
    {
        unemployed,
        sales,
        repair,
        legal
    }
    public Job currentJob = 0;

    public Employ()
    {
        skills[0] = Random.Range(1, 11);
        skills[1] = Random.Range(1, 11);
        skills[2] = Random.Range(1, 11);
        this.wage = CalculateWage();
        this.employeeID = currentID.ToString();
        currentID++;
    }


    private float CalculateWage()
    {
        float _wage = 10.0f;
        foreach(int i in skills)
            _wage += i * i / 10;

        int skillsum = skills[0] + skills[1] + skills[2];
        if (skillsum > 25)
            _wage *= 0.6f;
        else if (skillsum > 20)
            _wage *= 0.7f;
        else if (skillsum > 15)
            _wage *= 0.8f;


        return _wage;
    }
}