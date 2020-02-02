using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOffer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI questText, repairPrice, replacePrice;

    public void AssignCustomer(Custom c)
    {
        questText.text = c.quest.questText;
        repairPrice.text = "Repair Bill: $" + c.quest.repairPrice.ToString("c2");
        replacePrice.text = "Replace Bill: $" + c.quest.replacePrice.ToString("c2");
    }
}
