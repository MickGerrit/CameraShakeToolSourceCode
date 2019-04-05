using UnityEngine;
using UnityEditor;
using System.IO;
[System.Serializable]
[CustomEditor(typeof(CameraShake))]
public class CameraShakeInspectorWindow : Editor {
    public string json;
    private ShakeSaveData shakeSave;

    public override void OnInspectorGUI() {
        CameraShake cameraShake = (CameraShake)target;

        //the user can choose out of their camera shake presets right here:
        GUILayout.Label("Choose a camera shake preset:");
        EditorGUILayout.BeginHorizontal();
        cameraShake.jsonPreset = EditorGUILayout.ObjectField(cameraShake.jsonPreset, typeof(TextAsset), true);
        EditorGUILayout.EndHorizontal();
        if (cameraShake.jsonPreset == null) EditorGUILayout.HelpBox("Select your camera shake preset and drop this Camera Shake component into a public slot of your preferred script", MessageType.Info);
        

        //Load and assign the variables for the camera shake from the json
        if (cameraShake.jsonPreset != null && !cameraShake.previewShake) {
            json = File.ReadAllText(AssetDatabase.GetAssetPath(cameraShake.jsonPreset));
            shakeSave = JsonUtility.FromJson<ShakeSaveData>(json);
            cameraShake.duration = shakeSave.duration;
            cameraShake.magnitude = shakeSave.magnitude;
            cameraShake.interpolationSpeed = shakeSave.interpolationSpeed;
            cameraShake.roughness = shakeSave.roughness;
            cameraShake.rotationMagnitude = shakeSave.rotationMagnitude;
            cameraShake.originalPosition = shakeSave.originalPosition;
            cameraShake.originalRotation = shakeSave.originalRotation;
        }

    }
    
}
