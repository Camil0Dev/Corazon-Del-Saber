using UnityEngine;

public class BossMovement
{
    private Rigidbody2D _rb;
    private Transform _transform;
    private float _speed;
    private Transform _spriteTransform;
    private bool _defaultFacingRight;

    public BossMovement(Rigidbody2D rb, Transform transform, float speed, Transform spriteTransform, bool defaultFacingRight)
    {
        _rb = rb;
        _transform = transform;
        _speed = speed;
        _spriteTransform = spriteTransform;
        _defaultFacingRight = defaultFacingRight;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)_transform.position).normalized;

        // 🔹 MOVIMIENTO SOLO HORIZONTAL (evita flotar)
        float moveX = direction.x * _speed;
        _rb.linearVelocity = new Vector2(moveX, 0f);

        // Giro del sprite
        if (direction.x != 0)
        {
            float sign = _defaultFacingRight ? 1f : -1f;
            float newScaleX = sign * Mathf.Sign(direction.x) * Mathf.Abs(_spriteTransform.localScale.x);
            _spriteTransform.localScale = new Vector3(newScaleX, _spriteTransform.localScale.y, _spriteTransform.localScale.z);
        }
    }

    public void StopMoving()
    {
        _rb.linearVelocity = Vector2.zero;
    }
}