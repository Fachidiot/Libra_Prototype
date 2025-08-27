using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Card : MonoBehaviour
{
    [SerializeField] private int ownNumber;
    [SerializeField] private int pairNumber;

    [SerializeField] private Color backColor = Color.black;
    [SerializeField] private Color ownColor = Color.black;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite ownSprite;

    private CardGameManager manager;
    private SpriteRendererEventListener eventListener;
    private SpriteRenderer sr;
    private bool isFliped = false;
    public bool IsFliped { set { isFliped = value; } get { return isFliped; } }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        eventListener = GetComponent<SpriteRendererEventListener>();
        manager = gameObject.GetComponentInParent<CardGameManager>();
    }

    public void SetupCard(int own, Sprite back, Sprite front)
    {
        ownNumber = own;
        eventListener.GetLeftClickEvent().AddListener(FlipCard);
        ownSprite = front;
        backSprite = back;

        sr.sprite = isFliped ? ownSprite : backSprite;
        sr.color = isFliped ? ownColor : backColor;
    }

    public void SetupPair(int pair, Color color)
    {
        pairNumber = pair;
        ownColor = color;
    }

    public int GetOwnNum()
    {
        return ownNumber;
    }

    public int GetPairNum()
    {
        return pairNumber;
    }

    public void MatchedCard()
    {
        eventListener.GetLeftClickEvent().RemoveAllListeners();
    }

    public void FlipCard()
    {
        if (manager.IsChecking)
            return;
        isFliped = !isFliped;

        if (ownSprite && backSprite)
            sr.sprite = isFliped ? ownSprite : backSprite;
        sr.color = isFliped ? ownColor : backColor;
        manager.CardHasFlipped(this);
    }
}
