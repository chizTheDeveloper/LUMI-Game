using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 velocity = Vector3.one;
    public Transform cameraT;
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        offset = cameraT.position - target.position;
    }

    void Awake(){
        cameraT = transform;
    }

    void LateUpdate()
    {
        Follow();
    }

    public void Follow(){
        Vector3 moveTo = target.position + (target.rotation * offset);
        Vector3 currentPos = Vector3.SmoothDamp(cameraT.position, moveTo, ref velocity, 0.3f);
        cameraT.position = currentPos;
        cameraT.LookAt(target, target.up);
    }
}
