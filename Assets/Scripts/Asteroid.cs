using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 25.0f;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager not found!");
        }
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            var laser = other.GetComponent<Laser>();
            var player = laser.GetPlayer();

            if (player != null)
            {
                player.IncreaseScore(30);
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.15f);
            _spawnManager.StartSpawing();
        }
    }
}
