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
        if (pointHolder.GetPoints() >= damageCost && damageLvl < 4)
        {
            pointHolder.UpdatePoints(-damageCost);
            playerController.damageGivenMultiplier += 1;
            Debug.Log("Dmg amount: " + playerController.damageGivenMultiplier);
            damageCost = Mathf.Round(damageCost *= 1.5f);
            damageLvl++;
            ButtonDmgText.text = "Damage Upgrade: \n" + damageCost + " Points";
            if(damageLvl >= 4)
            {
                ButtonDmgText.text = "Damage Upgrade: \n Max";
            }
        }
    }
    public void UpgradeResistance()
    {
        if (pointHolder.GetPoints() >= damageCost && resistanceLvl < 4)
        {
            pointHolder.UpdatePoints(-damageCost);
            playerController.damageTakenMultiplier -= 0.1f;
            Debug.Log("Res amount: " + playerController.damageTakenMultiplier);
            resistanceCost = Mathf.Round(resistanceCost *= 1.5f);
            resistanceLvl++;
            ButtonResText.text = "Resistance Upgrade: \n" + resistanceCost + " Points";
            if (resistanceLvl >= 4)
            {
                ButtonResText.text = "Resistance Upgrade: \n Max";
            }
        }
    }

    public void HealPlayer()
    {
        if(pointHolder.GetPoints() >= healCost)
        {
            playerController.health = 100;
            healthSlider.UpdateHealthBar(100, 100);
             healCost = Mathf.Round(healCost *= 1.5f);
            ButtonHealText.text = "Full Heal: \n" + healCost + " Points";
        }
    }
}
