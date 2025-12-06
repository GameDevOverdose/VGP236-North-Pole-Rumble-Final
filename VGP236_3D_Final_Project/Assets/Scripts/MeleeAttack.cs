using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    private List<PlayerThrowback> _throwbackScripts = new List<PlayerThrowback>();

    [Header("Attack Settings")]
    [SerializeField] private float _attackRate = 1.0f;
    private float _attackTimer = 0.0f;

    [Header("Throwback Settings")]
    [SerializeField] private float _hitThrowbackForce = 10.0f;
    [SerializeField] private float _hitThrowbackIncrement = 0.25f;

    private void Awake()
    {
        PlayerInputGetter.Instance.MeleeAttack.performed += Attack;

        _attackTimer = _attackRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerThrowback>(out PlayerThrowback throwback))
        {
            _throwbackScripts.Add(throwback);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerThrowback>(out PlayerThrowback throwback))
        {
            _throwbackScripts.Remove(throwback);
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

            for (int i = 0; i < _throwbackScripts.Count; i++)
            {
                _throwbackScripts[i].Throwback(transform.forward, _hitThrowbackForce, _hitThrowbackIncrement);
            }
        }
    }
}
