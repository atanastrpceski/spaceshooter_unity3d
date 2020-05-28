using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private bool _isEnemyLaser = false;
    private Player _player;

    // Update is called once per frame
    void Update()
    {
        if (!_isEnemyLaser)
        {
            Move(Vector3.up);
        }
        else
        {
            Move(Vector3.down);
        }
    }

    void Move(Vector3 dir)
    {
        transform.Translate(dir * _speed * Time.deltaTime);

        if (transform.position.y > 6f)
        {
            if (transform.parent == null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public Player GetPlayer()
    {
        return _player;
    }

    public void AssignPlayerToLaser(Player player)
    {
        _player = player;
    }

    public void MarkAsEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
    }
}
