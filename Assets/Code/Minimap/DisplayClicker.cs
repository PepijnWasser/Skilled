using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayClicker : MonoBehaviour
{
    public Camera displayCam;
    public Camera energyMapCamera;

    public RectTransform clickWindow;

    Vector2 sizeDif;

    public LayerMask energyMask;

    Ray testRay;
    public GameObject emptyPrefab;
    public GameObject cubePrefab;

    private void Awake()
    {
        GameManager.playerMade += SetPlayerCamera;
    }

    private void OnDestroy()
    {
        GameManager.playerMade -= SetPlayerCamera;
    }


    // Update is called once per frame
    void Update()
    {
        if (testRay.direction != null)
        {
            Debug.DrawRay(displayCam.transform.position, testRay.direction * 5, Color.green);
        }
        if (Input.GetMouseButtonDown(0))
        {
          
            Ray mouseToDisplayRay = displayCam.ScreenPointToRay(Input.mousePosition);
            testRay = mouseToDisplayRay;

            RaycastHit displayHit;

            if (Physics.Raycast(mouseToDisplayRay, out displayHit, 10))
            {
                if(displayHit.collider.gameObject == this.gameObject)
                {
                    GameObject hit = Instantiate(emptyPrefab, displayHit.point, Quaternion.identity, this.transform);

                    Quaternion originalRotation = transform.rotation;

                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    Vector3[] worldCorners = new Vector3[4];
                    clickWindow.GetWorldCorners(worldCorners);

                    Vector2 bottomLeft;
                    Vector2 topRight;

                    bottomLeft.x = worldCorners[0].x;
                    bottomLeft.y = worldCorners[2].x;
                    topRight.x = worldCorners[0].y;
                    topRight.y = worldCorners[2].y;

                    sizeDif.x = bottomLeft.y - bottomLeft.x;
                    sizeDif.y = topRight.y - topRight.x;

                    float widthPercentage = ((hit.transform.position.x - bottomLeft.x) / sizeDif.x);
                    float heightPercentage = ((hit.transform.position.y - topRight.x) / sizeDif.y);

                    //Debug.Log(widthPercentage + "     +      " + heightPercentage);

                    transform.rotation = originalRotation;

                    Destroy(hit.gameObject);

                    Ray minimapCameraToEnergyUserRay = energyMapCamera.ScreenPointToRay(new Vector3(energyMapCamera.pixelWidth * widthPercentage, energyMapCamera.pixelHeight * heightPercentage, 0));

                    RaycastHit energyUserHit;

                    if (Physics.Raycast(minimapCameraToEnergyUserRay, out energyUserHit, 100, energyMask))
                    {
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