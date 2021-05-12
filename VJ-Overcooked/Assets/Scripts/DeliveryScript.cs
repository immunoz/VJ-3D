using System;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public float expireTime;
    public string dish;

    void Start()
    {
        slider.maxValue = expireTime;
    }

    private void Update()
    {
        if (expireTime > 0)
        {
            expireTime -= Time.deltaTime;
            setProcessTime(expireTime);
        }
    }

    public void setProcessTime(float normalizedTime)
    {
        slider.value = normalizedTime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
