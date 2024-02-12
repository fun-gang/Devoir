using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rate : MonoBehaviour
{
    private int rate;
    private TMP_Text rateText;
    
    void Start() {
        rateText = GameObject.FindGameObjectWithTag("RateUI").GetComponent<TMP_Text>();
        UpdateUI();
    }

    public void ChangeRate(int num, bool toNull) {
        if (toNull) rate = 0;
        else {
            rate += num;
            if (rate > 5) rate = 5;
        }
        UpdateUI();
    }
    
    private string RateConvertation(int rt) {
        if (rt <= 1) return "C";
        if (2 <= rt && rt <= 3) return "B";
        if (4 <= rt && rt <= 5) return "A";
        return "None";
    }
    
    private void UpdateUI() {
        rateText.text = RateConvertation(rate);
    }
}