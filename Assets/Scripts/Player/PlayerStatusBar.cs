using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBar : MonoBehaviour
{
    [SerializeField] private Slider statusBar;
    [SerializeField] private Slider easeBar;

    private float originMaxHp = 100f;
    private float lerpSpeed = 3f;
    private float barBackGroundSize;
    private float barSize;
    private bool isResizingDone = true;
    private void Awake()
    {
        barBackGroundSize = this.GetComponent<RectTransform>().sizeDelta.x;
        barSize = statusBar.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void UpdateStatusBar(float value, float maxValue)
    {
        statusBar.value = value / maxValue;

        if (statusBar.value > easeBar.value)
        {
            easeBar.value = statusBar.value;
        }
    }

    void Update()
    {
        if (statusBar.value <= easeBar.value)
        {
            easeBar.value = Mathf.Lerp(easeBar.value, statusBar.value, lerpSpeed * Time.deltaTime);
        }
    }

    public IEnumerator ResizeStatusBarSize(float amount)
    {
        isResizingDone = false;
        float percentage = amount / originMaxHp;
        float duration = 0.5f;

        float elapsedTime = 0f;
        float amountPerTick = (barBackGroundSize * percentage) / 100f;
        float interval = duration / 100f;

        while (elapsedTime < duration)
        {
            if (elapsedTime + interval > duration)
            {
                break;
            }

            RectTransform transform = this.gameObject.GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(transform.sizeDelta.x + amountPerTick, transform.sizeDelta.y);

            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }

        statusBar.GetComponent<RectTransform>().sizeDelta = new Vector2(statusBar.GetComponent<RectTransform>().sizeDelta.x + barSize * percentage, statusBar.GetComponent<RectTransform>().sizeDelta.y);
        easeBar.GetComponent<RectTransform>().sizeDelta = new Vector2(statusBar.GetComponent<RectTransform>().sizeDelta.x, statusBar.GetComponent<RectTransform>().sizeDelta.y);

        isResizingDone = true;
    }

    public bool IsResizingDone()
    {
        return isResizingDone;
    }
}
