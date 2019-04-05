using UnityEngine;
using UnityEditor;
using System.IO;
[System.Serializable]
[CustomEditor(typeof(CameraShake))]
public class CameraShakeEditorWindow : EditorWindow {
    private static CameraShakeEditorWindow _winInstance;
    public Transform camTransform;
    private CameraShake cameraShake;
    private Vector3 originPos;
    private Vector3 originRot;
    private bool setOrigin;

    private string fileName = "";

    [MenuItem("Tools/Camera Shake Maker Window")]
    public static void ShowWindow() {
        _winInstance = GetWindow<CameraShakeEditorWindow>(true, "Camera Shake Maker Window");
        
    }

    //here all the editor window GUI stuff happens. I wanted the user to be able to preview the shake he/she made without messing with the json file that is already loaded in the camera
    //shake component that is sitting on the camera game object. So I used a simple bool to check if this window is opened
    public void OnGUI() {
        
        GUILayout.Label("Camera Shake Maker");

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Scene Selected Camera Transform");
        camTransform = EditorGUILayout.ObjectField(camTransform, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Select Camera Transform")) {
            if (Selection.activeGameObject != null) {
                camTransform = Selection.activeGameObject.GetComponent<Transform>();
            }
            //Put a new camera shake component on the preferred camera
            if (camTransform.gameObject.GetComponent<CameraShake>() == null && camTransform.gameObject.GetComponent<Camera>() != null) {
                cameraShake = camTransform.gameObject.AddComponent<CameraShake>() as CameraShake;
            } else if (camTransform != null&& camTransform.gameObject.GetComponent<CameraShake>() == null) {
                EditorGUIUtility.PingObject(FindObjectOfType<Camera>());
                camTransform = null;
            }
            if (camTransform != null) {

                cameraShake = camTransform.gameObject.GetComponent<CameraShake>();
                cameraShake.cameraTransform = camTransform;
            }
            if (camTransform.gameObject.GetComponent<Camera>() == null) {
                setOrigin = false;
                camTransform = null;
            }
            if (camTransform == null || Selection.activeGameObject == null) {
                EditorGUIUtility.PingObject(FindObjectOfType<Camera>());
            }

        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        if (camTransform == null) EditorGUILayout.HelpBox("No GameObject selected, the GameObject needs to have a Camera component! Please select a GameObject with a Camera to access the transform needed to preview the camera shake.", MessageType.Error);
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        
        if (camTransform != null) {

            cameraShake.previewShake = true;
            
            EditorGUILayout.HelpBox("This will set the current position & rotation of the camera transform as the origin position & rotation of the camera shakes.", MessageType.Info);
            if (GUILayout.Button("Set Position & Rotation")) {
                originPos = camTransform.position;
                originRot = camTransform.rotation.eulerAngles;
                cameraShake.originalPosition = originPos;
                cameraShake.originalRotation = camTransform.rotation;
                setOrigin = true;
            }
            
        }
        GUILayout.Space(20);
        if (setOrigin) {
            if (camTransform != null) {
                cameraShake.duration = EditorGUILayout.Slider("Duration", cameraShake.duration, 0f, 5f);
                cameraShake.magnitude = EditorGUILayout.Slider("Positional Magnitude", cameraShake.magnitude, 0f, 5f);
                cameraShake.rotationMagnitude = EditorGUILayout.Slider("Rotational Magnitude", cameraShake.rotationMagnitude, 0f, 20f);
                cameraShake.interpolationSpeed = EditorGUILayout.Slider("Interpolation Speed", cameraShake.interpolationSpeed, 0f, 100f);
                cameraShake.roughness = EditorGUILayout.Slider("Roughness", cameraShake.roughness, 0f, 0.5f);
                if (Application.isPlaying) {
                    if (GUILayout.Button("Preview Camera Shake")) {
                        cameraShake = camTransform.gameObject.GetComponent<CameraShake>();
                        cameraShake.Shake();
                    }
                } else EditorGUILayout.HelpBox("You can preview the camera shake while playing your scene, then a button will appear", MessageType.Warning);

                GUILayout.Space(20);

                //here all the file browser magic happens
                if (GUILayout.Button("Save Camera Shake Preset To... ")) {
                    string path = EditorUtility.SaveFilePanelInProject("Save camera shake preset settings", "NewCameraShakePreset", "json",
            "Please enter a file name to save the preset to");
                    cameraShake = camTransform.gameObject.GetComponent<CameraShake>();
                    ShakeSaveData shakeSave = CreateShakeSave();
                    string json = JsonUtility.ToJson(shakeSave);
                    File.WriteAllText(path, json);
                    camTransform.GetComponent<CameraShake>().jsonPreset = (Object)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                }
            }
            EditorGUILayout.HelpBox("It takes some seconds before the camera shake preset will show up inside of Unity's project folder. After that you can assign it to a Camera Shake component on your camera gameobject. If you want to use multiple Camera Shake presets, you can do so by adding more Camera Shake components with each their own Camera Shake preset", MessageType.Info);
        }
    }
    private void OnDestroy() {
        if (cameraShake != null) cameraShake.previewShake = false;
    }
    //creating a new save with all the save data variables
    private ShakeSaveData CreateShakeSave() {
        ShakeSaveData shakeSave = new ShakeSaveData();
        shakeSave.duration = cameraShake.duration;
        shakeSave.magnitude = cameraShake.magnitude;
        shakeSave.interpolationSpeed = cameraShake.interpolationSpeed;
        shakeSave.roughness = cameraShake.roughness;
        shakeSave.rotationMagnitude = cameraShake.rotationMagnitude;
        shakeSave.originalPosition = cameraShake.originalPosition;
        shakeSave.originalRotation = cameraShake.originalRotation;
        return shakeSave;
    }

}
