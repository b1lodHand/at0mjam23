using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractorBehaviour : MonoBehaviour
{
    // Fields.
    [SerializeField] private DistractorPickup m_pickupPrefab;
    [SerializeField] private Transform m_pickupSpawnPosition;
    [SerializeField] private float m_distractionDuration;
    [SerializeField] private OverlapCircleCheckerObstacleSensitive m_guardCheck;
    [SerializeField] private AudioClip m_collisionClip;

    private void Start()
    {
        Invoke("DestroySelf", m_distractionDuration);
    }

    private void Update()
    {
        ApplyLogic();
    }

    void ApplyLogic()
    {
        if (m_guardCheck.Result.Count == 0) return;
        m_guardCheck.Result.ForEach(g =>
        {
            if (!g.TryGetComponent(out GuardCollisionHandler guard)) return;
            guard.StartDistractionBy(this, m_distractionDuration);
        });
    }

    void DestroySelf()
    {
        Instantiate(m_pickupPrefab, m_pickupSpawnPosition.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(m_collisionClip, transform.position, .5f);
    }
}
