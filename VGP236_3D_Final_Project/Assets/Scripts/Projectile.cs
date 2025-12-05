using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _travelSpeed = 50.0f;
    [SerializeField] private float _lifeTime = 5.0f;

    private void Awake()
    {
        StartCoroutine(DestroyProjectile());
    }

    private void Update()
    {
        transform.position += transform.forward * _travelSpeed * Time.deltaTime;
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
