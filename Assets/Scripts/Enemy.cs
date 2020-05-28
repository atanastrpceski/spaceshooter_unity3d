using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private GameObject _singleLaserPrefab;
    [SerializeField]
    private GameObject _doubleLaserPrefab;
    
    private float _animationTime = 2.8f;
    private float _nextFire = 0.0f;
    private bool _isDestroyed = false;

    private Animator _animator;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        if (_audioSource == null)
        {
            Debug.LogError("No audio source");
        }

        if (_animator == null)
        {
            Debug.LogError("The Animator is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (!_isDestroyed && Time.time > _nextFire)
            FireLaser();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6f, 0);
        }

        if (_isDestroyed)
        {
            _speed -= _speed / _animationTime * Time.deltaTime;
        }
    }

    private void FireLaser()
    {
        _nextFire = Time.time + Random.Range(3f, 7f);

        var singleLaser = Random.Range(0, 2) == 1;
        GameObject newCanon;

        if (singleLaser)
            newCanon = Instantiate(_singleLaserPrefab, transform.position, Quaternion.identity);
        else
            newCanon = Instantiate(_doubleLaserPrefab, transform.position, Quaternion.identity);

        var allLasers = newCanon.GetComponentsInChildren<Laser>();
        foreach (var laser in allLasers)
        {
            laser.MarkAsEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.gameObject.GetComponent<Player>();

            if (player != null)
                player.Damage();

            Destroy();
        }

        if (other.tag == "Laser")
        {
            var laser = other.GetComponent<Laser>();
            var player = laser.GetPlayer();

            if (player != null)
            {
                Destroy(other.gameObject);

                player.IncreaseScore(10);
                Destroy();
            }
        }
    }

    void Destroy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _isDestroyed = true;

        Destroy(GetComponent<Collider2D>());
        _audioSource.Play();

        Destroy(this.gameObject, _animationTime);
    }
}
