using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveToInicio : MonoBehaviour {
    float currCountdownValue = 2;

    public IEnumerator StartCountdown(float countdownValue = 2) {
        currCountdownValue = countdownValue;
        while(currCountdownValue > 0) {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }

    void Update() {
        if(currCountdownValue == 0) {
            SceneManager.LoadScene(0);
        } else {
            StartCoroutine(StartCountdown());
        }
    }
}
