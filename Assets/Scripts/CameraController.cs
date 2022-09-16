using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 OffsetFromCam = new Vector3(0.7f, -1.4f, 0.5f);
    private float currentZoom = 10f;
    public float targetHeight = 2f;
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20;

    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }
    private void LateUpdate()
    {
        transform.position = target.position - OffsetFromCam * currentZoom;
        transform.LookAt(target.position + Vector3.up * targetHeight);
    }
}
