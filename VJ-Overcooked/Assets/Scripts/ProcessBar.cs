using System;
using UnityEngine;
using UnityEngine.UI;

public class ProcessBar : MonoBehaviour
{
    public GameObject processBarObject;
    public Slider slider;

    public void setMaxTime(float normalizedTime) {

        processBarObject.SetActive(true);
        slider.value = normalizedTime;
    }

    public void setProcessTime(float normalizedTime) {
        slider.value = normalizedTime;
    }

    public void hide()
    {
        processBarObject.SetActive(false);
    }
}
