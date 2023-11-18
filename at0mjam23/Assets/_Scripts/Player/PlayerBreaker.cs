using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBreaker : MonoBehaviour
{
    // Fields.
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private LayerMask m_obstacleMask;
    [SerializeField] private BreakerBullet m_bulletPrefab;
    [SerializeField] private Texture2D m_crosshair;
    [SerializeField] private float m_breakDuration;
    [SerializeField] private float m_bulletSpeed;

    // Private.
    private IBreakable m_lastBreakable;
    private bool m_active = false;

    // Properties.
    private bool CanBreak => m_active && (m_lastBreakable == null || !m_lastBreakable.IsBroken());
    public bool Active => m_active;

    private void Start()
    {
        Deactivate();
    }

    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (!CanBreak) return;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mousePosition = GameManager.Instance.ActiveCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            var bullet = Instantiate(m_bulletPrefab, transform.position, Quaternion.identity);
            bullet.Init(this, m_breakDuration, (mousePosition - transform.position).normalized * m_bulletSpeed);
        }
    }

    public void BreakCallback(IBreakable breakableHit)
    {
        m_lastBreakable = breakableHit;
    }

    public void Activate()
    {
        m_active = true;
        Cursor.SetCursor(m_crosshair, Vector2.one / 2, CursorMode.Auto);
    }

    public void Deactivate()
    {
        m_active = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
