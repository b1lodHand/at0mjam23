using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractorPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerCollisionHandler player)) return;
        if (player.Player.HandleDistractor()) Destroy(gameObject);
    }
}
