using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    [SerializeField]
    private bool _isCoOp = false;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    private Animator _pauseAnimator;

    private void Start()
    {
        _pauseAnimator = GameObject.Find("PauseManu_pnl").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
   
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0)) && _isGameOver)
        {
            SceneManager.LoadScene("MainMenu");
        };

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.name == "MainMenu")
            {
                Application.Quit();
            }
            else
            {
                BackToMainMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                _pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
                _pauseAnimator.SetBool("isPaused", true);
            }
            
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsCoOpMode()
    {
        return _isCoOp;
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
