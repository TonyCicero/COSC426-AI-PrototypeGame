using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class god_cam : MonoBehaviour
{
    
    [SerializeField]
    float move_speed;
    [SerializeField]
    float rot_speed;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0); //keep z rotation at 0
        if (Input.GetKey("a"))
        {
            transform.localPosition += Vector3.forward * Time.deltaTime * move_speed;
        }

        if (Input.GetKey("s"))
        {
            transform.localPosition -= Vector3.right * Time.deltaTime * move_speed;
        }

        if (Input.GetKey("d"))
        {
            transform.localPosition -= Vector3.forward * Time.deltaTime * move_speed;
        }
        if (Input.GetKey("w"))
        {
            transform.localPosition += Vector3.right * Time.deltaTime * move_speed;
        }

        if (Input.GetKey(KeyCode.Space)) // move up
        {
            transform.position += Vector3.up * Time.deltaTime * move_speed;
        }
        if (Input.GetKey(KeyCode.LeftControl)) // move down
        {
            transform.position -= Vector3.up * Time.deltaTime * move_speed;
        }

        if (Input.GetKey(KeyCode.UpArrow)) // rot up
        {
            transform.Rotate( Time.deltaTime*rot_speed, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.DownArrow)) // rot down
        {
            transform.Rotate(- Time.deltaTime * rot_speed, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) // rot left
        {
            transform.Rotate(0 , - (Time.deltaTime * rot_speed), 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.RightArrow)) // rot right
        {
            transform.Rotate(0,  (Time.deltaTime * rot_speed), 0, Space.Self);
        }



    }
}
