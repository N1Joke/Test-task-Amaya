using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;


public class ClickRestart : MonoBehaviour
{
    public UnityEvent Restart;

    private void Start()
    {
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        Restart?.Invoke();
    }
}
