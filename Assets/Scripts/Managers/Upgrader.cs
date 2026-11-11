using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private Button DamageButton;
    [SerializeField] private Button ResistanceButton;
    [SerializeField] private Button HealButton;
    private float damageCost = 2, ResistanceCost = 2, healCost = 3;

    private playerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<playerController>();
    }

    private void UpgradeDamage()
    {
        if (playerController.points >= damageCost)
        {
            playerController.damageGivenMultiplier += 0.5f;
        }
    }
}
