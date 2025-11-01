using System;
using UnityEngine;
using UnityEngine.UI;

public class HeatBarImage : MonoBehaviour
{

    private Image image;

    public event EventHandler OnHeatBarFilled;

    private float fillSpeed = 0.1f;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0f;
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        if (image.fillAmount >= 1f)
        {
            OnHeatBarFilled?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DrainHeatBar()
    {
        image.fillAmount -= fillSpeed * Time.deltaTime;
    }
    public void FillHeatBar()
    {
        image.fillAmount += fillSpeed * Time.deltaTime;
    }
    public void ResetHeatBar()
    {
        image.fillAmount = 0f;
    }
}
