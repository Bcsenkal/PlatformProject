using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void EnablePhysics()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.down;
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
    }
}
