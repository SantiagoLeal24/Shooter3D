using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    private void OnCollisionEnter(Collision ObjetoTocado)
    {
        if(ObjetoTocado.gameObject.CompareTag("Target"))
        {
            print("hit" + ObjetoTocado.gameObject.name);

            CreateBulletImpactEffect(ObjetoTocado);

            Destroy(gameObject);

            
        }

        if (ObjetoTocado.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");

            CreateBulletImpactEffect(ObjetoTocado);

            Destroy(gameObject);
        }

        if (ObjetoTocado.gameObject.CompareTag("Enemy"))
        {
            ObjetoTocado.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);

            Destroy(gameObject);
        }
    }


    void CreateBulletImpactEffect(Collision ObjetoTocado)
    {

        ContactPoint contact = ObjetoTocado.contacts[0];

        GameObject hole = Instantiate(ReferenciaGlobal.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));


        hole.transform.SetParent(ObjetoTocado.gameObject.transform);


    }
}
