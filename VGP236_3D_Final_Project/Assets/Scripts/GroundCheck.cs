using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Animator _animator = null;

    private int _groundContacts = 0;
    public bool IsGrounded { get { return _groundContacts > 0; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            ++_groundContacts;
        }

        CheckGrounded();

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            --_groundContacts;
        }

        CheckGrounded();
    }

    void CheckGrounded()
    {
        if (IsGrounded)
        {
            _animator.SetBool("Grounded", true);
        }
        else
        {
            _animator.SetBool("Grounded", false);
        }   
    }
}