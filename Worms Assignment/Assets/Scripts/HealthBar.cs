using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image bar;

    public void SetMaxValue(int max){
        healthSlider.maxValue = max;
        healthSlider.value = max;

        bar.color = gradient.Evaluate(1f);
    }

    public void SetValue(int amount){
        healthSlider.value = amount;
        bar.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
