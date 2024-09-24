using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestroyTextInTime : MonoBehaviour
{
    public delegate void OnDestroyEventHandler();
    public event OnDestroyEventHandler OnDestroy;

    [SerializeField]
    private float duration;
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutAndDestroy(duration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOutAndDestroy(float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = text.color.a;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            Color newColor = text.color;
            newColor.a = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            text.color = newColor;
            yield return null;
        }

        OnDestroy?.Invoke();
        Destroy(gameObject);
    }
}
