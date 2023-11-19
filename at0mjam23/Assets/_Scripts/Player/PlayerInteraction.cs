using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Fields.
    [SerializeField] private Player m_playerScript;
    [SerializeField] private OverlapCircleCheckerObstacleSensitive m_check;
    [SerializeField] private List<IInteractable> m_foundInteractables = new List<IInteractable>();

    private void Update()
    {
        SearchForInteractables();
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_foundInteractables.Count > 0) m_foundInteractables.FirstOrDefault().Interact(this, m_playerScript);
    }

    void SearchForInteractables()
    {
        m_foundInteractables.Clear();
        m_check.Result.ForEach(t =>
        {
            if (!t.TryGetComponent(out IInteractable interactable)) return;
            m_foundInteractables.Add(interactable);
        });
    }
}
