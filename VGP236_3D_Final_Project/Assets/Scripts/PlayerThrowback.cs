using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowback : MonoBehaviour
{
    private Rigidbody rigidbody = null;
    [SerializeField] private float projectileThrowBackForce = 5.0f;
    [SerializeField] private float meleeThrowBackForce = 10.0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Vector3 direction = collision.transform.forward + (Vector3.up * 1.0f);
            direction.Normalize();

            rigidbody.AddForce(direction * projectileThrowBackForce, ForceMode.VelocityChange);

            Destroy(collision.gameObject);
        }
    }

    public void MeleeThrowback(Vector3 forwardDirection)
    {
        Vector3 direction = forwardDirection + (Vector3.up * 1.0f);
        direction.Normalize();

        rigidbody.AddForce(direction * meleeThrowBackForce, ForceMode.VelocityChange);
    }    
}
