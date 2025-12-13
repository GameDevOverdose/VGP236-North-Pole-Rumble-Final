using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController) != false)
        {
            int playerIndex = PlayerNumberAssigner.Instance.GetPlayerNumberIndex(other.gameObject);
            HealthManager.Instance.KillPlayer(playerIndex);
        }
    }
}