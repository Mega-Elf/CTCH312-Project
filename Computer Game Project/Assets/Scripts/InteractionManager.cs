using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoCrate hoveredAmmoCrate = null;

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

            // Weapon part hit
            if (objectHitByRaycast.GetComponentInParent<Weapon>())
            {
                // Get the whole weapon
                GameObject parentOfObjectHit = hit.transform.parent.gameObject;

                hoveredWeapon = parentOfObjectHit.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

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
                }
            }

            // Ammo Crate part hit
            if (objectHitByRaycast.GetComponentInParent<AmmoCrate>())
            {
                // Get the whole ammo crate
                GameObject parentOfObjectHit = hit.transform.parent.gameObject;

                hoveredAmmoCrate = parentOfObjectHit.gameObject.GetComponent<AmmoCrate>();
                hoveredAmmoCrate.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpAmmo(hoveredAmmoCrate);
                    // Reduce player score? spend points for ammo
                }
            }
            else
            {
                if (hoveredAmmoCrate)
                {
                    hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
