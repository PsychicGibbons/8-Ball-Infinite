using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    //Assignments
    [SerializeField] private Button DamageButton;
    [SerializeField] private Button ResistanceButton;
    [SerializeField] private Button HealButton;
    [SerializeField] private TextMeshProUGUI ButtonDmgText, ButtonResText, ButtonHealText;

    private float damageCost = 20, resistanceCost = 20, healCost = 30;
    private int damageLvl, resistanceLvl;

    private PointReader pointHolder;
    private playerController playerController;

    private void Start()
    {
        pointHolder = FindAnyObjectByType<PointReader>();
        playerController = FindAnyObjectByType<playerController>();
    }

    public void UpgradeDamage()
    {
        if (pointHolder.GetPoints() >= damageCost && damageLvl < 5)
        {
            pointHolder.UpdatePoints(-damageCost);
            playerController.damageGivenMultiplier += 0.5f;
            damageCost *= 1.5f;
            damageLvl++;
            ButtonDmgText.text = "Damage Upgrade: \n" + damageCost + " Points";
        }
    }
    public void UpgradeResistance()
    {
        if (pointHolder.GetPoints() >= damageCost && resistanceLvl < 5)
        {
            pointHolder.UpdatePoints(-damageCost);
            playerController.damageTakenMultiplier -= 0.1f;
            resistanceCost *= 1.5f;
            resistanceLvl++;
            ButtonResText.text = "Resistance Upgrade: \n" + resistanceCost + " Points";
        }
    }

    public void HealPlayer()
    {
        if(pointHolder.GetPoints() >= healCost)
        {
            playerController.health = 100;
            healCost *= 1.5f;
            ButtonHealText.text = "Full Heal: \n" + healCost + " Points";
        }
    }
}
