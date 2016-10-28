using UnityEngine;
using System.Collections;

public class PowerBean : MonoBehaviour {

    public int _rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Time.deltaTime * _rotationSpeed, 0, Space.World);
	}
}
