using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private Text _task;
    [SerializeField] private GameObject _elementTemplate;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private GameObject _buttonRestart;

    private List<int> _exeption;
    private string[] _tasksArray;
    private int _currentLevel;
    private GameObject _correctAnswer;

    private void Start()
    {
        _exeption = new List<int>();

        _tasksArray = _level.Task.Split(',');

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
        if (_currentLevel < _level.Level�omplexity.Length)
        {
            if (_currentLevel != 0)
            {
                DeliteCurrentLevel();
            }
            int rightAnswer = GetRandomIntWrongElement(_tasksArray.Length);
            _exeption.Add(rightAnswer);                      
            _task.text = "Find " + _tasksArray[rightAnswer];

            int complexity = _level.Level�omplexity[_currentLevel];

            int indexOfRightAnswer = Random.Range(0, complexity - 1);

            int[] levelSize = CalculateSize(complexity);

            int count = 0;
            int StartPosX = levelSize[1] / 2 * 80;
            int StartPosY = levelSize[0] / 2 * 80;

            for (int i = 0; i < levelSize[0]; i++)
            {
                for (int j = 0; j < levelSize[1]; j++)
                {
                    GameObject gameObject = Instantiate(_elementTemplate, _parent);
                    if (indexOfRightAnswer == count)
                    {
                        SetSettings(gameObject, i, j, rightAnswer, StartPosX, StartPosY);
                        gameObject.AddComponent(typeof(RightAnswer));
                        _correctAnswer = gameObject;
                        _correctAnswer.GetComponent<ClickElement>().CorrectAnswer.AddListener(ButttonAnswer);
                    }
                    else
                    {
                        SetSettings(gameObject, i, j, GetRandomIntRightElement(_tasksArray.Length, rightAnswer), StartPosX, StartPosY);
                    }
                    count++;
                }
            }

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

    private void SetSettings(GameObject gameObject, int i, int j, int index, float startPosX, float startPosY)
    {
        gameObject.GetComponent<Image>().sprite = _level.SpriteSheet[index];
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(j * 80 - startPosX, i * 80 - startPosY, 0);
    }

    private int GetRandomIntWrongElement(int range)
    {
        int RandomInt = Random.Range(0, range);
        foreach (int IntExeprion in _exeption)
        {
            if (RandomInt == IntExeprion)
            {
                try
                {
                    RandomInt = GetRandomIntWrongElement(range);
                }
                catch
                {
                    Debug.Log("Error");
                }

            }
        }

        return RandomInt;
    }

    private int GetRandomIntRightElement(int range, int exeprion)
    {
        int RandomInt = Random.Range(0, range);

        if (RandomInt == exeprion)
        {
            RandomInt = GetRandomIntRightElement(range, exeprion);
        }

        return RandomInt;
    }

    private void ButttonAnswer()
    {
        if (!BuildNewLevel())
        {
            _buttonRestart.GetComponent<ClickRestart>().Restart.AddListener(RestartLevel);
            _buttonRestart.SetActive(true);
        }
    }

    private void RestartLevel()
    {
        if (_tasksArray.Length >= _tasksArray.Length + _level.Level�omplexity.Length - _exeption.Count)
        {
            _currentLevel = 0;

            DeliteCurrentLevel();
            BuildNewLevel();

            _buttonRestart.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("New scene");
        }
    }
}