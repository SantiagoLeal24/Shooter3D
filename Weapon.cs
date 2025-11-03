using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    

    //disparar
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletPerBurst = 3;
    public int BurstBulletsLeft;

    //Spread

    public float spreadIntensity;

    //Bala
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    private Animator animator;

    //Cargar Arma
    public float reloadTime;
    public int Cargador, balasRestantes;
    public bool estaRecargando;

   

    public enum SHootingMode
    {
        Single,
        Burst,
        Auto
    }
    public SHootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        BurstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();

        balasRestantes = Cargador;
    }


    void Update()
    {
       if(currentShootingMode == SHootingMode.Auto)
        {
            // Holidng Down Left Mouse Button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }

       else if (currentShootingMode == SHootingMode.Single || currentShootingMode == SHootingMode.Burst)
        {
            //Click Left Mouse Button
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        }

       //Recargar automaticamente sin balas
       if (Input.GetKeyDown(KeyCode.R) && balasRestantes < Cargador && estaRecargando == false)
        {
            Recargar();
        }

       //if (readyToShoot && isShooting == false && estaRecargando == false)
       // {
       //     Recargar();
       // }

       if (balasRestantes <= 0 && estaRecargando == false)
        {
            Recargar();
        }

        if (readyToShoot && isShooting && balasRestantes > 0)
        {
            BurstBulletsLeft = bulletPerBurst;

            FireWeapon();
        }

        if (AmmoManager.Instance.balasDisplay != null)
        {
            AmmoManager.Instance.balasDisplay.text = $"{balasRestantes / bulletPerBurst}/{Cargador / bulletPerBurst}";
        }


        }

    private void FireWeapon()
    {
        balasRestantes--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        animator.SetTrigger("RECOIL");

        SoundManager.Instance.shootingSound.Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //Poitng the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //Disparar
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Destruir la bala

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //Chequear si disparamos
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);

            allowReset = false;

               
        }

        // Burst Mode
        if (currentShootingMode == SHootingMode.Burst && BurstBulletsLeft > 1 && balasRestantes > 0) //disparamos mas de una vez


        {
            BurstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void Recargar()
    {
        if (!estaRecargando)
        {
            StartCoroutine(ReloadCoroutine());
        }

    }

    private IEnumerator ReloadCoroutine()
    {
        estaRecargando = true;

        yield return new WaitForSeconds(reloadTime);

        balasRestantes = Cargador;
        estaRecargando = false;
    }

   

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        //Disparar desde el medio de la pantalla
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);

        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //Returning the direciton and spread
        return direction + new Vector3(x, y, 0);


    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
