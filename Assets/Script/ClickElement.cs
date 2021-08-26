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
        TryGetComponent(out RightAnswer rightAnswer);
        if (rightAnswer != null)
        {
            StartCoroutine(ShakeAndBounceCoroutine(true, false, true));
        }
        else
        {
            StartCoroutine(ShakeAndBounceCoroutine(false, true, false));
        }
    }

    IEnumerator ShakeAndBounceCoroutine(bool bounce = false, bool shake = false, bool rightAnswer = false)
    {
        if (bounce)
        {
            transform.DOShakeScale(0.5f, strength: new Vector3(0.1f * Mathf.Sign(this.transform.position.x), 0.1f * Mathf.Sign(this.transform.position.y), 0), vibrato: 0, randomness: 0, fadeOut: false);
        }
        else if (shake)
        {
            transform.DOShakePosition(1.5f, strength: new Vector3(1.5f, 0, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
        }

        if (rightAnswer)
        {
            yield return new WaitForSeconds(0.8f);
            CorrectAnswer?.Invoke();
        }
        else if (bounce)
            yield return new WaitForSeconds(0.8f);
        else if (shake)
            yield return new WaitForSeconds(1.5f);
    }
}

