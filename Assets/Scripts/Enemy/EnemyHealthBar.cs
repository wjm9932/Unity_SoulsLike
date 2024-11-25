using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider easeBar;
    [SerializeField] private Slider hpBar;
    [SerializeField] private bool shouldLookPlayer = true;

    private float lerpSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void UpdateHealthBar(float health, float maxHealth)
    {
        hpBar.value = health / maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if(hpBar.value !=  easeBar.value)
        {
            easeBar.value = Mathf.Lerp(easeBar.value, hpBar.value, lerpSpeed * Time.deltaTime);
        }

        if(shouldLookPlayer == true)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
