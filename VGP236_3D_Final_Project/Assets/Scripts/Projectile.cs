using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float travelSpeed = 50.0f;
    [SerializeField] private float lifeTime = 5.0f;

    private void Awake()
    {
        StartCoroutine(DestroyProjectile());
    }

    private void Update()
    {
        transform.position += transform.forward * travelSpeed * Time.deltaTime;
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
