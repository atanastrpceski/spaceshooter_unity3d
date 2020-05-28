using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();

        if (_animator == null)
        {
            Debug.LogError("The Animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetPlayerNumber() == 1)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _animator.SetBool("Turn_Left", true);
                _animator.SetBool("Turn_Right", false);
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                _animator.SetBool("Turn_Left", false);
                _animator.SetBool("Turn_Right", false);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _animator.SetBool("Turn_Left", false);
                _animator.SetBool("Turn_Right", true);
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                _animator.SetBool("Turn_Left", false);
                _animator.SetBool("Turn_Right", false);
            }
        }
        else if(_player.GetPlayerNumber() == 2)
        {
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                _animator.SetBool("Turn_Left", true);
                _animator.SetBool("Turn_Right", false);
            }
            if (Input.GetKeyUp(KeyCode.Keypad4))
            {
                _animator.SetBool("Turn_Left", false);
                _animator.SetBool("Turn_Right", false);
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                _animator.SetBool("Turn_Left", false);
                _animator.SetBool("Turn_Right", true);
            }
            if (Input.GetKeyUp(KeyCode.Keypad6))
            {
                _animator.SetBool("Turn_Left", false);
                _animator.SetBool("Turn_Right", false);
            }
        }
    }
}
