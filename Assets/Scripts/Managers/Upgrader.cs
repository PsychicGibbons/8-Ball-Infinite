using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    //Assignments
    [SerializeField] private Button DamageButton;
    [SerializeField] private Button ResistanceButton;
    [SerializeField] private Button HealButton;
    [SerializeField] private HealthBar healthSlider;
    [SerializeField] private TextMeshProUGUI ButtonDmgText, ButtonResText, ButtonHealText;

    //initial costs are hard coded at the start for simplicity
    private float damageCost = 20, resistanceCost = 20, healCost = 30;
    private int damageLvl, resistanceLvl;

    //Upgrades want to change the player's number of points and values in the player controller script
    private PointReader pointHolder;
    private playerController playerController;

    private void Start()
    {
        pointHolder = FindAnyObjectByType<PointReader>();
        playerController = FindAnyObjectByType<playerController>();
    }

    //Called via a button press
    public void UpgradeDamage()
    {
        //If we have enough points for the upgrade and it isn't already at max level:
        if (pointHolder.GetPoints() >= damageCost && damageLvl < 4)
        {
            //Spend the amount of points, increase the modifier to the amount of player damage given, increase the cost of the next upgrade and what lvl it's at
            pointHolder.UpdatePoints(-damageCost);
            playerController.damageGivenMultiplier += 1;
            damageCost = Mathf.Round(damageCost *= 1.5f);
            damageLvl++;
            //Update the text on the button to show the cost of the next upgrade
            ButtonDmgText.text = "Damage Upgrade: \n" + damageCost + " Points";
            //If the upgrade has reached max level, instead show it cannot be upgraded further
            if(damageLvl >= 4)
            {
                ButtonDmgText.text = "Damage Upgrade: \n Max";
            }
        }
    }
    //Called via a button press
    public void UpgradeResistance()
    {
        //If we have enough points for the upgrade and it isn't already at max level:
        if (pointHolder.GetPoints() >= resistanceCost && resistanceLvl < 4)
        {
            //Spend the amount of points, decrease the modifier to the amount of player damage taken, increase the cost of the next upgrade and the lvl it is at
            pointHolder.UpdatePoints(-resistanceCost);
            playerController.damageTakenMultiplier -= 0.1f;
            resistanceCost = Mathf.Round(resistanceCost *= 1.5f);
            resistanceLvl++;
            //Update the text on the button to show the cost of the next upgrade
            ButtonResText.text = "Resistance Upgrade: \n" + resistanceCost + " Points";
            //If the upgrade has reached max level, instead show it cannot be upgraded further
            if (resistanceLvl >= 4)
            {
                ButtonResText.text = "Resistance Upgrade: \n Max";
            }
        }
    }

    //Called via a button press
    public void HealPlayer()
    {
        //If we have enough points for the heal, there isn't a maximum amount of heals but it gets a lot more expensive:
        if(pointHolder.GetPoints() >= healCost)
        {
            //Spend the number of points, reset health and health bar to the maximum value, then increase the cost of the next heal
            pointHolder.UpdatePoints(-healCost);
            playerController.health = 100;
            healthSlider.UpdateHealthBar(100, 100);
             healCost = Mathf.Round(healCost *= 1.5f);
            //Update point text on the canvas to reflect the new heal cost
            ButtonHealText.text = "Full Heal: \n" + healCost + " Points";
        }
    }
}
