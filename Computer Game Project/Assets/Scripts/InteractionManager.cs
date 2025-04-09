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

    public GameObject questItemStep1;
    public GameObject questItemStep3;
    public bool questItemStep3Activated = false;
    public GameObject hoveredQuestItem = null;

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
                // if player is on step 1, and looking at quest item 1
                if (UIManager.Instance.currentQuestStep == 1 && objectHitByRaycast == questItemStep1)
                {
                    hoveredQuestItem = objectHitByRaycast;
                    objectHitByRaycast.GetComponent<Outline>().enabled = true; // highlight it
                    UIManager.Instance.questPickUpUI.gameObject.SetActive(true); // display prompt to pick up

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        UIManager.Instance.currentQuestStep++; // go to next step, step 2
                        UIManager.Instance.questGuideUI.text = $"Compass retrieved! Continue exploring the forest."; // update quest hint
                        Destroy(objectHitByRaycast); // remove compass, looks like you picked it up
                        UIManager.Instance.questPickUpUI.gameObject.SetActive(false); // hide pick up prompt
                    }
                }

                // if on step 3, looking at step 3 item, not already activated
                if (UIManager.Instance.currentQuestStep == 3 && objectHitByRaycast == questItemStep3 && questItemStep3Activated == false)
                {
                    hoveredQuestItem = objectHitByRaycast;
                    objectHitByRaycast.GetComponent<Outline>().enabled = true; // highlight it
                    UIManager.Instance.questInteractUI.gameObject.SetActive(true); // display prompt to interact

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        questItemStep3Activated = true; // player can't interacte more than once
                        UIManager.Instance.currentQuestStep++; // go to next step, step 4
                        UIManager.Instance.questGuideUI.text = $"S.O.S. sent out! Approach the lake..."; // update quest hint
                        UIManager.Instance.questInteractUI.gameObject.SetActive(false); // hide prompt to interact
                        objectHitByRaycast.GetComponent<Outline>().enabled = false; // disable highlight
                    }
                }

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
                            UIManager.Instance.weaponPickUpUI.GetComponent<TextMeshProUGUI>().text = $"Glock 18\r\nPress F to Pick Up";
                            break;
                        case Weapon.WeaponModel.AK47:
                            UIManager.Instance.weaponPickUpUI.GetComponent<TextMeshProUGUI>().text = $"AK-47\r\nPress F to Pick Up";
                            break;
                    }
                    UIManager.Instance.weaponPickUpUI.gameObject.SetActive(true);

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
                        UIManager.Instance.weaponPickUpUI.gameObject.SetActive(false);
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
                if (hoveredQuestItem) // a quest item was hovered
                {
                    hoveredQuestItem.GetComponent<Outline>().enabled = false; // disable its outline
                    UIManager.Instance.questPickUpUI.gameObject.SetActive(false); // hide prompt to pick up
                    UIManager.Instance.questInteractUI.gameObject.SetActive(false); // hide prompt to interact
                }

                if (hoveredWeapon) // a weapon was hovered
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false; // disable its outline
                    UIManager.Instance.weaponPickUpUI.gameObject.SetActive(false);
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
