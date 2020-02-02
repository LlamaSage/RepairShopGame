using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestOffer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI questText, repairPrice, replacePrice;
    public Custom customer;
    public Button acceptRepair, acceptReplace;

    public void AssignCustomer(Custom c)
    {
        if(!c.isBeingWorkedOn)
        {
            customer = c;
            questText.text = c.quest.questText;
            repairPrice.text = "Repair Bill: $" + c.quest.repairPrice.ToString("c2");
            replacePrice.text = "Replace Bill: $" + c.quest.replacePrice.ToString("c2");
            acceptRepair.interactable = true;
            acceptReplace.interactable = true;
        }
        else
        {
            questText.text = "I am already being worked on!";
            repairPrice.text = "Repair Bill: $" + c.quest.repairPrice.ToString("c2");
            replacePrice.text = "Replace Bill: $" + c.quest.replacePrice.ToString("c2");
            acceptRepair.interactable = false;
            acceptReplace.interactable = false;
        }
    }

    public void RepairButtonClick()
    {
        GameManager.Instance.acceptedRepairs.Add(customer);
        customer.isBeingWorkedOn = true;
        this.gameObject.SetActive(false);
    }

    public void ReplaceButtonClick()
    {
        GameManager.Instance.acceptedReplaces.Add(customer);
        customer.isBeingWorkedOn = true;
        this.gameObject.SetActive(false);
    }
}
