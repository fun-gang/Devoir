using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetHPCells : MonoBehaviour
{
    public GameObject[] hpCells;

    public void SetHPCellsUI(int num) {
        foreach (GameObject i in hpCells) {
            i.SetActive(false);
        }
        
        for (int i = 0; i < num; i++) {
            hpCells[i].SetActive(true);
        }
    }
}