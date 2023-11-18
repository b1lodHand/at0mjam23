using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignalGateType
{
    Single_Normal,
    Single_Negate,
    Duo_And,
    Duo_Xor,
}

public class SignalGate : MonoBehaviour
{
    // Fields.
    [SerializeField] private SignalGateType m_gateType = SignalGateType.Single_Normal;
    [SerializeField] private SignalGenerator m_generatorA;
    [SerializeField] private SignalGenerator m_generatorB;
    [SerializeField] private List<SignalListener> m_listeners = new List<SignalListener>();

    // Properties.
    public SignalGateType GateType => m_gateType;

    private void Awake()
    {
        if (m_generatorA != null)
        {
            m_generatorA.SetController(this);
            m_generatorA.OnSignalChanged += ApplyGateLogic;
        }

        if(m_generatorB != null)
        {
            m_generatorB.SetController(this);
            m_generatorB.OnSignalChanged += ApplyGateLogic;
        }

        m_listeners.ForEach(l => l.SetController(this));
    }

    private void Start()
    {
        //ApplyGateLogic();
    }

    private void OnValidate()
    {
        if(m_gateType == SignalGateType.Single_Normal || m_gateType == SignalGateType.Single_Negate) m_generatorB = null;
    }

    void ApplyGateLogic()
    {
        switch (m_gateType)
        {
            case SignalGateType.Single_Normal:
                m_listeners.ForEach(l => l.ListenToSignal(m_generatorA.SignalState));
                break;
            case SignalGateType.Single_Negate:
                m_listeners.ForEach(l => l.ListenToSignal(!m_generatorA.SignalState));
                break;
            case SignalGateType.Duo_And:
                var andResult = (m_generatorA.SignalState && m_generatorB.SignalState);
                m_listeners.ForEach(l => l.ListenToSignal(andResult));
                break;
            case SignalGateType.Duo_Xor:
                var xorResult = (m_generatorA.SignalState != m_generatorB.SignalState);
                m_listeners.ForEach(l => l.ListenToSignal(xorResult));
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .5f);

        if (m_generatorA != null)
        {
            Gizmos.DrawSphere(m_generatorA.transform.position, .3f);
            Gizmos.DrawLine(m_generatorA.transform.position, transform.position);
        }

        if (m_generatorB != null)
        {
            Gizmos.DrawSphere(m_generatorB.transform.position, .3f);
            Gizmos.DrawLine(m_generatorB.transform.position, transform.position);
        }

        if (m_listeners.Count == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < m_listeners.Count; i++)
        {
            Gizmos.DrawSphere(m_listeners[i].transform.position, .2f);
            Gizmos.DrawLine(m_listeners[i].transform.position, transform.position);
        }
    }
}