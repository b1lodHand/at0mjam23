using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalGenerator m_generator;
    [SerializeField] private List<PressButton> m_otherButtonsConnected;
}
