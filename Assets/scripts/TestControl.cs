using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            transform.Rotate(transform.rotation.eulerAngles + new Vector3(0f, 0.1f, 0f));
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        transform.position += new Vector3(horizontal * 0.1f, vertical * 0.1f, 0f);

        string[] joystickNames = Input.GetJoystickNames();
        Debug.Log("Joysticks detected: " + string.Join(", ", joystickNames));
    }
}
