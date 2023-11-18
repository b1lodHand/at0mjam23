using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBreaker : MonoBehaviour
{
    // Fields.
    [SerializeField] private OverlapCircleCheckerObstacleSensitive m_check;
    [SerializeField] private float m_breakDuration;
    [SerializeField] private List<IBreakable> m_foundBreakables = new List<IBreakable>();

    // Private.
    private IBreakable m_lastBreakable;

    // Properties.
    private bool CanBreak => m_lastBreakable == null || !m_lastBreakable.IsBroken();

    private void Update()
    {
        SearchForBreakables();
        GetInput();
    }

    void GetInput()
    {
        if (!CanBreak) return;

        if (Input.GetKeyDown(KeyCode.F) && m_foundBreakables.Count > 0)
        {
            var target = m_foundBreakables.FirstOrDefault();
            if (!target.Break(m_breakDuration)) return;

            m_lastBreakable = target;
        }
    }

    void SearchForBreakables()
    {
        m_foundBreakables.Clear();
        m_check.Result.ForEach(t =>
        {
            if (!t.TryGetComponent(out IBreakable breakable)) return;
            m_foundBreakables.Add(breakable);
        });
    }
}
