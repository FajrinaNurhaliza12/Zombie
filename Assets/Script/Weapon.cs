using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Weapon Settings")]
    [SerializeField] private float fireRate = 0.15f;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int reserveAmmo = 90;

    private int currentAmmo;
    private float nextFireTime;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        HandleShoot();
        HandleReload();
    }

    private void HandleShoot()
    {
        // ===== FIX TAMBAHAN (PAUSE CHECK) =====
        if (Time.timeScale == 0f)
            return;

        // ===== FIX TAMBAHAN (BLOCK KLIK UI) =====
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (!Mouse.current.leftButton.isPressed)
            return;

        if (Time.time < nextFireTime)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log("Peluru Habis!");
            return;
        }

        nextFireTime = Time.time + fireRate;

        currentAmmo--;

        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Debug.Log("Tembak | Sisa Peluru : " + currentAmmo);
    }

    private void HandleReload()
    {
        if (!Keyboard.current.rKey.wasPressedThisFrame)
            return;

        if (currentAmmo == maxAmmo)
            return;

        if (reserveAmmo <= 0)
            return;

        int need = maxAmmo - currentAmmo;
        int reloadAmount = Mathf.Min(need, reserveAmmo);

        currentAmmo += reloadAmount;
        reserveAmmo -= reloadAmount;

        Debug.Log("Reload");
    }
}