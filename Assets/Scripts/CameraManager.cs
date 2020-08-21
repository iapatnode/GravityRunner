using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera cam;
    public PlayerScript player;
    Vector3 offset = new Vector3(3, 0, -10);
    public Vector3 midpoint;
    public static bool zoomed = true;
    float camDefaultZoom;
    public float zoomedOut;
    float zoomTime;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        camDefaultZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            zoomOut();
        }
        if (Input.GetKeyUp(KeyCode.C)) {
            zoomIn();
        }
    }

    private void LateUpdate()
    {
        //Moves camera based on player position
        if (player && zoomed)
        {
            transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, offset.z);
        }
        else {
            transform.position = midpoint;
        }
    }

    private void zoomOut()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomedOut, 2f);
        zoomed = false;
    }
    private void zoomIn() {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camDefaultZoom, 2f);
        zoomed = true;
    }

    public bool zoomVal() {
        return zoomed;
    }
}
