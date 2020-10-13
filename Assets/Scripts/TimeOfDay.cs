using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class TimeOfDay : MonoBehaviour
{    
    [HideInInspector]
    public Light sunLight, moonLight;   //Directional light references for sun and moon
    [HideInInspector]
    public LensFlare sunLightFlare;     //Lens flare used for sun
    [HideInInspector]
    public VisualEffect cloudFX;    //Reference to cloud effect placement
    [HideInInspector]
    public Volume nightColorGrading, eveningMorningColorGrading;   //Profile assets for evening and night color grading   
    
    [HideInInspector]
    public float sunLightIntensity = 30f, moonLightIntensity = 2f, sunLightLensFlareIntensity = 100f;   //Max intensities for directional lights and flares

    [HideInInspector]
    public float seasonAngle = 30f;   //Extra axis control for sun/moon to simulate longer or shorter days

    [HideInInspector]
    public bool cycleTimeOfDay = false;   //Use this to pause the time of day cycle

    [HideInInspector]
    public float currentTime = 1600f, timeScale = 1f;    //Current time of day in 24-hour clock system
        
    public static float nightDimmer = 0;    //Controller for night time effects
    public static float eveningMorningDimmer = 0;   //Controller for evening and morning color grading profiles 

    
    void Update()
    {
        //Advance the time of day cycle
        if (cycleTimeOfDay)
        {
            currentTime += Time.deltaTime * timeScale;
        }
        
        //Starts the day timer over at the end of 24 hours
        currentTime = Mathf.Repeat(currentTime, 2400);

        //Controls the angle of the sun by mapping the current time to a 360 degree cycle, with time offset to account for correcting initial rotation
        sunLight.transform.eulerAngles = Vector3.right * ((currentTime - 600) / 2400) * 360;

        //Apply the season modifier
        sunLight.transform.Rotate(Vector3.forward, seasonAngle, Space.World);

        //Setting the intensity of the sun/moon on update for editor purposes
        sunLight.intensity = sunLightIntensity;
        moonLight.intensity = moonLightIntensity;
        sunLightFlare.brightness = sunLightLensFlareIntensity;

        //Toggle directional lights based on whether sun or moon is above the horizon line so we're not trying to cast two directional shadows
        if (-sunLight.transform.forward.y > 0)
        {
            sunLight.enabled = true;
            moonLight.enabled = false;
        }
        else
        {
            sunLight.enabled = false;
            moonLight.enabled = true;
        }

        //Interpolates color grading for evening and night based on sun's height above the horizon line        
        if (-sunLight.transform.forward.y < 0)
        {
            if (nightDimmer <= 1){nightDimmer += Time.deltaTime * timeScale * 0.005f;}
            if (eveningMorningDimmer >= 0) { eveningMorningDimmer -= Time.deltaTime * timeScale * 0.005f; }
        }
        else if (-sunLight.transform.forward.y < 0.3f)
        {
            if (nightDimmer >= 0){nightDimmer -= Time.deltaTime * timeScale * 0.005f; }
            if (eveningMorningDimmer <= 1){eveningMorningDimmer += Time.deltaTime * timeScale * 0.005f; }
        }
        else
        {
            if (nightDimmer >= 0){nightDimmer -= Time.deltaTime * timeScale * 0.005f; }
            if (eveningMorningDimmer >= 0){eveningMorningDimmer -= Time.deltaTime * timeScale * 0.005f; }
        }
        
        //Set color grading weight based on interpolated values above        
        nightColorGrading.weight = nightDimmer;
        eveningMorningColorGrading.weight = eveningMorningDimmer;

        //This fades the opacity of the clouds so they don't look to bright at night
        cloudFX.SetFloat("CloudFadeOut", Mathf.Clamp(nightDimmer, 0f, 0.9f));
    }

    public void SetTimeOfDay(float t)
    {
        //Set time
        currentTime = t;

        //Reset sun angle
        sunLight.transform.eulerAngles = Vector3.right * ((currentTime - 600) / 2400) * 360;
        sunLight.transform.Rotate(Vector3.forward, seasonAngle, Space.World);
        
        //Immediately switches color grading profiles for evening and night time when new time is set
        if (-sunLight.transform.forward.y < 0)
        {
            nightDimmer = 1;
            eveningMorningDimmer = 0;
        }
        else if (-sunLight.transform.forward.y < 0.3f)
        {
            nightDimmer = 0;
            eveningMorningDimmer = 1;
        }
        else
        {
            nightDimmer = 0;
            eveningMorningDimmer = 0;
        }
        
        nightColorGrading.weight = nightDimmer;
        eveningMorningColorGrading.weight = eveningMorningDimmer;
    }    
}
