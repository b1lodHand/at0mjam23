using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandle : MonoBehaviour
{
    // Fields.
    [SerializeField] private DistractorBehaviour m_distractorPrefab;
    [SerializeField] private GameObject m_handledDistractor;
    [SerializeField] private bool m_handlingDistractor = false;

    // Properties.
    public bool HandlingDistractor => m_handlingDistractor;

    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.G) && HandlingDistractor) DropDistractor();
    }

    public bool HandleDistractor()
    {
        if (HandlingDistractor) return false;

        m_handlingDistractor = true;
        m_handledDistractor.SetActive(true);
        return true;
    }

    public void DropDistractor()
    {
        if(!HandlingDistractor) return;

        m_handlingDistractor = false;
        Instantiate(m_distractorPrefab, transform.position, Quaternion.identity);
        m_handledDistractor.SetActive(false);
    }
}
