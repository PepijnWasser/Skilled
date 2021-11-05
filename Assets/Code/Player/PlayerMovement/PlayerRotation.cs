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
        GameSettingsManager.settingsSaved += SetSensitivity;

        SetSensitivity();
    }

    private void OnDestroy()
    {
        GameSettingsManager.settingsSaved -= SetSensitivity;
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
        int savedSensitivity = PlayerPrefs.GetInt("sensitivity");
        Debug.Log(savedSensitivity);
        if (savedSensitivity != 0)
        {
            sensitivity = savedSensitivity;
        }
        else
        {
            sensitivity = PlayerInfo.sensitivity;
        }
    }
}
