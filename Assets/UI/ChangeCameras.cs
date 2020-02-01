using UnityEngine;
using System.Collections;

public class ChangeCameras : MonoBehaviour
{

    [SerializeField] private GameObject[] cameras = new GameObject[5];

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("1") && cameras[0] != null)
        {
            switchCameras(0);
        }
        else if (Input.GetKeyDown("2") && cameras[1] != null)
        {
            switchCameras(1);
        }
        else if (Input.GetKeyDown("3") && cameras[2] != null)
        {
            switchCameras(2);
        }
        else if (Input.GetKeyDown("4") && cameras[3] != null)
        {
            switchCameras(3);
        }
        else if (Input.GetKeyDown("5") && cameras[4] != null)
        {
            switchCameras(4);
        }
    }

    private void switchCameras(int keyNum)
    {
        for (int i = 0; i < cameras.Length ; i++)
        {
            if (cameras[i] != null && keyNum != i)
            {
                // turn camera off
                cameras[i].GetComponent<Camera>().enabled = false;
            }
            else
            {
                // turn camera on
                cameras[i].GetComponent<Camera>().enabled = true;
            }
        }
    }
}