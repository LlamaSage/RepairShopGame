using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplayHandler : MonoBehaviour
{
    void Start()
    {
        moneyTextField = GetComponent<TMPro.TextMeshProUGUI>();
        moneyTextField.text = "$ " + GameManager.Instance.currentMoney.ToString("c2");
        displayMoney = GameManager.Instance.currentMoney;
        StartCoroutine(MoneyUpdater());
    }

    //You need a variable for the actual score
    
    //And you also need a variable that holds the increasing score number, let's call it display score
    public float displayMoney;
    //Variable for the UI Text that will show the score
    public TMPro.TextMeshProUGUI moneyTextField;
    
    private IEnumerator MoneyUpdater()
    {
        while (true)
        {
            if(Mathf.Abs(displayMoney-GameManager.Instance.currentMoney)>10000)
            {
                if(displayMoney > GameManager.Instance.currentMoney)
                {
                    displayMoney = GameManager.Instance.currentMoney + 100.0f;
                }
                else
                {
                    displayMoney = GameManager.Instance.currentMoney - 100.0f;
                }
            }
            if (displayMoney < GameManager.Instance.currentMoney)
            {
                moneyTextField.color = new Color32(0, 255, 0, 255);
                displayMoney+=1.0f;
            }
            else if ( displayMoney > GameManager.Instance.currentMoney)
            {
                moneyTextField.color = new Color32(255, 0, 0, 255);
                displayMoney-= 1.0f;
            }
            if(GameManager.Instance.currentMoney-1.0f <= displayMoney && displayMoney <= GameManager.Instance.currentMoney+1.0f)
            {
                moneyTextField.color = new Color32(255, 255, 255, 255);
                displayMoney = GameManager.Instance.currentMoney;
            }

            moneyTextField.text = "$ " + displayMoney.ToString("c2");
            yield return new WaitForSeconds(0.01f); // I used .2 secs but you can update it as fast as you want
        }
    }
}
