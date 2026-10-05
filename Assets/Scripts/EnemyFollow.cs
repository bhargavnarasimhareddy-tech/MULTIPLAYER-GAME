using UnityEngine;
using Unity.Netcode;

public class EnemyFollow : NetworkBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Only the player who owns the Enemy can control it
        if (!IsOwner)
            return;

        float horizontal = 0f;
        float vertical = 0f;

        // Arrow keys
        if (Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1f;

        if (Input.GetKey(KeyCode.RightArrow))
            horizontal = 1f;

        if (Input.GetKey(KeyCode.UpArrow))
            vertical = 1f;

        if (Input.GetKey(KeyCode.DownArrow))
            vertical = -1f;

        Vector3 movement = new Vector3(
            horizontal,
            0f,
            vertical
        );

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        rb.MovePosition(
            rb.position +
            movement * speed * Time.fixedDeltaTime
        );
    }
}