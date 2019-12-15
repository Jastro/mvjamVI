using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDetection : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collision) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement PM = player.GetComponent<PlayerMovement>();
        PM.target = collision;
    }

    void OnTriggerExit2D(Collider2D collision) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement PM = player.GetComponent<PlayerMovement>();
        PM.target = null;
    }
}
