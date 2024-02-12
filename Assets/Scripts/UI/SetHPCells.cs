using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHPCells : MonoBehaviour
{
    public GameObject[] hpCells;
    public Image img;

    public void SetHPCellsUI(int num) {
        foreach (GameObject i in hpCells) {
            i.SetActive(false);
        }
        
        for (int i = 0; i < num; i++) {
            hpCells[i].SetActive(true);
        }
    }

    public void SetEnergyBarUI(float per) {
        img.fillAmount = per;
    }
}
