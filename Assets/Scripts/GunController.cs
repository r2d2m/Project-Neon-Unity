﻿using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool isFiring;
    [SerializeField] private Bullet bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float secondsBetweenShots;
    private float shotCounter;

    [SerializeField] private Transform gunLocation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = secondsBetweenShots;
                Bullet newBullet = Instantiate(bullet, gunLocation.position, gunLocation.rotation) as Bullet;
                newBullet.Speed = bulletSpeed;
            }
        }
        else
        {
            shotCounter = 0f;
        }
    }

    private void OverdriveStart()
    {
        if (this.CompareTag("Player"))
        {
            secondsBetweenShots = secondsBetweenShots / 2;
        }
    }

    private void OverdriveStop()
    {
        if (this.CompareTag("Player"))
        {
            secondsBetweenShots = secondsBetweenShots * 2;
        }
    }
}
