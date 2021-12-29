using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    private PlayerController player;
    private int healthPackPrice = 200;
    private GameObject[] buttons;
    private SpawnManager spawnManager;
    public GameObject statusWindow;
    public TextMeshProUGUI playerMoney;
    public Slider shopPlayerHP;
    public Image fill;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        buttons = GameObject.FindGameObjectsWithTag("Shop Button");
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            ButtonStatus();
            shopPlayerHP.value = player.healthBar.value;
            fill.color = player.fill.color;
        }
    }

    private void ButtonStatus()
    {
        playerMoney.text = player.money + "$";
        for (int i = 1; i < player.weapons.Length; i++)
        {
            Weapon weapon = player.weapons[i].GetComponent<Weapon>();
            if (!weapon.isBought)
            {
                if (player.money < weapon.price)
                {
                    buttons[i - 1].GetComponent<Button>().interactable = false;
                }
                else
                {
                    buttons[i - 1].GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                if (player.money < weapon.ammoPrice)
                {
                    buttons[i - 1].GetComponent<Button>().interactable = false;
                }
                else
                {
                    buttons[i - 1].GetComponent<Button>().interactable = true;
                }
            }
        }
        if (player.money < healthPackPrice || player.getPlayerHP() == 100)
        {
            buttons[5].GetComponent<Button>().interactable = false;
        }
        else
        {
            buttons[5].GetComponent<Button>().interactable = true;
        }
    }

    public void BuyShotgun()
    {
        Weapon weapon = player.weapons[1].GetComponent<Weapon>();
        if (!weapon.isBought) {
            if (player.money >= weapon.price)
            {
                player.money -= weapon.price;
                weapon.isBought = true;
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Shotgun\nAmmo - " + weapon.ammoPrice +"$";
            }
        }
        else
        {
            if(player.money >= weapon.ammoPrice)
            {
                player.money -= weapon.ammoPrice;
                weapon.ammoLeft += weapon.magazineCapacity*2;
            }
        }
    }

    public void BuyAssaultRiffle()
    {
        Weapon weapon = player.weapons[2].GetComponent<Weapon>();
        if (!weapon.isBought)
        {
            if (player.money >= weapon.price)
            {
                player.money -= weapon.price;
                weapon.isBought = true;
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Assault Riffle\nAmmo - " + weapon.ammoPrice + "$";
            }
        }
        else
        {
            if (player.money >= weapon.ammoPrice)
            {
                player.money -= weapon.ammoPrice;
                weapon.ammoLeft += weapon.magazineCapacity * 2;
            }
        }
    }

    public void BuySemiAutoRifle()
    {
        Weapon weapon = player.weapons[3].GetComponent<Weapon>();
        if (!weapon.isBought)
        {
            if (player.money >= weapon.price)
            {
                player.money -= weapon.price;
                weapon.isBought = true;
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Semi-Automatic Rifle\nAmmo - " + weapon.ammoPrice + "$";
            }
        }
        else
        {
            if (player.money >= weapon.ammoPrice)
            {
                player.money -= weapon.ammoPrice;
                weapon.ammoLeft += weapon.magazineCapacity * 2;
            }
        }
    }

    public void BuyMachineGun()
    {
        Weapon weapon = player.weapons[4].GetComponent<Weapon>();
        if (!weapon.isBought)
        {
            if (player.money >= weapon.price)
            {
                player.money -= weapon.price;
                weapon.isBought = true;
                buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Machine Gun\nAmmo - " + weapon.ammoPrice + "$";
            }
        }
        else
        {
            if (player.money >= weapon.ammoPrice)
            {
                player.money -= weapon.ammoPrice;
                weapon.ammoLeft += weapon.magazineCapacity * 2;
            }
        }
    }

    public void BuySniperRifle()
    {
        Weapon weapon = player.weapons[5].GetComponent<Weapon>();
        if (!weapon.isBought)
        {
            if (player.money >= weapon.price)
            {
                player.money -= weapon.price;
                weapon.isBought = true;
                buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Sniper Rifle\nAmmo - " + weapon.ammoPrice + "$";
            }
        }
        else
        {
            if (player.money >= weapon.ammoPrice)
            {
                player.money -= weapon.ammoPrice;
                weapon.ammoLeft += 12;
            }
        }
    }

    public void BuyHealthPack()
    {
       if(player.money >= healthPackPrice)
        {
            player.money -= healthPackPrice;
            if (player.getPlayerHP() + 20 < 100)
            {
                player.setPlayerHP(player.getPlayerHP() + 20);
            }
            else
            {
                player.setPlayerHP(100);
            }
        }
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        statusWindow.SetActive(true);
        spawnManager.shopOpen = false;
        Time.timeScale = 1;
    }
}
