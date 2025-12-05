using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    private List<PlayerThrowback> throwbackScripts = new List<PlayerThrowback>();

    [SerializeField] private float attackRate = 1.0f;
    private float attackTimer = 0.0f;

    private void Awake()
    {
        PlayerInputGetter.Instance.MeleeAttack.performed += Attack;

        attackTimer = attackRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerThrowback>(out PlayerThrowback throwback))
        {
            throwbackScripts.Add(throwback);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerThrowback>(out PlayerThrowback throwback))
        {
            throwbackScripts.Remove(throwback);
        }
    }

    private void Update()
    {
        if(attackTimer < attackRate)
        {
            attackTimer += Time.deltaTime;
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(attackTimer >= attackRate)
        {
            attackTimer = 0.0f;

            for (int i = 0; i < throwbackScripts.Count; i++)
            {
                throwbackScripts[i].MeleeThrowback(transform.forward);
            }
        }
    }
}
