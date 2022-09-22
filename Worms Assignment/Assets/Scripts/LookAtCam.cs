using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    [SerializeField] Transform cam;
    // Update is called once per frame
    private void Awake() {
        cam = GameObject.Find("Main Camera").transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
