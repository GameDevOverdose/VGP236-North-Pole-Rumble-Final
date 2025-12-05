using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileThrow : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform spawnTransform = null;
    [SerializeField] private float throwForce = 10.0f;
    [SerializeField] private float throwRate = 1.0f;
    private float throwTimer = 0.0f;

    private PlayerController playerController = null;

    private bool canThrow = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        PlayerInputGetter.Instance.ThrowAction.performed += Throw;

        throwTimer = throwRate;
    }

    private void Update()
    {
        if(throwTimer < throwRate)
        {
            throwTimer += Time.deltaTime;
        }
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if(!playerController.IsSliding && canThrow && throwTimer >= throwRate)
        {
            throwTimer = 0.0f;

            GameObject instantiatedProjectile = Instantiate(projectilePrefab, spawnTransform.position, spawnTransform.rotation);

            if (instantiatedProjectile.TryGetComponent<Rigidbody>(out Rigidbody projectileRigidbody))
            {
                projectileRigidbody.AddForce(spawnTransform.forward * throwForce, ForceMode.VelocityChange);
            }
        }
    }

    public void SetCanThrow(bool value)
    {
        canThrow = value;
    }
}
