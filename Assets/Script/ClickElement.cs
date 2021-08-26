using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

public class ClickElement : MonoBehaviour
{
    public UnityEvent CorrectAnswer;

    private void Start()
    {
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        StartCoroutine(ShakeAndBounceCoroutine());
    }

    IEnumerator ShakeAndBounceCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        TryGetComponent(out RightAnswer rightAnswer);
        if (rightAnswer != null)
        {
            transform.DOShakeScale(0.5f, strength: new Vector3(0.1f * Mathf.Sign(this.transform.position.x), 0.1f * Mathf.Sign(this.transform.position.y), 0), vibrato: 0, randomness: 0, fadeOut: false);
        }
        else
        {
            transform.DOShakePosition(1.5f, strength: new Vector3(1.5f, 0, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
        }

        if (rightAnswer != null)
        {
            yield return new WaitForSeconds(0.8f);
            CorrectAnswer?.Invoke();
        }
        else
            yield return new WaitForSeconds(1.5f);

        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

