using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Animator _animator;
    
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        _animator.enabled = true;
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        Debug.Log("rigidbodies disabled");
    }

    public void ActivateRagdoll()
    {
        _animator.enabled = false;
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
