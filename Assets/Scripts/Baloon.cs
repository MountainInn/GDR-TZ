using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem), typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Baloon : MonoBehaviour, IResetable
{
    Vector3 initialPosition;

    ParticleSystem ps;
    SpriteRenderer sr;

    [SerializeField]
    UnityEvent
        onCoinCollected,
        onBaloonPopingStart,
        onBaloonPopingEnd;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();

        initialPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        sr.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Spike"))
        {
            Pop();
            onBaloonPopingStart?.Invoke();
            Invoke("OnBaloonPopingEndInvoke", 1);
        }
        else if (collider.CompareTag("Coin"))
        {
            onCoinCollected?.Invoke();
            collider.GetComponent<Coin>().SetActive(false);
        }
    }

    void Pop()
    {
        ps.Play();
        sr.enabled = false;
    }

    void OnBaloonPopingEndInvoke()
    {
        onBaloonPopingEnd?.Invoke();
    }
}
