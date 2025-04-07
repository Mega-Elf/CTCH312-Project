using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image inactiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalCountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalCountUI;

    [Header("Player HUD")]
    public Sprite emptySlot;

    public GameObject crosshair;

    public TextMeshProUGUI roundCountUI;
    public int currentRound = 0;
    public TextMeshProUGUI killCountUI;
    public int killCount = 0;

    public Player player;
    public TextMeshProUGUI playerHealthUI;

    [Header("Other")]
    public GameObject crosshairCanvas;
    public GameObject playerHUDCanvas;
    public GameObject gameEndScreenCanvas;
    public TextMeshProUGUI roundsSurvivedUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerHealthUI.text = $"{player.playerHealth}";
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon inactiveWeapon = GetInactiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsRemaining / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoRemaining(activeWeapon.currentWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.currentWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (inactiveWeapon)
            {
                inactiveWeaponUI.sprite = GetWeaponSprite(inactiveWeapon.currentWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            inactiveWeaponUI.sprite = emptySlot;
        }

        // Update kill count
        if (killCountUI.text != $"{killCount}")
        {
            killCountUI.text = $"{killCount}";
        }

        // Update round count
        if (roundCountUI.text != $"{currentRound}")
        {
            roundCountUI.text = $"{currentRound}";
        }

        // Update you survived x rounds
        if (roundsSurvivedUI.text != $"You Survived {currentRound - 1} Rounds")
        {
            roundsSurvivedUI.text = $"You Survived {currentRound - 1} Rounds";
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Glock18:
                return Resources.Load<GameObject>("Glock18_Weapon").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.AK47:
                return Resources.Load<GameObject>("AK47_Weapon").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Glock18:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.AK47:
                return Resources.Load<GameObject>("AR_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetInactiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null; // this won't happen, but we have to return something
    }
}
