using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOutside : MonoBehaviour
{
    public void Act() {
        SceneManager.LoadScene("Anomaly");
    }

    public void Stop() {
        
    }
}
