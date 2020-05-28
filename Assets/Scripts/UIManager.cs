using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private Text _scoreTextP1;

    [SerializeField]
    private Text _scoreTextP2;

    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _currentImageP1;
    [SerializeField]
    private Image _currentImageP2;

    private bool isGameOverP1;
    private bool isGameOverP2;

    private int _singlePlayerScore;
    private int _singlePlayerBestScore;

    bool enableGameOverFlicker = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        _restartLevelText.text = "Click the screen to restart the level";
#elif UNITY_IOS
        _restartLevelText.text = "Click the screen to restart the level";
#else
        _restartLevelText.text = "Press the \"R\" key on your keyboard to restart the level";
#endif
        _scoreTextP1.text = "Score: " + 0;

        if(_scoreTextP2 != null)
            _scoreTextP2.text = "Score: " + 0;

        _singlePlayerBestScore = PlayerPrefs.GetInt("HighScore", 0);
        _bestScoreText.text = "Best: " + _singlePlayerBestScore;

        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is null");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    IEnumerator FlickerText()
    {
        while (enableGameOverFlicker)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(!_gameOverText.gameObject.activeSelf);
        }
    }

    public void UpdateScore(Player player, int score)
    {
        if (player.GetPlayerNumber() == 1)
        {
            _singlePlayerScore = score;
            _scoreTextP1.text = "Score: " + score;
        }
        else
            _scoreTextP2.text = "Score: " + score;
    }

    public void CheckForBestScore()
    {
        if (_singlePlayerScore > _singlePlayerBestScore)
        {
            _singlePlayerBestScore = _singlePlayerScore;
            PlayerPrefs.SetInt("HighScore", _singlePlayerBestScore);
            _bestScoreText.text = "Best: " + _singlePlayerBestScore;
        }
    }

    void GameOver()
    {
        if (!_gameManager.IsCoOpMode() || isGameOverP1 && isGameOverP2)
        {
            CheckForBestScore();

            _gameOverText.gameObject.SetActive(true);
            _restartLevelText.gameObject.SetActive(true);

            enableGameOverFlicker = true;
            StartCoroutine(FlickerText());

            _spawnManager.GameOver();
            _gameManager.GameOver();
        }
    }

    public void UpdateLives(Player player, int currentLives)
    {
        if (player.GetPlayerNumber() == 1)
        {
            _currentImageP1.sprite = _liveSprites[currentLives];
            if (currentLives == 0)
            {
                isGameOverP1 = true;
                GameOver();
            }
        }
        else
        {
            _currentImageP2.sprite = _liveSprites[currentLives];
            if (currentLives == 0)
            {
                isGameOverP2 = true;
                GameOver();
            }
        }
    }

    public void ResumePlay()
    {
        _gameManager.ResumeGame();
    }

    public void BackToMainManu()
    {
        _gameManager.BackToMainMenu();
    }
}
