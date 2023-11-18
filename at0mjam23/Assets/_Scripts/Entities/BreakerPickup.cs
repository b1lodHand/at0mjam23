using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerCollisionHandler player)) return;
        player.Player.ActivateBreaker();

        Destroy(gameObject);
    }
}
