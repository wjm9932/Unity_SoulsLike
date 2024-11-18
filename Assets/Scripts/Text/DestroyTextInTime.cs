using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class DestroyTextInTime : MonoBehaviour, IPoolableObject
{
    public delegate void OnDestroyEventHandler();
    public event OnDestroyEventHandler OnDestroy;

    public IObjectPool<GameObject> pool { get; private set; }

    private Color originTextColor;
    private Coroutine fadeCoroutine;

    [SerializeField]
    private float duration;
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originTextColor = text.color;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeOutAndDestroy(float duration)
    {
        yield return new WaitForSeconds(1.2f);

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(text.color.a, 0f, elapsedTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, newAlpha);
            yield return null;
        }

        OnDestroy?.Invoke();
        Release();
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public void Initialize(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        this.GetComponent<RectTransform>().SetParent(parent, false);
        fadeCoroutine = StartCoroutine(FadeOutAndDestroy(duration - 1.2f));
    }
    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        this.GetComponent<RectTransform>().SetParent(null, false);
        StopCoroutine(fadeCoroutine);
        text.color = originTextColor;
        OnDestroy = null;
        pool.Release(this.gameObject);
    }
}
