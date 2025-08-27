using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.ComponentModel.Design.Serialization;

public class CardGameManager : MonoBehaviour
{
    [Header("Init Settings")]
    [SerializeField] private int cardCount = 6;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<Card> cards = new List<Card>();
    private List<Card> flippedCards = new List<Card>();
    [SerializeField] private CardSpriteDatabase cardSpriteDatabase;
    [SerializeField] private CardColorDatabase cardColorDatabase;
    [SerializeField] private GameObject axisObject;
    [SerializeField] private float cardDisplaySize = 2f;
    [SerializeField] private float cardLineLength;

    [Header("Spin Settings")]
    [SerializeField] private bool hardMode = false;
    [SerializeField] private float maxSpinSpeed = 720f;
    [SerializeField] private float spinDeceleration = 200f;
    private float currentSpinSpeed;
    private bool isSpinning = false;

    [Header("Card Match Settings")]
    [SerializeField] private float calculateTime = 1f;
    private bool isChecking = false;
    public bool IsChecking {get{return isChecking;}}

    void Start()
    {
        InitCards();
        SpinTable();
    }

    #region INITIALIZE_METHODS

    private void GenerateCards()
    {
        foreach (Card child in cards)
            Destroy(child.gameObject);
        cards.Clear();

        for (int i = 0; i < cardCount; ++i)
        {
            GameObject cardObj = Instantiate(cardPrefab, axisObject.transform);
            cardObj.name = "Card_" + (i + 1);

            Card newCard = cardObj.GetComponent<Card>();
            if (null != newCard)
                cards.Add(newCard);
        }
    }
    public void InitCards()
    {
        matchedCardCount = 0;
        // 카드 생성
        GenerateCards();

        // 카드 위치 설정
        ClaculateCardPos();

        // 카드 번호 부여
        for (int i = 0; i < cards.Count; ++i)
        {
            cards[i].SetupCard(i, cardSpriteDatabase.backSprite, cardSpriteDatabase.sprites[i]);
        }

        // 카드 페어 번호 랜덤 부여
        List<int> indices = new List<int>();
        for (int i = 0; i < cards.Count; ++i)
            indices.Add(i);

        for (int i = 0; i < indices.Count; ++i)
        {
            int temp = indices[i];
            int rand = Random.Range(i, indices.Count);
            indices[i] = indices[rand];
            indices[rand] = temp;
        }

        for (int i = 0; i < cards.Count; i += 2)
        {
            int cardIndex1 = indices[i];
            int cardIndex2 = indices[i + 1];

            cards[cardIndex1].SetupPair(cardIndex2, cardColorDatabase.colors[i]);
            cards[cardIndex2].SetupPair(cardIndex1, cardColorDatabase.colors[i]);
        }
    }
    private void ClaculateCardPos()
    {
        // 모든 카드를 보기 좋게 배치하기 위한 원의 둘레 계산
        float circumference = cards.Count * cardDisplaySize;
        // 원의 반지름 계산
        float dynamicRadius = circumference / (2f * Mathf.PI);

        // 인스펙터에서 설정한 최소 반지름(cardLineLength)과 동적 반지름 중 더 큰 값을 사용
        float radiusToUse = Mathf.Max(dynamicRadius, cardLineLength);
        // 카드 개수에 맞춰 각도를 계산
        float angleIncrement = 360f / cards.Count;

        for (int i = 0; i < cards.Count; ++i)
        {
            float angle = i * angleIncrement;
            float radiusinRadians = Mathf.Deg2Rad * angle;

            float x = radiusToUse * Mathf.Cos(radiusinRadians);
            float y = radiusToUse * Mathf.Sin(radiusinRadians);

            cards[i].gameObject.transform.localPosition = new Vector3(x, y, 10);
        }
    }
    #endregion


    #region INSPECTOR_PUBLIC METHODS
    public void SpinTable()
    {
        if (!isSpinning)
            StartCoroutine(SpinCoroutine());
    }

    public void CardHasFlipped(Card card)
    {
        // 이미 뒤집혔거나 검사중이라면 false
        if (isChecking || !card.IsFliped || flippedCards.Contains(card))
            return;

        flippedCards.Add(card);

        if (2 == flippedCards.Count)
        {
            isChecking = true;
            StartCoroutine(CheckMatchCoroutine());
        }
    }
    #endregion

    #region COROUTINE METODS
    private int matchedCardCount = 0;
    private IEnumerator CheckMatchCoroutine()
    {
        yield return new WaitForSeconds(calculateTime);

        Card card1 = flippedCards[0];
        Card card2 = flippedCards[1];

        flippedCards.Clear();
        isChecking = false;

        if (card1.GetPairNum() == card2.GetOwnNum() && card1.GetOwnNum() == card2.GetPairNum())
        {
            Debug.Log("짝 맞추기 성공");
            card1.MatchedCard();
            card2.MatchedCard();
            ++matchedCardCount;
        }
        else
        {
            Debug.Log("짝 맞추기 실패");
            card1.FlipCard();
            card2.FlipCard();

            // Life -1

            if (hardMode)
                SpinTable();
        }

        if ((cardCount / 2) <= matchedCardCount)
        {   // Game Clear
            Debug.Log("Game Clear");
            StageManager.Instance.ChangeStage(3);
        }
    }

    private IEnumerator SpinCoroutine()
    {
        isSpinning = true;
        currentSpinSpeed = maxSpinSpeed;

        while (currentSpinSpeed > 0)
        {
            axisObject.transform.Rotate(0, 0, currentSpinSpeed * Time.deltaTime);
            Quaternion axisRotation = axisObject.transform.rotation;

            // 카드 보정
            foreach (Card card in cards)
                card.gameObject.transform.localRotation = Quaternion.Inverse(axisRotation);

            // 회전 감소
            currentSpinSpeed -= spinDeceleration * Time.deltaTime;

            yield return null;
        }

        currentSpinSpeed = 0;
        isSpinning = false;
    }
    #endregion
}
