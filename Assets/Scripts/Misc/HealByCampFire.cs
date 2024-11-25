using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HealByCampFire : MonoBehaviour
{
    private Coroutine healCoroutine;

    [SerializeField] private float healAmount;
    [SerializeField] private float totalHealDuration;
    [SerializeField] private float healDuration;
    private float healInterval = 0.01f; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            healCoroutine = StartCoroutine(HealOverTime(character));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            StopCoroutine(healCoroutine);
            healCoroutine = null;
        }
    }
    private IEnumerator HealOverTime(Character character)
    {
        float healAmountPerStep = healAmount / (healDuration / healInterval);
        float elapsedTime = 0f;

        while (true)
        {
            yield return new WaitForSeconds(totalHealDuration - healDuration);

            character.GetComponent<PlayerEvent>().Visit(this.gameObject.tag);

            elapsedTime = 0f;
            while (elapsedTime < healDuration)
            {
                character.RecoverHP(healAmountPerStep);
                elapsedTime += healInterval;
                yield return new WaitForSeconds(healInterval);
            }
        }
    }
}
