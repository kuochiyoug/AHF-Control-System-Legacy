using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutiDisplayAvaliableScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("displays connected: " + Display.displays.Length);
        for (int i = 0; i < Display.displays.Length; i++) {
			Display.displays [i].Activate ();
		}
	}

}
