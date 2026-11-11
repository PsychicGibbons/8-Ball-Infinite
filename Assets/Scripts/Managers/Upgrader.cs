using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    //Assignments
    [SerializeField] private Button DamageButton;
    [SerializeField] private Button ResistanceButton;
    [SerializeField] private Button HealButton;
    private Text ButtonDmgText, ButtonResText, ButtonHealText;

    private float damageCost = 2, resistanceCost = 2, healCost = 3;
    private int damageLvl, resistanceLvl;

    private playerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<playerController>();
    }

    public void UpgradeDamage()
    {
        if (playerController.points >= damageCost && damageLvl < 5)
        {
            playerController.points -= damageCost;
            playerController.damageGivenMultiplier += 0.5f;
            damageCost *= 1.5f;
            damageLvl++;
        }
    }
    public void UpgradeResistance()
    {
        if (playerController.points >= damageCost && damageLvl < 5)
        {
            playerController.points -= damageCost;
            playerController.damageTakenMultiplier -= 0.1f;
            resistanceCost *= 1.5f;
            resistanceLvl++;
        }
    }

    public void HealPlayer()
    {
        if(playerController.points >= healCost)
        {
            playerController.health = 100;
            healCost *= 1.5f;
        }
    }
}
