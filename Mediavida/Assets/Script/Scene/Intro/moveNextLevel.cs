﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveNextLevel : MonoBehaviour {
    void Update() {
        if (Input.anyKey) {
            SceneManager.LoadScene(2);
        }
    }
}
