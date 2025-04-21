using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Activación")]
    public bool FollowingTarget = true;

    [Header("Disparo")]
    [Range(0.1f, 5f)]
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Detección")]
    public GameObject player;
    public float detectionRange = 5f;
    public LayerMask obstacleLayer;

    [Header("Sprite")]
    public SpriteRenderer turretSprite; // Asigna tu sprite renderer aquí

    private float fireCooldown = 0f;

    void Update()
    {
        if (!FollowingTarget || player == null) return;

        Vector2 direction = player.transform.position - transform.position;

        // Apunta la torreta
        RotateToFace(direction);

        if (CanSeePlayer(direction))
        {
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
            else
            {
                fireCooldown -= Time.deltaTime;
            }
        }
    }

    void RotateToFace(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip del sprite si pasa los 90 grados (mirando hacia la izquierda)
        if (turretSprite != null)
        {
            turretSprite.flipY = Mathf.Abs(angle) > 90f;
        }
    }

    bool CanSeePlayer(Vector2 direction)
    {
        float distance = direction.magnitude;
        if (distance > detectionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);
        return hit.collider == null;
    }

    void Shoot()
    {
        if (bulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null && player != null)
            {
                Vector2 direction = player.transform.position - firePoint.position;
                bulletScript.SetDirection(direction);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}