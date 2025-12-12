using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.SetActive(false);
            int id = collision.gameObject.GetComponent<PlayerController>().GetId;
            GameController.Instance.KillPlayer(id);
        }
    }
}