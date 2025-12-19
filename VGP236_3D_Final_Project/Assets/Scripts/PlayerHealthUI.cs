using System;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _healthText;
    private int uIIndex = 0;

    private void Start()
    {
        if(gameObject.TryGetComponent<TMPro.TextMeshProUGUI>(out var tmp))
        {
            _healthText = tmp;
        }
        else
        {
            _healthText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        UpdateHealthUI();
    }

    private void OnEnable()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        uIIndex = UIManager.Instance.GetPlayerHealthUIIndex(gameObject);
        
        if(uIIndex < 0 || uIIndex >= PlayerManager.Instance._activatedPlayers.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        _healthText.text = $"Player {uIIndex + 1}: {PlayerManager.Instance._activatedPlayers[uIIndex].GetComponent<PlayerHealth>().CurrentHealth}";
    }
}
