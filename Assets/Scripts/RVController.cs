﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CollisionTypes;
using System;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}

public class RVController : MonoBehaviour {

	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	public float maxWheelSpeed;
	public bool enableFastStop;

	private CollisionDetector collDetect;
	[SerializeField]
	private AnimationCurve drivingBadnessDecay;
	public float drivingBadness; //Scales from 0 to 100;

	void Start() {
		collDetect = new CollisionDetector(this);
	}

	void Update() {
		drivingBadness -= Time.deltaTime * drivingBadnessDecay.Evaluate(drivingBadness);
		drivingBadness = Math.Max(0, drivingBadness);
		Debug.Log(drivingBadness);
	}

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider) {
		if (collider.transform.childCount == 0) {
			return;
		}

		Transform visualWheel = collider.transform.GetChild(0);

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);

		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	public void FixedUpdate() {
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * xAxis;
		float motorInput = maxMotorTorque * yAxis;
		//float motorInput= Mathf.Min(maxMotorTorque    * yAxis, maxWheelSpeed);

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				float motorVal = motorInput;


				axleInfo.leftWheel.motorTorque = motorInput;
				axleInfo.rightWheel.motorTorque = motorInput;

			}

			if (enableFastStop) {
				if (yAxis == 0) {

					axleInfo.leftWheel.brakeTorque = 500000;
					axleInfo.rightWheel.brakeTorque = 500000;
					// If the player isn't inputting, we slow down quickly
				} else {

					axleInfo.leftWheel.brakeTorque = 0;
					axleInfo.rightWheel.brakeTorque = 0;
				}
			}

			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "obstacle") {
			CollisionType colType = collDetect.getCollisionType(collision);
			drivingBadness += colType.collBadness;
			Audio.playSfx(colType.getRandomSfx());

			//TODO mess with PP propriotnoal to driving badness
		}
	}
}
