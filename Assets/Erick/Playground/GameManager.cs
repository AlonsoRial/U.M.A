using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

public class TempButton : MonoBehaviour
{
    public Button startButton;
    public Button menuButton;
    public Button quitButton;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(() => GameManager.Instance.LoadGame());

        if (menuButton != null)
            menuButton.onClick.AddListener(() => GameManager.Instance.LoadMainMenu());

        if (quitButton != null)
            quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
    }
}
