using UnityEngine;
using System.Collections;
using System;

public class JointInfo {

	public DateTime time;
	public float[][] jointInfo;

	public JointInfo(DateTime d, float[][] jointInfo) {
		time = d;
		this.jointInfo = jointInfo;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
