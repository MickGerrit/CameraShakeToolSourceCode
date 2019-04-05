using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    public CameraShake magnum;
    public CameraShake earthQuake;
	
	// Just an example scene
	void Update () {
		if (Input.GetButton("Fire1")) {
            magnum.Shake();
        }
        if (Input.GetButton("Fire2")) {
            Debug.Log("Shake");
            earthQuake.Shake();
        }
    }
}
