using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Camera mapCam;

    public float speed;

    Vector3 dragOrigin;

    Vector3 originalPos;

    public float zoomSensitivity;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(mapCam.transform.position.x, 100, mapCam.transform.position.z);
        originalPos = transform.position;
    }

    private void OnDestroy()
    {
        
    }

    public void PanCamera()
    {
        Vector3 difference = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = mapCam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            difference = dragOrigin - mapCam.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector3 direction = Vector3.zero;
        direction.z += Input.GetAxisRaw("Vertical");
        direction.x += Input.GetAxisRaw("Horizontal");

        mapCam.transform.position += difference;
        mapCam.transform.position += direction * Time.deltaTime * speed;
    }

    public void ZoomCamera()
    {
        float camZoom = mapCam.orthographicSize;

        if(Input.mouseScrollDelta.y < 0)
        {
            camZoom += zoomSensitivity * Time.deltaTime;
        }
        else if(Input.mouseScrollDelta.y > 0)
        {
            camZoom -= zoomSensitivity * Time.deltaTime;
        }
        camZoom = Mathf.Clamp(camZoom, 10, 100);
        mapCam.orthographicSize = camZoom;
    }

    public void SetPosition(Vector3 newPos)
    {
        Debug.Log(this.transform);
    }
}
