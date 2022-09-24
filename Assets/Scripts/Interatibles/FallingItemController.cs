using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingItemController : MonoBehaviour
{
    Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            rigidBody.isKinematic = true;
    }
}
