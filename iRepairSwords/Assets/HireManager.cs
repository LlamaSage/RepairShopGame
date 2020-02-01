using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireManager : MonoBehaviour
{
    public GameObject EmployeeProfilePrefab;
    public GameObject NoHiresText;
    public List<GameObject> createdProfiles;

    private void OnEnable()
    {
        var _toDestroy = GameObject.FindGameObjectsWithTag("EmployeeProfile");
        for (int i = 0; i < _toDestroy.Length; i++)
        {
            Destroy(_toDestroy[i]);
        }

        if (GameManager.Instance.potentialEmployees.Count <= 0)
        {
            NoHiresText.SetActive(true);
            return;
        }
        else
        {
            NoHiresText.SetActive(false);
        }
        foreach(Employ e in GameManager.Instance.potentialEmployees)
        {
            GameObject go = Instantiate(EmployeeProfilePrefab, this.transform);
            go.GetComponent<EmployeesProfileHandler>().SetEmployee(e);

        }
    }
}
