using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectsPlacement : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    private ARRaycastManager arRaycastManager;
    private ARAnchorManager arAnchorManager;

    bool isManagerScanningPlanes;
    bool isAnchorCreated;
    bool isPlaneFounded;


    [SerializeField] private GameObject prefabToPlace;
    [SerializeField] private Vector3 placementOffset;

    public Text scan;



    private void Awake()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arAnchorManager = FindObjectOfType<ARAnchorManager>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();

        isManagerScanningPlanes = false;
        arPlaneManager.enabled = false;

        //trackMarker.SetActive(false);
        scan.text = "Відскануйте стіл";
    }

    private void Update()
    {
        ReceiveInput();
        CheckForScanningStatus();
    }

    private void ReceiveInput()
    {
        if (Input.touchCount != 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    if (isManagerScanningPlanes == false)
                    {
                        DebugConsole.instance.Log("Start plane detection");
                        StartPlaneDetection();
                    }
                    else
                    {
                        if (isPlaneFounded == true)
                        {
                            DebugConsole.instance.Log("Object spawned");
                            PlaceObject(touch.position);
                        }
                    }
                }
            }
        }
    }
    
    private void StartPlaneDetection() {
        DebugConsole.instance.Log("Manager scanning planes");
        isManagerScanningPlanes = true;
        arPlaneManager.enabled = true;
    }

    private void CheckForScanningStatus()
    {
        if (isManagerScanningPlanes == true)
        {
            if (arPlaneManager.trackables.count > 0)
            {
                if (isPlaneFounded == false)
                {
                    isPlaneFounded = true;
                }
                scan.text = "Куди кинути куб?";
            }
            else 
            {
                if (isPlaneFounded == true)
                {
                    isPlaneFounded = false;
                }
                scan.text = "Сканую...";
            }
        }
    }

    private void PlaceObject(Vector2 touchPosition)
    {

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes);

        if (hits.Count != 0)
        {
            if (isAnchorCreated == false)
            {
                SetAnchorPoint(hits[0].pose);
            }
            Vector3 placementPosition = hits[0].pose.position + placementOffset;
            Instantiate(prefabToPlace, placementPosition, Quaternion.identity);
        }
    }

    private void SetAnchorPoint(Pose pose)
    {
        DebugConsole.instance.Log("Anchor was created at " + pose.position.ToString());
        isAnchorCreated = true;
        arAnchorManager.AddAnchor(pose);
    }
}
