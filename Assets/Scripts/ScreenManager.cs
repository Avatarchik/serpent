﻿using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

    public GameObject initialOpen;

    private GameObject current;

    public void OpenScreen(GameObject screen) {
        if (current != null)
            current.SetActive(false);

        screen.SetActive(true);
        current = screen;
    }

	void Start () {
        current = initialOpen;

        if (current != null)
            current.SetActive(true);
	}
	
	void Update () {
	
	}
}