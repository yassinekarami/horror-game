using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class SplineDollyCamera : MonoBehaviour
{

    public List<GameObject> pointOfInterests;
    public bool follow = true;
    CinemachineSplineDolly splineDolly;
    CinemachineCamera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineDolly = new CinemachineSplineDolly();
        cam = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            cam.Follow = pointOfInterests[0].transform;
        }
          
        else
        {
            cam.Follow = pointOfInterests[1].transform;
        }
    }
}
