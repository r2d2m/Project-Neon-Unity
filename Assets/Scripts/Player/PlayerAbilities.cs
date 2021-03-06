﻿using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Dash Ability")]
    [SerializeField] private float dashForce = 50f;
    [SerializeField] private int numberOfDashes = 3;
    [Tooltip("How long it takes before dash can be used again")]
    [SerializeField] private float dashRechargeTime = 2f;
    [SerializeField] private ParticleSystem dashParticles;
    [SerializeField] private AudioClip dashSfx;
    [SerializeField] private float dashSfxVolume = 0.5f;

    [Header("Crash Ability")]
    [SerializeField] private int numberOfCrashes = 1;
    [SerializeField] private float crashRechargeTime = 4f;
    [SerializeField] private GameObject crashParticles;
    [SerializeField] private AudioClip crashSfx;
    [SerializeField] private float crashSfxVolume = 0.5f;

    [Header("Overdrive Ability")]
    [SerializeField] private int numberOfOverdrives = 1;
    [SerializeField] private float overdrivePeriod = 5f;
    [SerializeField] private float overdriveRechargeTime = 10f;
    [SerializeField] private ParticleSystem overdriveParticles;
    [SerializeField] private float overdriveUpgradeAmount = 2.5f;
    [SerializeField] private AudioClip overdriveSfx;
    [SerializeField] private float overdriveSfxVolume = 0.5f;

    [Header("Should null except for tutorial level")]
    [SerializeField] private AbilityTutorial tutorial;

    private bool isOverdriveActive = false;
    private bool canOverdrive = false;

    private PlayerMovement playerMove;
    private Rigidbody playerRb;
    private Animator anim;
    private AudioSource audioSource;

    public event System.Action<int> OnDashChange;
    public event System.Action<int> OnCrashChange;
    public event System.Action<bool> OnODChange;
    public event System.Action OnDashEnable;
    public event System.Action OnCrashEnable;
    public event System.Action OnOdEnable;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeAbilityValues();
        playerMove = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    private void InitializeAbilityValues()
    {
        this.numberOfDashes = GamewideControl.instance.NumberOfDashes;
        this.numberOfCrashes = GamewideControl.instance.NumberOfCrashes;
        this.overdrivePeriod = GamewideControl.instance.OverdrivePeriod;
        this.numberOfOverdrives = 1; // In case scene transitions before recharge
        OnDashChange(numberOfDashes);
        OnCrashChange(numberOfCrashes);
        OnODChange(false);
        if (overdrivePeriod != 0)
        {
            this.canOverdrive = true; // See above
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            Dash();
        }
        if (Input.GetButtonDown("Crash"))
        {
            Crash();
        }
        if (Input.GetButtonDown("Overdrive"))
        {
            Overdrive();
        }


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            numberOfCrashes = 100;
            numberOfDashes = 100;
            numberOfOverdrives = 100;
        }
#endif

    }

    private void Dash()
    {
        if (numberOfDashes > 0 && !isOverdriveActive)
        {
            dashParticles.Play();
            anim.SetTrigger("Dash");
            playerRb.AddForce(playerMove.GetInputDirection() * dashForce, ForceMode.Impulse);
            numberOfDashes--;
            OnDashChange(numberOfDashes);
            StartCoroutine(DashRecharge());
            audioSource.PlayOneShot(dashSfx, dashSfxVolume);
        }
        else if (isOverdriveActive)
        {
            audioSource.PlayOneShot(dashSfx, dashSfxVolume);
            dashParticles.Play();
            anim.SetTrigger("Dash");
            playerRb.AddForce(playerMove.GetInputDirection() * dashForce, ForceMode.Impulse);
        }
    }

    private IEnumerator DashRecharge() // TODO sfx for recharge
    {
        yield return new WaitForSeconds(dashRechargeTime);
        numberOfDashes++;
        OnDashChange(numberOfDashes);
    }

    public void UpgradeDash()
    {
        numberOfDashes++;
        OnDashChange(numberOfDashes);
        if (tutorial)
        {
            OnDashEnable();
        }
    }

    private void Crash()
    {
        if (numberOfCrashes > 0 && !isOverdriveActive)
        {
            audioSource.PlayOneShot(crashSfx, crashSfxVolume);
            Instantiate(crashParticles, transform.position, Quaternion.identity);
            anim.SetTrigger("Crash");
            numberOfCrashes--;
            OnCrashChange(numberOfCrashes);
            StartCoroutine(CrashRecharge());
        }
        else if (isOverdriveActive)
        {
            audioSource.PlayOneShot(crashSfx, crashSfxVolume);
            Instantiate(crashParticles, transform.position, Quaternion.identity);
            anim.SetTrigger("Crash");
        }
    }

    private IEnumerator CrashRecharge()
    {
        yield return new WaitForSeconds(crashRechargeTime);
        numberOfCrashes++;
        OnCrashChange(numberOfCrashes);
    }

    public void UpgradeCrash()
    {
        numberOfCrashes++;
        OnCrashChange(numberOfCrashes);
        if (tutorial)
        {
            OnCrashEnable();
        }
    }

    private void Overdrive()
    {
        if (numberOfOverdrives > 0 && canOverdrive)
        {
            audioSource.PlayOneShot(overdriveSfx, overdriveSfxVolume);
            isOverdriveActive = true;
            OnODChange(false);
            OnDashChange(999);
            OnCrashChange(999);
            BroadcastMessage("OverdriveStart");
            overdriveParticles.Play();
            anim.SetTrigger("Overdrive");
            numberOfOverdrives--;
            StartCoroutine(OverdrivePeriod());
        }
    }

    private IEnumerator OverdrivePeriod()
    {
        yield return new WaitForSeconds(overdrivePeriod);
        isOverdriveActive = false;
        BroadcastMessage("OverdriveStop");
        OnCrashChange(numberOfCrashes);
        OnDashChange(numberOfDashes);
        audioSource.Stop();
        overdriveParticles.Stop();
        StartCoroutine(OverdriveRecharge());
    }

    private IEnumerator OverdriveRecharge()
    {
        yield return new WaitForSeconds(overdriveRechargeTime);
        OnODChange(true);
        numberOfOverdrives++;
    }

    public void UpgradeOverdrive()
    {
        canOverdrive = true;
        overdrivePeriod += overdriveUpgradeAmount;
        OnODChange(true);
        if (tutorial)
        {
            OnOdEnable();
        }
    }

    private void OverdriveStart()
    {
        dashForce = dashForce / 2;
    }

    private void OverdriveStop()
    {
        dashForce *= 2;
    }

    private void OnDestroy()
    {
        GamewideControl.instance.NumberOfDashes = this.numberOfDashes;
        GamewideControl.instance.NumberOfCrashes = this.numberOfCrashes;
        GamewideControl.instance.OverdrivePeriod = this.overdrivePeriod;
    }
}
