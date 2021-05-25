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
    public int score;

    private float timer;

    void Start()
    {
        timer = expireTime;
        slider.maxValue = timer;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            setProcessTime(timer);
        } else
        {
            timer = expireTime;
            slider.maxValue = timer;
            GameObject.Find("GameManager").GetComponent<GameManager>().increaseScore(-score/2);
        }
    }

    public void setProcessTime(float normalizedTime)
    {
        slider.value = normalizedTime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
