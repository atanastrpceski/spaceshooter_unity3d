using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float _speedMultiplier = 2f;

    [SerializeField]
    private int _powerUpActiveInSeconds = 5;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _triplePrefab;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private GameObject _leftEnginePrefab, _rightEnginefab;

    [SerializeField]
    private AudioClip _laserSound;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _score;

    [SerializeField]
    public int _playerNumber;

    private bool _enableTripleShot;
    private bool _enableShield;
    private bool _enableSpeedBoost;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;

    private float nextFire = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is null!");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The Spawn Manager is null!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is null!");
        }

        if (!_gameManager.IsCoOpMode())
            transform.position = new Vector3(0, 0, 0);

        _audioSource.clip = _laserSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerNumber == 1)
        {
            CalculateMovementP1();

            if ((Input.GetKeyDown(KeyCode.Space)
                || CrossPlatformInputManager.GetButtonDown("Jump")) 
                && Time.time > nextFire)
            {
                FireLaser();
            }
        }

        if (_playerNumber == 2)
        {
            CalculateMovementP2();

            if (Input.GetKeyDown(KeyCode.Keypad0) && Time.time > nextFire)
            {
                FireLaser();
            }
        }
    }

    void CalculateMovementP1()
    {
        // Time.deltaTime = real time, 1 second

        //var horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        //var verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        //var direction = new Vector3(horizontalInput, verticalInput, 0);
        //transform.Translate(direction * _speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5, 0), 0);

        if (transform.position.x >= 10.25)
        {
            transform.position = new Vector3(-10.25f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10.25)
        {
            transform.position = new Vector3(10.25f, transform.position.y, 0);
        }
    }

    void CalculateMovementP2()
    {
        var horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        var verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5, 0), 0);

        if (transform.position.x >= 10.25)
        {
            transform.position = new Vector3(-10.25f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10.25)
        {
            transform.position = new Vector3(10.25f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        nextFire = Time.time + _fireRate;
        if (_enableTripleShot)
        {
            var tripleLaser = Instantiate(_triplePrefab, transform.position, Quaternion.identity);
            foreach (var laser in tripleLaser.GetComponentsInChildren<Laser>())
            {
                laser.AssignPlayerToLaser(this);
            }
        }
        else
        {
            var laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            laser.GetComponent<Laser>().AssignPlayerToLaser(this);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_enableShield)
        {
            _shieldPrefab.SetActive(false);
            _enableShield = false;
            return;
        }

        _lives--;

        if (_lives == 2)
            _leftEnginePrefab.SetActive(true);

        if (_lives == 1)
            _rightEnginefab.SetActive(true);

        _uiManager.UpdateLives(this, _lives);

        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }

    public void IncreaseScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(this, _score);
    }

    public void EnableTripleShot()
    {
        _enableTripleShot = true;
        StartCoroutine(DisableTripleShot());
    }

    public void EnableSpeedBoost()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(DisableSpeedBoost());
    }

    public void EnableShield()
    {
        _enableShield = true;
        _shieldPrefab.SetActive(true);
    }

    public int GetPlayerNumber()
    {
        return _playerNumber;
    }

    IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(_powerUpActiveInSeconds);
        _enableTripleShot = false;
    }

    IEnumerator DisableSpeedBoost()
    {
        yield return new WaitForSeconds(_powerUpActiveInSeconds);
        _speed /= _speedMultiplier;
    }
}
