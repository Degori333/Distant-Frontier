using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletSpawnPoint;
    public GameObject bulletPrefab;
    public AudioClip reloadSound;
    public AudioClip[] shootingSound;
    private AudioSource audioSource;

    public bool isBought;
    
    public int price;
    public int ammoPrice;
    public int damage;

    public float bulletCD;
    [SerializeField] private int bulletAmount = 1;
    [SerializeField] private float offset = 0;

    public int magazineCapacity = 1;
    public int ammoInMagazine;

    public int ammoLeft;

    public float reloadTime = 1.0f;
    public bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        ammoInMagazine = magazineCapacity;
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator ReloadTime()
    {    
        if (ammoInMagazine != magazineCapacity && ammoLeft > 0)
        {
            audioSource.PlayOneShot(reloadSound);
            yield return new WaitForSeconds(reloadTime);
            gameObject.GetComponentInParent<PlayerController>().canShoot = false;
            if (ammoLeft + ammoInMagazine >= magazineCapacity)
            {
                ammoLeft = ammoLeft + ammoInMagazine - magazineCapacity;
                ammoInMagazine = magazineCapacity;
            }
            else
            {
                ammoInMagazine += ammoLeft;
                ammoLeft = 0;
            }
        }
        
        isReloading = false;
        gameObject.GetComponentInParent<PlayerController>().canShoot = true;
    }

    public void Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadTime());
        }
    }

    public void Shoot()
    {
        if (ammoInMagazine > 0) 
        {
            float currentOffset = 0;
            for (int i = 0; i < bulletAmount; i++)
            {
                currentOffset = i * (offset * 2 / bulletAmount) - offset;
                Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, BulletOffset(currentOffset));
                audioSource.PlayOneShot(shootingSound[Random.Range(0, shootingSound.Length)]);
            }
            ammoInMagazine--;
        }
    }

    private Quaternion BulletOffset(float offset)
    {
        Quaternion bulletRot = GetComponentInParent<PlayerController>().transform.rotation;
        bulletRot *= Quaternion.Euler(90, offset, 0);
        return bulletRot;
    }
    
}
