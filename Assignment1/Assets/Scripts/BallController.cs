using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    red,
    green,
    blue,
    debris
}

public class BallController : MonoBehaviour
{
    public float velocity;
    public BallType ballType;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private Vector2 _velocityCache;
    // TODO: a int counting how many balls in collision, and another int counting how many players in collision
    private int _ballCol;
    private int _playerCol;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(BallType type)
    {
        // Position, velocity, and drag
        transform.position = new Vector3(0f, 6.5f, 0f);
        _rigidbody.velocity = new Vector2(0f, 0f);
        _velocityCache = new Vector2(0f, 0f);
        _rigidbody.drag = 10;
        _rigidbody.angularDrag = 10;
        // Counters
        _ballCol = 0;
        _playerCol = 0;
        // TODO: BallType initialization
        if (type == BallType.red)
        {
            _renderer.color = new Color32((byte)255, (byte)0, (byte)0, (byte)255);
        }
        if (type == BallType.green)
        {
            _renderer.color = new Color32((byte)0, (byte)255, (byte)0, (byte)255);
        }
        if (type == BallType.blue)
        {
            _renderer.color = new Color32((byte)0, (byte)0, (byte)255, (byte)255);
        }
        if (type == BallType.debris)
        {
            _renderer.color = new Color32((byte)128, (byte)128, (byte)128, (byte)255);
        }
        ballType = type;
    }

    public void OnPause()
    {
        _velocityCache = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
        _rigidbody.velocity = new Vector2(0, 0);
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void OnResume()
    {
        _rigidbody.velocity = _velocityCache;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, velocity * -1);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Set all drag to 0 upon contact with the track
        if (col.gameObject.tag == "Track")
        {
            _rigidbody.drag = 0;
            _rigidbody.angularDrag = 0;
        }
        // Increase corresponding counters
        if (col.gameObject.tag == "Ball")
        {
            _ballCol += 1;
        }
        if (col.gameObject.tag == "Player")
        {
            _playerCol += 1;
        }
        // TODO: check debris destroy using relative position
        // TODO: check destroy: check both counters, check y position relative to player
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // decrease corresponding counters
        if (col.gameObject.tag == "Ball")
        {
            _ballCol -= 1;
        }
        if (col.gameObject.tag == "Player")
        {
            _playerCol -= 1;
        }
    }
}
