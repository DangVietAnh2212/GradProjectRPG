using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Camera cam;

    private void LateUpdate()
    {
        transform.forward = cam.transform.forward;
    }
}
