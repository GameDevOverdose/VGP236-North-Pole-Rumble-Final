using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationSpeed = new Vector3(0.0f, 100.0f, 0.0f);

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed.y * Time.deltaTime, Space.World);
    }
}
