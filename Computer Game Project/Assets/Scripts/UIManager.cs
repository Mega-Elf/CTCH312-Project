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

    [Header("Other")]
    public Sprite emptySlot;

    public GameObject crosshair;

    public TextMeshProUGUI roundCountUI;
    public int roundCount = 1;
    public TextMeshProUGUI killCountUI;
    public int killCount = 0;

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

        switch (killCount) // increase round based on kills
        {
            case 2: // round 1 complete
                if (roundCount == 1)
                {
                    roundCount++; // go to round 2
                }
                break;
            case 4: // round 2 complete
                if (roundCount == 2)
                {
                    roundCount++; // go to round 3
                }
                break;
            case 6: // round 3 complete
                if (roundCount == 3)
                {
                    roundCount++; // go to round 4
                }
                break;
            case 45: // round 4 complete
                if (roundCount == 4)
                {
                    roundCount++; // go to round 5
                }
                break;
            case 69: // round 5 complete
                if (roundCount == 5)
                {
                    roundCount++; // go to round 6
                }
                break;
            case 96: // round 6 complete
                if (roundCount == 6)
                {
                    roundCount++; // go to round 7
                }
                break;
            case 124: // round 7 complete
                if (roundCount == 7)
                {
                    roundCount++; // go to round 8
                }
                break;
            default:
                break;
        }

        // Update kill count
        if (killCountUI.text != $"{killCount}")
        {
            killCountUI.text = $"{killCount}";
        }

        // Update round count
        if (roundCountUI.text != $"{roundCount}")
        {
            roundCountUI.text = $"{roundCount}";
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
