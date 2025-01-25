using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Bubble Data")]
    public List<int> bubbles = new List<int>();

    [Header("Prefabs & UI")]
    public List<GameObject> bubblePrefabs;
    public Image timerUI;

    [Header("Game Settings")]
    public float timerDuration = 5f;
    public int numberOfValues = 10;

    private List<int> unusedBubbles;
    private int sortedItems;
    private int currentIndex;
    private int nextIndex;
    private float timer;
    private System.Random rnd = new System.Random();
    private GameObject currentBubbleGO;
    private GameObject nextBubbleGO;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerUI.fillAmount = timer / timerDuration;
        if (timer <= 0)
        {
            GameOver();
        }
    }

    void InitializeGame()
    {
        bubbles = Enumerable.Range(1, numberOfValues).ToList();
        bubbles = bubbles.OrderBy(x => rnd.Next()).ToList();
        sortedItems = 0;
        currentIndex = 0;
        timer = timerDuration;
        ResetUnusedList();
        GetNext();
        ShowComparisonBubbles();
    }

    void ResetUnusedList()
    {
        int countUnsorted = bubbles.Count - sortedItems;
        if (countUnsorted < 1) return;
        unusedBubbles = bubbles.Take(countUnsorted).OrderBy(x => rnd.Next()).ToList();
    }

    void ShowComparisonBubbles()
    {
        if (currentBubbleGO) Destroy(currentBubbleGO);
        if (nextBubbleGO) Destroy(nextBubbleGO);
        Vector3 leftPos = new Vector3(-2, 0, 0);
        Vector3 rightPos = new Vector3(2, 0, 0);
    
        System.Random rnd = new System.Random();
        currentBubbleGO = Instantiate(bubblePrefabs[rnd.Next()], leftPos, Quaternion.identity);
        var currentBubbleScript = currentBubbleGO.GetComponent<Bubble>();
        currentBubbleScript.mapBubble(bubbles[currentIndex]);
        nextBubbleGO = Instantiate(bubblePrefabs[rnd.Next()],rightPos, Quaternion.identity);
        var nextBubbleScript = nextBubbleGO.GetComponent<Bubble>();
        nextBubbleScript.mapBubble(bubbles[nextIndex]);
    }

    public void OnLeftClick()
    {
        if (bubbles[currentIndex] > bubbles[nextIndex])
        {
            int temp = bubbles[nextIndex];
            bubbles[nextIndex] = bubbles[currentIndex];
            bubbles[currentIndex] = temp;
            currentIndex = nextIndex;
            if (currentIndex == bubbles.Count - sortedItems - 1)
            {
                sortedItems++;
                if (sortedItems >= bubbles.Count)
                {
                    Win();
                    return;
                }
                ResetUnusedList();
                currentIndex = 0;
            }
            timer = timerDuration;
            RemoveCurrentFromUnused();
            GetNext();
            ShowComparisonBubbles();
        }
        else
        {
            GameOver();
        }
    }

    public void OnRightClick()
    {
        if (bubbles[currentIndex] <= bubbles[nextIndex])
        {
            currentIndex = nextIndex;
            if (currentIndex == bubbles.Count - sortedItems - 1)
            {
                sortedItems++;
                if (sortedItems >= bubbles.Count)
                {
                    Win();
                    return;
                }
                ResetUnusedList();
                currentIndex = 0;
            }
            timer = timerDuration;
            RemoveCurrentFromUnused();
            GetNext();
            ShowComparisonBubbles();
        }
        else
        {
            GameOver();
        }
    }

    void RemoveCurrentFromUnused()
    {
        if (unusedBubbles == null) return;
        if (unusedBubbles.Contains(bubbles[currentIndex]))
        {
            unusedBubbles.Remove(bubbles[currentIndex]);
        }
    }

    void GetNext()
    {
        if (unusedBubbles == null || unusedBubbles.Count == 0)
        {
            ResetUnusedList();
            if (unusedBubbles == null || unusedBubbles.Count == 0) return;
        }
        int nextVal = unusedBubbles[0];
        unusedBubbles.RemoveAt(0);
        nextIndex = bubbles.IndexOf(nextVal);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }

    void Win()
    {
        Debug.Log("You Win!");
    }

    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnLeftClick();
        }
    }

    public void OnRightClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRightClick();
        }
    }
}