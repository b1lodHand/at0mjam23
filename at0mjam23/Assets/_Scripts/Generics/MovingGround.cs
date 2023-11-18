using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    // Fields.
    [SerializeField] private Rigidbody2D m_rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out PlayerMovement player)) return;
        player.transform.SetParent(transform, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!collision.gameObject.TryGetComponent(out PlayerMovement player)) return;
        player.transform.SetParent(null, true);
    }
}
