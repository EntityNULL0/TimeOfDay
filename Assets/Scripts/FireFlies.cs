using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class FireFlies : MonoBehaviour
{
    public float spawnRate = 7;
    
    void Update()
    {
        //Set float parameters for the firefly FX graph
        GetComponent<VisualEffect>().SetFloat("SpawnRate", spawnRate * TimeOfDay.nightDimmer);
        GetComponent<VisualEffect>().SetFloat("FadeOut", 1 - TimeOfDay.nightDimmer);
    }
}
