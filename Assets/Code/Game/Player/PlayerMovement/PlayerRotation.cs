using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public GameObject cameraPos;
    public GameObject lookTargetAxis;

    public float sensitivity = 100f;

    public Animator animator;

    float yRotation = 0f;
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

    void LateUpdate()
    {       
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);


        xRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -70F, 70F);

        lookTargetAxis.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
        

        if(animator.GetInteger("MovementPlayer") == 0)
        {
            if (lookTargetAxis.transform.localRotation.eulerAngles.y > 40 && lookTargetAxis.transform.localRotation.eulerAngles.y < 320)
            {
                if (lookTargetAxis.transform.localRotation.eulerAngles.y <= 160)
                {
                    animator.SetTrigger("PlayerTurnLeft");
                }
                else
                {
                    animator.SetTrigger("PlayerTurnRight");
                }
                transform.rotation = Quaternion.Euler(0, lookTargetAxis.transform.rotation.eulerAngles.y, 0);
                lookTargetAxis.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

                xRotation = 0F;
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, lookTargetAxis.transform.rotation.eulerAngles.y, 0);
            lookTargetAxis.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

            xRotation = 0F;
        }
    }

    void SetSensitivity()
    {
        int savedSensitivity = PlayerPrefs.GetInt("sensitivity");
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
