using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField]
    private Image hpBar;
    private Slider easeBar;
    private float lerpSpeed = 3f;

    private void Awake()
    {
        easeBar = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    public void UpdateStatusBar(float value, float maxValue)
    {
        hpBar.fillAmount = value / maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        if (hpBar.fillAmount != easeBar.value)
        {
            easeBar.value = Mathf.Lerp(easeBar.value, hpBar.fillAmount, lerpSpeed * Time.deltaTime);
        }
    }
}
