using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrowingSnowball : MonoBehaviour
{
    [SerializeField] private GameObject snowballPrefab = null;
    private GameObject snowballInstance = null;

    private Vector3 startScale = Vector3.zero;
    [SerializeField] Vector3 endScale = Vector3.one;

    [SerializeField] private float maxGrowSpeed = 1.0f;

    private Rigidbody playerRigidbody = null;
    private float playerMaxSpeed = 0.0f;
    private ProjectileThrow projectileThrowScript = null;

    //Can use this in case implementing a cooldown/powerup
    private bool canGrowSnowball = true;

    private void Awake()
    {
        playerRigidbody = transform.parent.GetComponent<Rigidbody>();
        playerMaxSpeed = transform.parent.GetComponent<PlayerController>().MoveSpeed;
        projectileThrowScript = transform.parent.GetComponent<ProjectileThrow>();

        PlayerInputGetter.Instance.GrowSnowballAction.performed += GrowSnowball;
    }

    private void Update()
    {
        if (snowballInstance == null)
        {
            projectileThrowScript.SetCanThrow(true);
            return;
        }

        projectileThrowScript.SetCanThrow(false);

        Vector3 growth = Vector3.one * maxGrowSpeed * GrowthFactor() * Time.deltaTime;
        Vector3 newScale = Vector3.Min(snowballInstance.transform.localScale + growth, endScale);

        Vector3 deltaScale = newScale - snowballInstance.transform.localScale;

        snowballInstance.transform.position += snowballInstance.transform.forward * (deltaScale.z / 2f);

        snowballInstance.transform.localScale = newScale;
    }

    private float GrowthFactor()
    {
        float playerCurrentSpeed = playerRigidbody.linearVelocity.magnitude;

        return Mathf.Clamp(playerCurrentSpeed / playerMaxSpeed, 0.0f, 1.0f);
    }

    private void GrowSnowball(InputAction.CallbackContext context)
    {
        if (snowballInstance == null && canGrowSnowball)
        {
            InstantiateSnowball();
        }
        else
        {
            //JUST DESTROY FOR TESTING PURPOSES
            Destroy(snowballInstance);
        }
    }

    private void InstantiateSnowball()
    {
        snowballInstance = Instantiate(snowballPrefab, this.transform);
        snowballInstance.transform.position = transform.position;
        snowballInstance.transform.localScale = startScale;
    }
}
