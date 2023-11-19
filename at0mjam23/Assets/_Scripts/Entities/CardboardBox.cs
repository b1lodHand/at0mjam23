using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardBox : MonoBehaviour, IInteractable
{
    // Fields.
    [SerializeField] private Transform m_hidePosition;
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Sprite m_open, m_close;
    [SerializeField] private AudioClip m_openClip;

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

    public void Open()
    {
        m_renderer.sprite = m_open;
        AudioSource.PlayClipAtPoint(m_openClip, transform.position, .5f);
    }

    public void Close()
    {
        m_renderer.sprite = m_close;
        AudioSource.PlayClipAtPoint(m_openClip, transform.position, .5f);
    }
}
