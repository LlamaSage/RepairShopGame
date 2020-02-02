using UnityEngine;

public class Custom
{
    public float patience = 12.0f;
    public WaitSpace currentWaitingSpace;
    public Quest quest = new Quest();
}

public class Quest
{
    public string questText;
    public string itemName;
    public float repairPrice, replacePrice, repairTime, sellTime;
    private static string[] itemNamesBase = {
            "iStab",
            "iShoot",
            "iMaim",
            "iSmite",
            "iDestroy"
            };
    private static float repairPriceBase = 20.0f;
    private static float replacePriceBase = 40.0f;
    private static float repairTimeBase = 1.5f;
    private static float sellTimeBase = 3.0f;

    public Quest()
    {
        int r = Random.Range(0, itemNamesBase.Length);
        itemName = itemNamesBase[r];
        r++;
        float randomFactor = Random.Range(0.7f, 1.3f);
        repairPrice = r * repairPriceBase * randomFactor;
        replacePrice = r * replacePriceBase * randomFactor;
        repairTime = r * repairTimeBase * randomFactor;
        sellTime = r * sellTimeBase * randomFactor;
        questText = "Hello, I need my " + itemName + " repaired. Could you help me?";
    }
        
} 