using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceShader : MonoBehaviour
{
    public Shader EffectShader;
    // Start is called before the first frame update
    void Awake() {
        GetComponent<Camera>().SetReplacementShader (EffectShader, "RenderType");
    }   

    // Update is called once per frame
    void Update()
    {
    }
}
