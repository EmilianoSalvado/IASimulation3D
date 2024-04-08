using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed;
    float x, y;

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (x != 0 || y != 0)
            _rb.MovePosition(transform.position += CalculateVelocity(x, y));
    }

    Vector3 CalculateVelocity(float x, float y)
    {
        return Vector3.ClampMagnitude(transform.forward * y + transform.right * x, 1) * (_speed * Time.fixedDeltaTime);
    }
}
