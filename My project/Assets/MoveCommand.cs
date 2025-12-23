using UnityEngine;

public class MoveCommand : ICommand
{
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _speed;

    public MoveCommand(Rigidbody2D rb, Vector2 direction, float speed)
    {
        _rb = rb;
        _direction = direction;
        _speed = speed;
    }

    public void Execute()
    {
        Vector2 newPos = _rb.position + _direction * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
    }
}