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
        if (_rb.bodyType == RigidbodyType2D.Static) return;

        Vector2 direction = (targetPosition - (Vector2)_transform.position).normalized;
        _rb.linearVelocity = new Vector2(direction.x * _speed, 0f);

        if (Mathf.Abs(direction.x) > 0.01f)
        {
            bool isMovingRight = direction.x > 0;
            bool shouldFlip = isMovingRight != _defaultFacingRight;

            float scaleX = Mathf.Abs(_spriteTransform.localScale.x) * (shouldFlip ? -1f : 1f);
            _spriteTransform.localScale = new Vector3(scaleX, _spriteTransform.localScale.y, _spriteTransform.localScale.z);
        }
    }

    public void StopMoving()
    {
        if (_rb.bodyType != RigidbodyType2D.Static)
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }
}