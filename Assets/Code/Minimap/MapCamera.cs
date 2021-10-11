using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Camera camera;

    public float speed;

    Vector3 dragOrigin;

    Vector3 originalPos;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = new Vector3(camera.transform.position.x, 100, camera.transform.position.z);
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
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector3 direction = Vector3.zero;
        direction.z += Input.GetAxisRaw("Vertical");
        direction.x += Input.GetAxisRaw("Horizontal");

        camera.transform.position += difference;
        camera.transform.position += direction * Time.deltaTime * speed;
    }

    public void SetPosition(Vector3 newPos)
    {
        Debug.Log(this.transform);
    }
}
