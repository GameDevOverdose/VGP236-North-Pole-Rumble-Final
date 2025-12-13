using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class HealthHeartBar : MonoBehaviour
{
    [SerializeField] GameObject _heartPrefab;
    [SerializeField] PlayerController _playerController;
    private PlayerHealth _playerHealthScript;

    private List<HealthHeart> _hearts = new List<HealthHeart>();

    void Start()
    {
        _playerHealthScript = _playerController.GetComponent<PlayerHealth>();

        DrawHearts();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDamaged -= DrawHearts;
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        _hearts = new List<HealthHeart>();
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(_heartPrefab); 
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        _hearts.Add(heartComponent);
    }

    public void DrawHearts()
    {
        ClearHearts();

        for (int i = 0; i < _playerHealthScript.MaxHealth; ++i)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < _playerHealthScript.CurrentHealth; ++i)
        {
            _hearts[i].SetHeartImage(HeartStatus.Full);
        }
    }

    private void UpdateUI()
    {
        DrawHearts();
    }
}
