using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    private List<PlayerKnockback> _knockbackScripts = new List<PlayerKnockback>();

    [Header("Attack Settings")]
    [SerializeField] private float _attackRate = 1.0f;
    private float _attackTimer = 0.0f;

    [Header("Knockback Settings")]
    [SerializeField] private float _knockbackForce = 10.0f;
    [SerializeField] private float _knockbackIncrement = 0.25f;

    private int playerNumber = 0;

    private void Awake()
    {
        playerNumber = PlayerNumberAssigner.Instance.GetPlayerNumberIndex(transform.parent.gameObject);
        PlayerInputGetter.Instance.MeleeAttack(playerNumber).performed += Attack;

        _attackTimer = _attackRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerKnockback>(out PlayerKnockback throwback))
        {
            _knockbackScripts.Add(throwback);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerKnockback>(out PlayerKnockback throwback))
        {
            _knockbackScripts.Remove(throwback);
        }
    }

    private void Update()
    {
        if(_attackTimer < _attackRate)
        {
            _attackTimer += Time.deltaTime;
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(_attackTimer >= _attackRate)
        {
            _attackTimer = 0.0f;

            for (int i = 0; i < _knockbackScripts.Count; i++)
            {
                _knockbackScripts[i].Knockback(transform.forward, _knockbackForce, _knockbackIncrement);
            }
        }
    }
}
