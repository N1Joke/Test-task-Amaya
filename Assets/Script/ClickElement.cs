using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

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
        if (TryGetComponent(out RightAnswer rightAnswer))
        {
            CorrectAnswer?.Invoke();
        }
        else
        {
            Debug.Log(false);
        }
    }
}

