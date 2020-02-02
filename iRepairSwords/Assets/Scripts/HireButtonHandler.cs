using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireButtonHandler : MonoBehaviour
{
    public GameObject hireMenu;

    public void ToggleHireMenu()
    {
        hireMenu.SetActive(!hireMenu.activeSelf);
    }
}
