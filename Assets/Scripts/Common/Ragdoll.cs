using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Animator _animator;
    public bool isPlayer;
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        if (!isPlayer)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        if (!isPlayer)
        {
            DeactivateRagdoll();
        }
        else
        {
            {
                ActivateRagdoll();
            }
        }
    }

    public void DeactivateRagdoll()
    {
        if (!isPlayer)
        {
            _animator.enabled = true;
        }
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        Debug.Log("rigidbodies disabled");
    }

    public void ActivateRagdoll()
    {
        if (!isPlayer)
        {
            _animator.enabled = false;
        }
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        var rigidbody = _animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidbody.AddForce(force, ForceMode.VelocityChange);
    }
}
