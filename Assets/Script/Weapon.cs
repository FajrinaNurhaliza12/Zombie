using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    // ===== TAMBAHAN SFX =====
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [Header("Weapon Settings")]
    [SerializeField] private float fireRate = 0.15f;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int reserveAmmo = 90;

    private int currentAmmo;
    private float nextFireTime;

    // Status tombol Attack
    private bool isFireButtonHeld = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        HandleShoot();
        HandleReload();
    }

    // Dipanggil saat tombol Attack ditekan
    public void FireButtonDown()
    {
        isFireButtonHeld = true;
    }

    // Dipanggil saat tombol Attack dilepas
    public void FireButtonUp()
    {
        isFireButtonHeld = false;
    }

    private void HandleShoot()
    {
        // Pause
        if (Time.timeScale == 0f)
            return;

        // Menembak hanya jika tombol Attack sedang ditekan
        if (!isFireButtonHeld)
            return;

        // Fire Rate
        if (Time.time < nextFireTime)
            return;

        // Cek peluru
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

        // ===== MAINKAN SUARA TEMBAKAN =====
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

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