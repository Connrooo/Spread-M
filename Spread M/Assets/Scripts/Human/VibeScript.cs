using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibeScript : MonoBehaviour
{

    Camera mainCam;


    private void Awake()
    {
        mainCam= Camera.main;
    }


    private void Update()
    {
        transform.LookAt(mainCam.transform.position);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Debug.Log("Hello");
    }
}
