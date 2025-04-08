using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletDamage;

    private void OnCollisionEnter(Collision objectHit)
    {
        if (objectHit.gameObject.CompareTag("Target"))
        {
            print("hit " + objectHit.gameObject.name);
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }
        else if (objectHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }
        else
        {
            print("hit something other than target or wall");
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }

        if (objectHit.gameObject.CompareTag("Zombie"))
        {
            print("hit " + objectHit.gameObject.name + " for " + bulletDamage + " damage.");
            CreateBulletImpactEffect(objectHit);
            objectHit.gameObject.GetComponentInParent<Zombie>().TakeDamage(bulletDamage, objectHit.gameObject);
            Destroy(gameObject);
        }

        if (objectHit.gameObject.CompareTag("ZombieHead"))
        {
            print("hit " + objectHit.gameObject.name + " in the head for " + bulletDamage * 1.5f + " damage.");
            CreateBulletImpactEffect(objectHit);
            objectHit.gameObject.GetComponentInParent<Zombie>().TakeDamage(bulletDamage * 1.5f, objectHit.gameObject);
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        if (objectHit.gameObject.CompareTag("Metal"))
        {
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactMetalEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
                );

            hole.transform.SetParent(objectHit.gameObject.transform);
        }
        else if (objectHit.gameObject.CompareTag("Wood"))
        {
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactWoodEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
                );

            hole.transform.SetParent(objectHit.gameObject.transform);
        }
        else if (objectHit.gameObject.CompareTag("Sand"))
        {
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactSandEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
                );

            hole.transform.SetParent(objectHit.gameObject.transform);
        }
        else if (objectHit.gameObject.CompareTag("Zombie") || objectHit.gameObject.CompareTag("ZombieHead")) // zombie headshot or bodyshot
        {
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactFleshEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
                );

            hole.transform.SetParent(objectHit.gameObject.transform);
        }
        else if (objectHit.gameObject.CompareTag("Wall")) // stone
        {
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactStoneEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
                );

            hole.transform.SetParent(objectHit.gameObject.transform);
        }
        // else dont make any effect

    }
}
