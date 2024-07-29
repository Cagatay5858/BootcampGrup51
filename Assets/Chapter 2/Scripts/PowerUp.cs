using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCar"))
        {
            Debug.Log("Power-up collected by player car.");
            BumperCarController playerCar = other.GetComponent<BumperCarController>();
            if (playerCar != null)
            {
                FireProjectileAtNearestEnemy(playerCar);
            }
            Destroy(gameObject);
        }
    }

    void FireProjectileAtNearestEnemy(BumperCarController playerCar)
    {
        GameObject nearestEnemy = playerCar.FindNearestEnemy();
        if (nearestEnemy != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, playerCar.transform.position + playerCar.transform.forward, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                projectileController.SetTarget(nearestEnemy); 
                Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
                if (projectileRb != null)
                {
                    Vector3 direction = (nearestEnemy.transform.position - playerCar.transform.position).normalized;
                    projectileRb.velocity = direction * projectileSpeed; 
                }
            }
        }
    }
}