using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSliderUpdater : MonoBehaviour
{
    public Slider PersonSlider, TechSlider, LegalSlider;

    

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.acceptedRepairs.Count > 0)
        {
            if(TechSlider.maxValue != GameManager.Instance.acceptedRepairs[0].quest.repairTime)
            {
                TechSlider.maxValue = GameManager.Instance.acceptedRepairs[0].quest.repairTime;
            }
            TechSlider.value = GameManager.Instance.currentRepairProgress;
        }
        else
        {
            TechSlider.maxValue = 1.0f;
            TechSlider.value = 0.0f;
        }


        if (GameManager.Instance.acceptedReplaces.Count > 0)
        {
            if (PersonSlider.maxValue != GameManager.Instance.acceptedReplaces[0].quest.sellTime)
            {
                PersonSlider.maxValue = GameManager.Instance.acceptedReplaces[0].quest.sellTime;
            }
            PersonSlider.value = GameManager.Instance.currentReplaceProgress;
        }
        else
        {
            PersonSlider.maxValue = 1.0f;
            PersonSlider.value = 0.0f;
        }

        LegalSlider.value = GameManager.Instance.currentAttackCounter;
    }
}
