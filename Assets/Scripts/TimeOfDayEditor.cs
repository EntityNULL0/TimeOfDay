using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.VFX;
using UnityEngine.Rendering;

[CustomEditor(typeof(TimeOfDay))]
public class TimeOfDayEditor : Editor
{
    float timeToSet = 0;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TimeOfDay timeOfDay = (TimeOfDay)target;
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.fontSize = 18;
        headerStyle.normal.textColor = Color.white;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("References", headerStyle);

        EditorGUILayout.Space();

        EditorGUILayout.ObjectField("Sun Light", timeOfDay.sunLight, typeof(Light), true);
        EditorGUILayout.ObjectField("Moon Light", timeOfDay.moonLight, typeof(Light), true);
        EditorGUILayout.ObjectField("Sun Lens Flare", timeOfDay.sunLightFlare, typeof(LensFlare), true);
        EditorGUILayout.ObjectField("Cloud VFX", timeOfDay.cloudFX, typeof(VisualEffect), true);
        EditorGUILayout.ObjectField("Night PostFX Volume", timeOfDay.nightColorGrading, typeof(Volume), true);
        EditorGUILayout.ObjectField("Evening/Morning PostFX Volume", timeOfDay.eveningMorningColorGrading, typeof(Volume), true);        

        EditorGUILayout.Space(15);

        EditorGUILayout.LabelField("Sun/Moon Settings", headerStyle);

        EditorGUILayout.Space();

        timeOfDay.sunLightIntensity = EditorGUILayout.FloatField("Sun Light Intensity:", timeOfDay.sunLightIntensity);
        timeOfDay.moonLightIntensity = EditorGUILayout.FloatField("Moon Light Intensity:", timeOfDay.moonLightIntensity);
        timeOfDay.sunLightLensFlareIntensity = EditorGUILayout.FloatField("Sun Lens Flare Intensity:", timeOfDay.sunLightLensFlareIntensity);
        EditorGUILayout.LabelField("Season Angle Modifier:", GUILayout.Width(100), GUILayout.Height(15));
        timeOfDay.seasonAngle = EditorGUILayout.Slider(timeOfDay.seasonAngle, -45, 45f);

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Time Controls", headerStyle);

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUIStyle timeStyle = new GUIStyle();
        timeStyle.fontSize = 15;
        timeStyle.normal.textColor = Color.white;
        EditorGUILayout.LabelField("Current Time:", timeStyle, GUILayout.Width(120), GUILayout.Height(25));        
        EditorGUILayout.LabelField(timeOfDay.currentTime.ToString(), timeStyle, GUILayout.Width(120), GUILayout.Height(25));
        GUILayout.EndHorizontal();        

        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(timeOfDay.cycleTimeOfDay == true);
        if (GUILayout.Button("Start Cycle", GUILayout.Width(100), GUILayout.Height(30)))
        {
            timeOfDay.cycleTimeOfDay = true;
        }
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(timeOfDay.cycleTimeOfDay == false);
        if (GUILayout.Button("Stop Cycle", GUILayout.Width(100), GUILayout.Height(30)))
        {
            timeOfDay.cycleTimeOfDay = false;
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Time"))
        {
            timeOfDay.SetTimeOfDay(timeToSet);
        }
        timeToSet = EditorGUILayout.Slider(timeToSet, 0f, 2400f);        
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cycle Speed:", GUILayout.Width(80), GUILayout.Height(15));
        timeOfDay.timeScale = EditorGUILayout.Slider(timeOfDay.timeScale, 0.1f, 100f);
        GUILayout.EndHorizontal();
    }    
}
