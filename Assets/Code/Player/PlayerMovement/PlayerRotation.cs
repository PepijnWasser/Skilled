using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public GameObject cameraPos;

    public float sensitivity = 100f;
    float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        VideoSettingsManager.settingsSaved += SetSensitivity;
    }

    private void OnDestroy()
    {
        VideoSettingsManager.settingsSaved -= SetSensitivity;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPos.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up, mouseX);
    }

    void SetSensitivity()
    {
        sensitivity = PlayerPrefs.GetInt("sensitivity");
    }
}
