using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightMatrix : MonoBehaviour
{
    public Camera _lightCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var renderer = GetComponent<Renderer>();
        bool d3d = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;
        // Matrix4x4 M = _lightCamera.transform.localToWorldMatrix;
        Matrix4x4 V = _lightCamera.worldToCameraMatrix;
        Matrix4x4 P = _lightCamera.projectionMatrix;

        // De donde sale esto? 
        // http://www.opengl-tutorial.org/es/intermediate-tutorials/tutorial-16-shadow-mapping/
        var bias = new Matrix4x4() {
            m00 = 0.5f, m01 = 0,    m02 = 0,    m03 = 0.5f,
            m10 = 0,    m11 = 0.5f, m12 = 0,    m13 = 0.5f,
            m20 = 0,    m21 = 0,    m22 = 0.5f, m23 = 0.5f,
            m30 = 0,    m31 = 0,    m32 = 0,    m33 = 1,
        };
        Matrix4x4 MVP = bias * P*V;
        GetComponent<Renderer>().material.SetMatrix("_lightProjectionMatrix", MVP);
    }
}
