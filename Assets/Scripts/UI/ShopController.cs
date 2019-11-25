﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerPoints playerPoints;

    [Header("Upgrade Costs")]
    [SerializeField] private int fireRateCost = 100;
    [SerializeField] private int hpIncrementCost = 200;
    [SerializeField] private int dashIncrementCost = 350;
    [SerializeField] private int crashIncrementCost = 500;
    [SerializeField] private int overdriveIncrementCost = 500;

    [Header("Current Levels")]
    [SerializeField] private int fireRateLevel = 1;
    [SerializeField] private int hpLevel = 1;
    [SerializeField] private int dashLevel = 1;
    [SerializeField] private int crashLevel = 1;
    [SerializeField] private int overdriveLevel = 1;

    [Header("Max Level")]
    [SerializeField] private int maxLevel = 4;

    [Header("TMPro UGUI Objects")]
    [SerializeField] private TextMeshProUGUI fireRateCostLabel;
    [SerializeField] private TextMeshProUGUI fireRateLevelLabel;
    [SerializeField] private TextMeshProUGUI hpIncrementCostLabel;
    [SerializeField] private TextMeshProUGUI hpLevelLabel;
    [SerializeField] private TextMeshProUGUI dashIncrementCostLabel;
    [SerializeField] private TextMeshProUGUI dashLevelLabel;
    [SerializeField] private TextMeshProUGUI crashIncrementCostLabel;
    [SerializeField] private TextMeshProUGUI crashLevelLabel;
    [SerializeField] private TextMeshProUGUI overdriveIncrementCostLevel;
    [SerializeField] private TextMeshProUGUI overdriveLevelLabel;

    // Start is called before the first frame update
    void Start()
    {
        shopCanvas.SetActive(false);
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        fireRateCostLabel.text = fireRateCost.ToString();
        fireRateLevelLabel.text = fireRateLevel.ToString();
        hpIncrementCostLabel.text = hpIncrementCost.ToString();
        hpLevelLabel.text = hpLevel.ToString();
        dashIncrementCostLabel.text = dashIncrementCost.ToString();
        dashLevelLabel.text = dashLevel.ToString();
        crashIncrementCostLabel.text = crashIncrementCost.ToString();
        crashLevelLabel.text = crashLevel.ToString();
        overdriveIncrementCostLevel.text = overdriveIncrementCost.ToString();
        overdriveLevelLabel.text = overdriveLevel.ToString();
    }

    public void FireRateUpgradeButton()
    {
        if (fireRateLevel <= maxLevel)
        {
            if (playerPoints.GetPoints() >= fireRateCost)
            {
                playerPoints.SubtractPoints(fireRateCost);
                fireRateCost *= 2;
                player.GetComponentInChildren<GunController>().UpgradeFirerate();
                fireRateLevel++;
                UpdateLabels();
            }
        }        
    }

    public void HPUpgradeButton()
    {
        if (hpLevel <= maxLevel)
        {
            if (playerPoints.GetPoints() >= hpIncrementCost)
            {
                playerPoints.SubtractPoints(hpIncrementCost);
                hpIncrementCost *= 2;
                player.GetComponent<PlayerHealth>().UpgradeHealth();
                hpLevel++;
                UpdateLabels();
            }
        }        
    }

    public void DashUpgradeButton()
    {
        if (dashLevel <= maxLevel)
        {
            if (playerPoints.GetPoints() >= dashIncrementCost)
            {
                playerPoints.SubtractPoints(dashIncrementCost);
                dashIncrementCost *= 2;
                player.GetComponent<PlayerAbilities>().UpgradeDash();
                dashLevel++;
                UpdateLabels();
            }
        }        
    }

    public void CrashUpgradeButton()
    {
        if (crashLevel <= maxLevel)
        {
            if (playerPoints.GetPoints() >= crashIncrementCost)
            {
                playerPoints.SubtractPoints(crashIncrementCost);
                crashIncrementCost *= 2;
                player.GetComponent<PlayerAbilities>().UpgradeCrash();
                crashLevel++;
                UpdateLabels();
            }
        }        
    }

    public void OverdriveUpgradeButton()
    {
        if (overdriveLevel <= maxLevel)
        {
            if (playerPoints.GetPoints() >= overdriveIncrementCost)
            {
                playerPoints.SubtractPoints(overdriveIncrementCost);
                overdriveIncrementCost *= 2;
                player.GetComponent<PlayerAbilities>().UpgradeOverdrive();
                overdriveLevel++;
                UpdateLabels();
            }
        }        
    }

    private void WaveComplete()
    {
        shopCanvas.SetActive(true);
    }

    public void NextWaveButton()
    {
        EnemySpawnManager.Instance.NextWave();
        shopCanvas.SetActive(false);
    }
}