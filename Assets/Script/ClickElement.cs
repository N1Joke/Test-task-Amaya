using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

public class ClickElement : MonoBehaviour
{ 
    public UnityEvent CorrectAnswer;
    private bool _NotAnswerOnClick = false;

    private void Start()
    {
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (!_NotAnswerOnClick)
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
    }

    public IEnumerator ShakeAndBounceCoroutine(bool bounce = false, bool shake = false, bool rightAnswer = false, float waitBeforeAction = 0f)
    {
        if (waitBeforeAction != 0f)
        {
            yield return new WaitForSeconds(waitBeforeAction);
        }

        if (bounce)
        {
            //RectTransform rectTransform = GetComponent<RectTransform>();
            //transform.DOShakeScale(0.5f, strength: new Vector3(0.01f * rectTransform.sizeDelta.x, 0.01f * rectTransform.sizeDelta.y, 0), vibrato: 0, randomness: 1, fadeOut: true);
            RectTransform rectTransform = GetComponent<RectTransform>();
            //rectTransform.DOShakeScale(5f, strength: new Vector3(0.01f * rectTransform.sizeDelta.x, 0.01f * rectTransform.sizeDelta.y, 0), 0,0);
            rectTransform.DOPunchScale(new Vector3(-0.005f * rectTransform.sizeDelta.x, -0.005f * rectTransform.sizeDelta.y, 0), 0.5f, 0, 0);
        }
        else if (shake)
        {
            transform.DOShakePosition(1.5f, strength: new Vector3(1.5f, 0, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
        }

        if (rightAnswer)
        {
            yield return new WaitForSeconds(0.5f);
            CorrectAnswer?.Invoke();
        }
        else if (bounce)
            yield return new WaitForSeconds(0.5f);
        else if (shake)
            yield return new WaitForSeconds(1.5f);
    }

    public void DisableButton()
    {
        _NotAnswerOnClick = true;
    }
}

