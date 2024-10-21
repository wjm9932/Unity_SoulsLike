using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField]
    private Image statusBar;
    private Slider easeBar;
    private float lerpSpeed = 3f;

    private void Awake()
    {
        easeBar = GetComponent<Slider>();
    }

    public void UpdateStatusBar(float value, float maxValue)
    {
        statusBar.fillAmount = value / maxValue;

        if(statusBar.fillAmount >= easeBar.value)
        {
            easeBar.value = statusBar.fillAmount;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (statusBar.fillAmount <= easeBar.value)
        {
            easeBar.value = Mathf.Lerp(easeBar.value, statusBar.fillAmount, lerpSpeed * Time.deltaTime);
        }
    }
}
