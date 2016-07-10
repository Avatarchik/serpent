﻿using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	private const float kRotationSpeed = 120; // Deg/sec

	void Start () {

	}
	
	void Update () {
		transform.Rotate (0, kRotationSpeed * Time.deltaTime, 0);
	}
}
