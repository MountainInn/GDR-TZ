using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]
public class Coin: MonoBehaviour, IResetable
{
    SpriteRenderer sr;
    CircleCollider2D coll;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<CircleCollider2D>();
    }

    public void Reset()
    {
        SetActive(true);
    }

    public void SetActive(bool isActive)
    {
        coll.enabled = sr.enabled = isActive;
    }
}
