using UnityEngine;
using System.Collections;
using System.IO;
public class CameraShake : MonoBehaviour {

    //public variables
    public bool previewShake;
    public Object jsonPreset;
    public Transform cameraTransform;
    public float duration;
    public float magnitude;
    public float interpolationSpeed;
    public float roughness;
    public float rotationMagnitude;
    public Vector3 originalPosition;
    public Quaternion originalRotation;

    //private variables
    private float elapsed;
    private bool shaked;
    private float shakeTime;
    private float xPos;
    private float yPos;
    private float zRot;
    private float roughnessLevel;
    private float lerpLevel;

    //just in case this will be a standalone script, not for tool purposes I set up some things to be sure.
    private void Start() {
        if (this.gameObject.GetComponent<Camera>() != null) {
            cameraTransform = this.gameObject.transform;
        } 
        elapsed = 0f;
        shaked = false;
    }

    //This is how the camera shake works
    public IEnumerator CameraShakeCoroutine(float duration, float magnitude, float smoothness, float roughness, float rotationMagnitude, Vector3 originalPos, Quaternion originalRot) {
        
            lerpLevel = smoothness;
            roughnessLevel = roughness;
            elapsed = 0f;
            shakeTime = 0f;
            xPos = Random.Range(-1f, 1f) * magnitude + transform.position.x;
            yPos = Random.Range(-1f, 1f) * magnitude + transform.position.y;
            zRot = Random.Range(-1f, 1f) * rotationMagnitude + transform.rotation.z;
        
            while (elapsed <= duration) {
                shakeTime += Time.deltaTime;
                if (shakeTime > roughnessLevel) {
                    xPos = Random.Range(-1f, 1f) * magnitude + originalPosition.x;
                    yPos = Random.Range(-1f, 1f) * magnitude + originalPosition.y;
                    zRot = Random.Range(-1f, 1f) * rotationMagnitude + originalRotation.eulerAngles.z;
                    shakeTime = 0;
                }
                if (elapsed < (duration - 2 / lerpLevel)) {
                    cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, Quaternion.Euler(cameraTransform.rotation.x, cameraTransform.rotation.y, zRot), Time.deltaTime * lerpLevel);
                    cameraTransform.position = Vector3.Lerp(cameraTransform.position, new Vector3(xPos, yPos, originalPosition.z), Time.deltaTime * lerpLevel);
                } else {
                    cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, originalRotation, Time.deltaTime * lerpLevel);
                    cameraTransform.position = Vector3.Lerp(cameraTransform.position, originalPosition, Time.deltaTime * lerpLevel);
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
        transform.rotation = originalRotation;
        transform.position = originalPosition;

    }

    //Put the coroutine inside a function, because I couldnt make it work for an editor button.. maybe later
    public void Shake() {
        StartCoroutine(CameraShakeCoroutine(duration, magnitude, interpolationSpeed, roughness, rotationMagnitude, originalPosition, originalRotation));
    }

    //little function so the user can set the position and rotation they need
    public void SetShakeTransform() {
            originalPosition = transform.position;
            originalRotation = transform.rotation;

    }


}
