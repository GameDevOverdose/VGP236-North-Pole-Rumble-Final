using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] List<PlayerController> players = new List<PlayerController>();
    [SerializeField] List<TMP_Text> healthText = new List<TMP_Text>();

    //[SerializeField] List<Checkpoint> allCheckpoints;
    //[SerializeField] AudioSource hurtSound = null;

    private List<Vector3> lastCheckpointPosition = new List<Vector3>();

    static public float totalSeconds = 0.0f;
    static public bool timerStop = true;
    static public bool winGame = false;
    static private GameController instance = null;
    static public GameController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < players.Count; i++)
        {
            lastCheckpointPosition.Add(players[i].transform.position);
            players[i].Initialize();
            healthText[i].text = players[i].GetHealth;
        }
    }

    public void KillPlayer(int playerId)
    {
        //hurtSound.Play();
        players[playerId].RemoveHealth(1);
        healthText[playerId].text = players[playerId].GetHealth;
        players[playerId].ResetVelocity();
        players[playerId].transform.position = lastCheckpointPosition[playerId];
    }
}
