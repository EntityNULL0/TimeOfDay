using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spire : MonoBehaviour
{
    public Material emissiveMaterial;   //Reference to the emissive shader we're going to control
    public Transform gem;      //Reference to the gem at the top of the spire
    public Light spireLight;    //Point light that's attached to the spire gem
    public float gemRotateSpeed = 10f;     //Gem will rotate this fast during the night time
    public float spireEmissiveIntensity = 10f;      //Controls the emissive multiplier of the emissive material
    public float spireLightIntensity = 12000f;      //Controls the intensity for the point light on the gem
    public float spireDimmer = 0f;      //Controls the 0-1 parameter on the emissive shader
    public Color spireColor = Color.blue;      //Color of the emissive material    
    
    void Update()
    {
        //Toggle the dimmer based on the night dimmer from the TimeOfDay script
        spireDimmer = TimeOfDay.nightDimmer;

        //Rotate the gem over time during the night
        gem.Rotate(Vector3.up, Time.deltaTime * gemRotateSpeed * spireDimmer); 
        
        //Set the parameters on the emissive shader
        emissiveMaterial.SetColor("_Color", spireColor);
        emissiveMaterial.SetFloat("_Intensity", spireEmissiveIntensity);
        emissiveMaterial.SetFloat("_Dimmer", spireDimmer);

        //Toggles the spire light on during the night
        spireLight.intensity = spireLightIntensity * spireDimmer;
    }
}
