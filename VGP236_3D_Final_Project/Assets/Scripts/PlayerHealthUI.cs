using System;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _healthText;
    private int uIIndex = 0;

    private void Start()
    {
        _healthText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        uIIndex = UIManager.Instance.GetPlayerHealthUIIndex(gameObject);
        _healthText.text = $"Player {uIIndex + 1}: {PlayerManager.Instance._playersInScene[uIIndex].GetComponent<PlayerHealth>().CurrentHealth}";
    }
}
