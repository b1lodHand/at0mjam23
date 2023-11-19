using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerPickup : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("breaker") == 1) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerCollisionHandler player)) return;
        player.Player.ActivateBreaker();
        PlayerPrefs.SetInt("breaker", 1);

        Destroy(gameObject);
    }
}
