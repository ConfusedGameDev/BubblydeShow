using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

    public TextMeshProUGUI label, timerLabel;

    bool isGameDone;
    public GameObject step;

    public List<GameObject> steps;
    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerUI.fillAmount = timer / timerDuration;
        if(timerLabel)
            timerLabel.text= Mathf.RoundToInt( timer).ToString();
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
        for (int i = 0; i < bubbles.Count; i++)
        {
            var startpos = transform.position - (Vector3.right * 8f) + (Vector3.fwd * 15f) - Vector3.up * 6f; ;
            var newStep = Instantiate(step, startpos + Vector3.right * i * 1.25f, Quaternion.identity);
            newStep.transform.localScale = Vector3.one + Vector3.up * bubbles[i];
            newStep.GetComponentInChildren<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            steps.Add(newStep);
        }
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
        Vector3 leftPos = new Vector3(-5, 0, 0);
        Vector3 rightPos = new Vector3(5, 0, 0);

        int rnd = UnityEngine.Random.Range(0, bubblePrefabs.Count);
        currentBubbleGO = Instantiate(bubblePrefabs[rnd], leftPos, Quaternion.identity);
        var currentBubbleScript = currentBubbleGO.GetComponent<Bubble>();
        currentBubbleScript.mapBubble(bubbles[currentIndex]);
        currentBubbleScript.offsetParent(leftPos * 40f);
        nextBubbleGO = Instantiate(bubblePrefabs[rnd], rightPos, Quaternion.identity);
        var nextBubbleScript = nextBubbleGO.GetComponent<Bubble>();
        nextBubbleScript.mapBubble(bubbles[nextIndex]);
        label.text = currentBubbleScript.mondai;
        nextBubbleScript.offsetParent(rightPos * 40f);
    }

    public void OnLeftClick()
    {
        if (isGameDone) return;
        if (bubbles[currentIndex] >= bubbles[nextIndex])
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
            StartCoroutine(animateSteps(0.5f));
            if (hasWon())
            {
                Win();
            }
            else
            {
                timer = timerDuration;
                RemoveCurrentFromUnused();
                GetNext();
                if (!isGameDone)
                    ShowComparisonBubbles();
            }

        }
        else
        {
            GameOver();
        }
    }

    public bool hasWon()
    {
        for (int x = 0;x < bubbles.Count-1;x++)
        {
            if (bubbles[x] > bubbles[x + 1])
                return false;
        }
        return true;
    }
    public void OnRightClick()
    {
        if (isGameDone) return;
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
            StartCoroutine(animateSteps(0.5f));
            if (hasWon())
            {
                Win();
            }
            else
            {
                timer = timerDuration;
                RemoveCurrentFromUnused();
                GetNext();
                if (!isGameDone)
                    ShowComparisonBubbles();
            }
        }
        else
        {
            GameOver();
        }
    }

    public IEnumerator animateSteps(float duration)
    {
        var delta = 0f;
        while (delta < duration)
        {
            yield return null;
            for (int x = 0; x < steps.Count; x++)
            {
                steps[x].transform.localScale = Vector3.Lerp(steps[x].transform.localScale, Vector3.one + Vector3.up * bubbles[x],Time.deltaTime);

            }
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
        nextIndex= currentIndex + 1;
    }

    void GameOver()
    {
        if(isGameDone) return;
        if (label)
            label.text = "ゲーム　オバー";
 
        StartCoroutine(onGameComplete());
        isGameDone= true;
    }

    void Win()
    {
        if (isGameDone) return;
        if (label)
            label.text = "バッブリ";

        StartCoroutine(onGameComplete());
        isGameDone = true;
    }



    IEnumerator onGameComplete()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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