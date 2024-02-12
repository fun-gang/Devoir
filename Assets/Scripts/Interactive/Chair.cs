using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    private GameObject player;
    public Transform sitPosition;
    private bool alreadySit;
    [SerializeField] private bool isLeft = false;

    void Start() {
        player = GameObject.Find("Player");
    }

    public void Act() {
        if (!alreadySit) {
            player.transform.position = sitPosition.position;
            RotatePlayer();
            Movement.control = false;
            alreadySit = true;
        }
    }

    private void RotatePlayer() {
        int numRot = 0;
        if (isLeft) numRot = -1;
        else numRot = 1;
        
        player.transform.localScale = new Vector3(numRot,1,1);
    }

    public void Stop() {
        Movement.control = true;
        alreadySit = false;
    }
}
