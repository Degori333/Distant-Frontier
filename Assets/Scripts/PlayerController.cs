using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public GameObject[] weapons;
    private PlayerControllerX playerControllerX;
    private Actions actions;
    private Weapon weaponSelected;
    private AudioSource audioSource;
    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI ammoStatus;
    public Slider healthBar;
    public Gradient gradient;
    public Image fill;

    [SerializeField] private float speed;
    private float playerHP = 100;

    public int currentWeapon = 0;
    public int money = 0;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private float horizontalBoundry = 22.0f;
    [SerializeField] private float verticalBoundry = 11.0f;

    public bool canShoot = true;
    public bool alive = true;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        actions = GetComponent<Actions>();
        audioSource = GetComponent<AudioSource>();
        playerControllerX = GetComponent<PlayerControllerX>();
        weapons[0].GetComponent<Weapon>().isBought = true;
        actions.Aiming();
        weaponSelected = weapons[currentWeapon].GetComponent<Weapon>();
        SetMaxHealth();
    }

    public void setPlayerHP(float newHP)
    {
        playerHP = newHP;
        healthBar.value = playerHP;
        fill.color = gradient.Evaluate(1f);
    }
    
    public float getPlayerHP()
    {
        return playerHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStarted)
        {
            PlayerMovement();
            if (!gameManager.pauseMenuUI.activeInHierarchy)
            {
                PlayerMouseFollow();
            }
            OnClickShoot(weaponSelected.bulletCD);
            ReloadWeaponCheck();
            ChooseWeapon();
        }
    }

    private void SetMaxHealth()
    {
        healthBar.maxValue = playerHP;
        healthBar.value = playerHP;
        fill.color = gradient.Evaluate(1f);
    }

    // Moves player around
    void PlayerMovement()
    {
        if (alive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            transform.position += (Vector3.right * Time.deltaTime * speed * horizontalInput);
            transform.position += (Vector3.forward * Time.deltaTime * speed * verticalInput);

            // Boundary Check
            if (transform.position.x > horizontalBoundry)
            {
                transform.position = new Vector3(horizontalBoundry, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < -horizontalBoundry)
            {
                transform.position = new Vector3(-horizontalBoundry, transform.position.y, transform.position.z);
            }
            if (transform.position.z > verticalBoundry)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, verticalBoundry);
            }
            else if (transform.position.z < -verticalBoundry)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -verticalBoundry);
            }
        }
    }

    // Allows player to switch a weapon
    private void ChooseWeapon()
    {
        if (!weaponSelected.isReloading && !Input.GetMouseButton(0) && canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && weapons[0].GetComponent<Weapon>().isBought && currentWeapon != 0)
            {
                SelectWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && weapons[1].GetComponent<Weapon>().isBought && currentWeapon != 1)
            {
                SelectWeapon(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && weapons[2].GetComponent<Weapon>().isBought && currentWeapon != 2)
            {
                SelectWeapon(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && weapons[3].GetComponent<Weapon>().isBought && currentWeapon != 3)
            {
                SelectWeapon(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && weapons[4].GetComponent<Weapon>().isBought && currentWeapon != 4)
            {
                SelectWeapon(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) && weapons[5].GetComponent<Weapon>().isBought && currentWeapon != 5)
            {
                SelectWeapon(5);
            }
        }
    }

    private void SelectWeapon(int weaponIndex)
    {
        // Set all weapons Inactive
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        // Set a chosen weapon active and make it a current weapon
        weapons[weaponIndex].SetActive(true);
        currentWeapon = weaponIndex;
        canShoot = false;
        float weaponSwitchCD = 0.4f;
        StartCoroutine(CooldownBetweenShots(weaponSwitchCD));
        playerControllerX.SetArsenal(weapons[weaponIndex].name);
        weaponSelected = weapons[currentWeapon].GetComponent<Weapon>();

    }

    IEnumerator CooldownBetweenShots(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    private void ReloadWeaponCheck()
    {
        if (Input.GetKeyDown(KeyCode.R) && !Input.GetMouseButton(0))
        {
            weaponSelected.Reload();
        }
    }

    private void ammoDisplay()
    {
        ammoStatus.text = weaponSelected.ammoLeft + "/" + weaponSelected.ammoInMagazine;
    }

    public IEnumerator Die()
    {
        alive = false;
        actions.Death();
        audioSource.PlayOneShot(audioSource.clip);
        yield return new WaitForSeconds(5);
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
        Destroy(gameObject);
        gameManager.Exit();
    }

    // Makes player shoot a bullet on click
    private void OnClickShoot(float cooldown)
    {
        if (!gameManager.gamePaused && !gameManager.spawnManager.shopOpen && gameManager.gameStarted)
        {
            if (currentWeapon == 0)
            {
                weaponSelected.ammoLeft = 999;
            }
            ammoDisplay();
            if (!weaponSelected.isReloading)
            {
                if (Input.GetMouseButton(0) && canShoot && weaponSelected.ammoInMagazine > 0)
                {
                    canShoot = false;
                    weaponSelected.Shoot();
                    actions.Attack();
                    StartCoroutine(CooldownBetweenShots(cooldown));
                }
                else if (Input.GetMouseButtonDown(0) && weaponSelected.ammoInMagazine <= 0)
                {
                    canShoot = false;
                    weaponSelected.Reload();
                }
            }
        }
    }

    // Makes player follow the mouse
    void PlayerMouseFollow()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}
