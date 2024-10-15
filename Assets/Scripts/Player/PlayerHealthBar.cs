using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
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
    public void UpdateHealthBar(float health, float maxHealth)
    {
        hpBar.fillAmount = health / maxHealth;
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
