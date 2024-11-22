using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] public float distance = 5f;
    [SerializeField] public float duration;
    private float speed;
    private float elapsedTime = 0f;

    void Start()
    {
        speed = distance / duration;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            float distanceThisFrame = speed * Time.deltaTime;
            transform.Translate(Vector3.forward * distanceThisFrame);
            elapsedTime += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}