using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Level[] _AllPossibleLevels;
    [SerializeField] private Text _task;
    [SerializeField] private GameObject _elementTemplate;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private GameObject _buttonRestart;
    [SerializeField] private GameObject _cell;
    [SerializeField] private GameObject _backGroundLoad;

    private Level _level;
    private List<int> _exeption;
    private string[] _tasksArray;
    private int _currentLevel;
    private GameObject _correctAnswer;
    private bool _bounceOnStart = true;

    private void Start()
    {
        _level = _AllPossibleLevels[Random.Range(0, _AllPossibleLevels.Length)];

        _exeption = new List<int>();

        _tasksArray = _level.Task.Split(',');
        for (int i = 0; i < _tasksArray.Length; i++)
        {
            _exeption.Add(i);
        }

        BuildNewLevel();
    }

    private void DeliteCurrentLevel()
    {
        for (int i = _parent.childCount - 1; i >= 0; i--)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
    }

    public bool BuildNewLevel()
    {
        if (_currentLevel < _level.Level—omplexity.Length)
        {
            if (_currentLevel != 0)
            {
                DeliteCurrentLevel();
            }
            int rightAnswer = 0;
            try
            {
                rightAnswer = _exeption[Random.Range(0, _exeption.Count - 1)];
            }
            catch
            {
                Debug.Log("error");
            }
            _exeption.Remove(rightAnswer);

            _task.text = "Find " + _tasksArray[rightAnswer];

            int complexity = _level.Level—omplexity[_currentLevel];

            int indexOfRightAnswer = Random.Range(0, complexity - 1);

            int[] levelSize = CalculateSize(complexity);

            int count = 0;
            int StartPosX = levelSize[1] / 2 * 80;
            int StartPosY = levelSize[0] / 2 * 80;

            for (int i = 0; i < levelSize[0]; i++)
            {
                for (int j = 0; j < levelSize[1]; j++)
                {
                    GameObject elementGobj = Instantiate(_elementTemplate, _parent);
                    GameObject cellGobj = Instantiate(_cell, _parent);
                    if (indexOfRightAnswer == count)
                    {
                        SetSettings(elementGobj, i, j, rightAnswer, StartPosX, StartPosY, cellGobj);
                        elementGobj.AddComponent(typeof(RightAnswer));
                        _correctAnswer = elementGobj;
                        _correctAnswer.GetComponent<ClickElement>().CorrectAnswer.AddListener(ButttonAnswer);
                    }
                    else
                    {
                        SetSettings(elementGobj, i, j, GetRandomIntWrongElement(_tasksArray.Length, rightAnswer), StartPosX, StartPosY, cellGobj);
                    }
                    count++;
                }
            }

            _bounceOnStart = false;

            _currentLevel++;

            return true;
        }
        else
            return false;
    }

    private int[] CalculateSize(int width, int height = 0)
    {
        int[] size = new int[2];

        if (width % 4 == 0 && width / 4 != 1)
        {
            height += 3;
            size = CalculateSize(width / 4, height);
        }
        else if (width % 3 == 0 && width / 3 != 1)
        {
            height += 2;
            size = CalculateSize(width / 3, height);
        }
        else if (width % 2 == 0 && width / 2 != 1)
        {
            height++;
            size = CalculateSize(width / 2, height);
        }
        else
        {
            height++;
            if (height < width)
            {
                size[0] = height;
                size[1] = width;
            }
            else
            {
                size[0] = width;
                size[1] = height;
            }
        }

        return size;
    }

    private void SetSettings(GameObject element, int i, int j, int index, float startPosX, float startPosY, GameObject cell)
    {
        Image image = element.GetComponent<Image>();
        image.sprite = _level.SpriteSheet[index];
        RectTransform rectTransform = element.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(image.preferredWidth / _level.SpriteScaleFactor, image.preferredHeight / _level.SpriteScaleFactor);
        rectTransform.anchoredPosition = new Vector3(j * 85 - startPosX, i * 85 - startPosY, 0);
        cell.GetComponent<RectTransform>().anchoredPosition = new Vector3(j * 85 - startPosX, i * 85 - startPosY, 0);
        if (_bounceOnStart)
        {
            element.GetComponent<StartIntro>().DoStartIntro();
        }
    }

    private int GetRandomIntWrongElement(int range, int exeprion)
    {
        int RandomInt = Random.Range(0, range);

        if (RandomInt == exeprion)
        {
            RandomInt = GetRandomIntWrongElement(range, exeprion);
        }

        return RandomInt;
    }

    private void ButttonAnswer()
    {
        if (!BuildNewLevel())
        {
            _buttonRestart.GetComponent<ClickRestart>().Restart.AddListener(RestartLevel);
            DisableButtons();
            _backGroundLoad.GetComponent<FadeEffect>().FadeIn(1f, 0.8f, false);
            _buttonRestart.SetActive(true);
        }
    }

    private void RestartLevel()
    {
        StartCoroutine(DelayLoadLevel());
    }

    private void DisableButtons()
    {
        foreach (Transform transform in _parent.transform)
        {
            if (transform.TryGetComponent<ClickElement>(out ClickElement clickElement))
                clickElement.DisableButton();
        }
    }

    private IEnumerator DelayLoadLevel()
    {
        _backGroundLoad.GetComponent<FadeEffect>().FadeIn(1.5f, 1.0f, false);
        _buttonRestart.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        if (_exeption.Count >= _level.Level—omplexity.Length)
        {
            _currentLevel = 0;

            DeliteCurrentLevel();
            BuildNewLevel();

            _backGroundLoad.GetComponent<FadeEffect>().FadeIn(1f, 0.0f, false);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}