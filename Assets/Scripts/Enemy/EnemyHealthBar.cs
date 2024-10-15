using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider easeBar;
    [SerializeField] private Slider hpBar;

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

        Vector3 direction = transform.position - Camera.main.transform.position;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
