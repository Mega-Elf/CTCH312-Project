using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoCrate hoveredAmmoCrate = null;

    public Transform player;
    public float distanceFromObject;
    public float pickUpRange = 2.5f;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
            
            // Get distance from root (from whole object for objects placeds in main hierarchy, which is where interactables need to be placed)
            // doesn't give errors for any other objects.
            distanceFromObject = Vector3.Distance(player.position, objectHitByRaycast.transform.root.position);

            if (distanceFromObject <= pickUpRange) // within range of object
            {
                // Weapon part hit
                if (objectHitByRaycast.GetComponentInParent<Weapon>())
                {
                    // Get the whole weapon object
                    GameObject parentOfObjectHit = hit.transform.parent.gameObject;

                    hoveredWeapon = parentOfObjectHit.gameObject.GetComponent<Weapon>();
                    hoveredWeapon.GetComponent<Outline>().enabled = true;

                    Weapon.WeaponModel model = hoveredWeapon.currentWeaponModel;
                    switch (model)
                    {
                        case Weapon.WeaponModel.Glock18:
                            UIManager.Instance.weaponBuyUI.GetComponent<TextMeshProUGUI>().text = $"Glock 18\r\nPress F to Pick Up";
                            break;
                        case Weapon.WeaponModel.AK47:
                            UIManager.Instance.weaponBuyUI.GetComponent<TextMeshProUGUI>().text = $"AK-47\r\nPress F to Pick Up";
                            break;
                    }
                    UIManager.Instance.weaponBuyUI.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickUpWeapon(parentOfObjectHit.gameObject);
                    }
                }
                else
                {
                    if (hoveredWeapon)
                    {
                        hoveredWeapon.GetComponent<Outline>().enabled = false;
                        UIManager.Instance.weaponBuyUI.gameObject.SetActive(false);
                    }
                }

                // Ammo Crate part hit
                if (objectHitByRaycast.GetComponentInParent<AmmoCrate>())
                {
                    // Get the whole ammo crate object
                    GameObject parentOfObjectHit = hit.transform.parent.gameObject;

                    hoveredAmmoCrate = parentOfObjectHit.gameObject.GetComponent<AmmoCrate>();
                    hoveredAmmoCrate.GetComponent<Outline>().enabled = true;
                    UIManager.Instance.ammoBuyUI.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (UIManager.Instance.scoreCount >= 1000)
                        {
                            UIManager.Instance.scoreCount -= 1000;
                            WeaponManager.Instance.PickUpAmmo(hoveredAmmoCrate);
                            print("Ammo bought successfully");
                        }
                        else
                        {
                            print("Not enough points to buy ammo");
                        }
                    }
                }
                else
                {
                    if (hoveredAmmoCrate)
                    {
                        hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
                        UIManager.Instance.ammoBuyUI.SetActive(false);
                    }
                }
            }
            else // out of range of object
            {
                if (hoveredWeapon) // a weapon was hovered
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false; // disable its outline
                    UIManager.Instance.weaponBuyUI.gameObject.SetActive(false);
                }

                if (hoveredAmmoCrate) // a ammo crate was hovered
                {
                    hoveredAmmoCrate.GetComponent<Outline>().enabled = false; // disable its outline
                    UIManager.Instance.ammoBuyUI.SetActive(false);
                }
            }
        }
    }
}
