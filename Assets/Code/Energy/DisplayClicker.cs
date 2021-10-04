using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayClicker : MonoBehaviour
{
    public Camera displayCam;
    public Camera energyMapCamera;

    public RectTransform clickWindow;
    Vector2 xBorder;
    Vector2 yBorder;

    Vector2 sizeDif;

    public LayerMask energyMask;

    private void Awake()
    {
        GameManager.playerMade += SetPlayerCamera;
    }

    private void OnDestroy()
    {
        GameManager.playerMade -= SetPlayerCamera;
    }

    void Start()
    {
        Vector3[] worldCorners = new Vector3[4];
        clickWindow.GetWorldCorners(worldCorners);

        xBorder.x = worldCorners[0].x;
        xBorder.y = worldCorners[2].x;
        yBorder.x = worldCorners[0].y;
        yBorder.y = worldCorners[2].y;

        sizeDif.x = xBorder.y - xBorder.x;
        sizeDif.y = yBorder.y - yBorder.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseToDisplayRay = displayCam.ScreenPointToRay(Input.mousePosition);

            RaycastHit displayHit;

            if (Physics.Raycast(mouseToDisplayRay, out displayHit, 10))
            {
                if (displayHit.point.x > xBorder.x && displayHit.point.x < xBorder.y && displayHit.point.y > yBorder.x && displayHit.point.y < yBorder.y)
                {
                    float widthPercentage = ((displayHit.point.x - xBorder.x) / sizeDif.x);
                    float heightPercentage = ((displayHit.point.y - yBorder.x) / sizeDif.y);
                    Ray minimapCameraToEnergyUserRay = energyMapCamera.ScreenPointToRay(new Vector3(energyMapCamera.pixelWidth * widthPercentage, energyMapCamera.pixelHeight * heightPercentage, 0));

                    RaycastHit energyUserHit;

                    if (Physics.Raycast(minimapCameraToEnergyUserRay, out energyUserHit, 100, energyMask))
                    {
                        Debug.Log(energyUserHit.transform.gameObject);
                        energyUserHit.transform.parent.GetComponent<EnergyUser>().switchEnergy();
                    }
                }
            }
        }
    }

    void SetPlayerCamera(GameObject _player, Camera cam)
    {
        displayCam = cam;
    }
}