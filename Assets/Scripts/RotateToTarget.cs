using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Start()
    {
        transform.rotation = target.rotation;
    }

    void Update()
    {
        transform.rotation = target.rotation;

    }
}
