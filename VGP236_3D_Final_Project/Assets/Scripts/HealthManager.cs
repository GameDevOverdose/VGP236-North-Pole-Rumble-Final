using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthManager : MonoBehaviour
{
    static public float totalSeconds = 0.0f;
    static public bool timerStop = true;
    static public bool winGame = false;
    static private HealthManager instance = null;
    static public HealthManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            //Application.Quit();
            Debug.Log("Escape key pressed - Pause Menu");
        }
    }

    public void KillPlayer(int playerNumberIndex)
    {
        GameObject playerToKill = PlayerManager.Instance._playersInScene[playerNumberIndex];

        if (!GameStateManager.Instance.GameWon)
        {
            playerToKill.GetComponent<PlayerHealth>().IncrementHealth(-1);
            UIManager.Instance.UpdateHealthUIElements();
        }

        if (PlayerManager.Instance._playersInScene[playerNumberIndex].GetComponent<PlayerHealth>().IsAlive)
        {
            playerToKill.GetComponent<PlayerController>().ResetPlayerState();
            playerToKill.GetComponent<PlayerSpawn>().Spawn();
        }
        else
        {
            playerToKill.SetActive(false);
        }
    }
}
