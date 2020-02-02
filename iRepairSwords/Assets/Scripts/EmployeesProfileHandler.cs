using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeesProfileHandler : MonoBehaviour
{
    public Employ employee;
    public TMPro.TextMeshProUGUI empID;
    public Slider personal, technical, legal;
    public TMPro.TextMeshProUGUI empWageField;
    public Text buttonText;

    public void SetEmployee(Employ e)
    {
        this.employee = e;
        UpdateProfile();
    }

    public void UpdateProfile()
    {
        empID.text = employee.employeeID;
        personal.value = employee.skills[0];
        technical.value = employee.skills[1];
        legal.value = employee.skills[2];
        empWageField.text = employee.wage.ToString("c2");
        if(employee.isHired)
        {
            buttonText.text = "Fire";
        }
        else
        {
            buttonText.text = "Hire";
        }
    }

    public void HireButtonPressed()
    {
        if (employee.isHired)
        {
            GameManager.Instance.FireEmployee(employee);
            Destroy(this.gameObject);
        }
        else
        {
            if(GameManager.Instance.HireEmployee(employee))
            {
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("No Space Available");
            }
        }

    }

}
