using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] private Transform _playersHealthUIParent = null;
    public List<GameObject> _playersHealthUI = new List<GameObject>();

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        foreach (Transform child in _playersHealthUIParent)
        {
            _playersHealthUI.Add(child.gameObject);
        }

        for (int i = 0; i < _playersHealthUI.Count; i++)
        {
            if (i < PlayerManager.Instance.PlayersSupported)
            {
                _playersHealthUI[i].SetActive(true);
            }
            else
            {
                _playersHealthUI[i].SetActive(false);
            }
        }
    }

    public int GetPlayerHealthUIIndex(GameObject healthUIObject)
    {
        for (int i = 0; i < _playersHealthUI.Count; i++)
        {
            if (_playersHealthUI[i] == healthUIObject)
            {
                return i;
            }
        }

        Debug.LogError($"Health UI {healthUIObject} not found in assigned health UI elements.");
        return default;
    }

    public void UpdateUI()
    {
        UpdateHealthUIElements();
    }

    public void UpdateHealthUIElements()
    {
        for (int i = 0; i < _playersHealthUI.Count; i++)
        {
            if (_playersHealthUI[i].activeSelf)
            {
                PlayerHealthUI healthUIComponent = _playersHealthUI[i].GetComponent<PlayerHealthUI>();
                healthUIComponent.UpdateHealthUI();
            }
        }
    }
}
