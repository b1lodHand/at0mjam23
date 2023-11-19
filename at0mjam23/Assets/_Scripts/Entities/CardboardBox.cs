using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardBox : MonoBehaviour, IInteractable
{
    // Fields.
    [SerializeField] private Transform m_hidePosition;

    // Properties.
    public Transform HidePosition => m_hidePosition;

    public void Interact(params object[] data)
    {
        var player = (Player)data[1];
        if(!player.IsHidden)
        {
            player.Hide(this);
        }
    }
}
