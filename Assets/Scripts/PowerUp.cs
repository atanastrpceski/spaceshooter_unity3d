using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    // 0 - TripleShot
    // 1 - Speed
    // 2 - Shields
    [SerializeField]
    private PowerUpType _powerupType;

    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        var direction = Vector3.down;
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y <= -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupType)
                {
                    case PowerUpType.TripleShot:
                        player.EnableTripleShot();
                        break;
                    case PowerUpType.SpeedBoost:
                        player.EnableSpeedBoost();
                        break;
                    case PowerUpType.Shield:
                        player.EnableShield();
                        break;
                }

                AudioSource.PlayClipAtPoint(_clip, transform.position, 1f);

                Destroy(this.gameObject);
            }

            PlaySound();
        }
    }

    private void PlaySound()
    {
        
    }
}
