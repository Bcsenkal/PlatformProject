using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformPart : MonoBehaviour
{
    private Rigidbody rb;
    private PlatformColorization platformColorization;
    private bool isFalling;

    void Update()
    {
        if(!isFalling) return;
        if(transform.position.y < -10)
        {
            SendToPool();
        }
    }

    public void CacheComponents()
    {
        rb = GetComponent<Rigidbody>();
        platformColorization = GetComponent<PlatformColorization>();
        platformColorization.CacheComponents();
    }

    public void Activate(Color color, int direction)
    {
        platformColorization.SetColor(color);
        rb.isKinematic = false;
        rb.AddForce(new Vector3(direction,-1,0), ForceMode.Impulse);
        isFalling = true;
    }

    private void SendToPool()
    {
        isFalling = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Managers.EventManager.Instance.ONOnAddFallingPartToPool(this);
        gameObject.SetActive(false);
    }
}
