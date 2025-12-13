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
        GameObject playerToKill = PlayerNumberAssigner.Instance._playersInScene[playerNumberIndex];

        playerToKill.GetComponent<PlayerHealth>().IncrementHealth(-1);
        playerToKill.GetComponent<PlayerController>().ResetPlayerState();
        playerToKill.GetComponent<PlayerSpawn>().Spawn();
    }
}
