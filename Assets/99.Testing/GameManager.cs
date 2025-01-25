using NUnit.Framework;
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
    public List<int> originalBubbles = new List<int>();

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
    public List<CatRigController> cats;
    public GameObject catStep;

    public AudioClip leftClip, rightClip;
    public AudioSource source;

    public GameObject bubbleExplosion;
    bool gameStarted=false;
    IEnumerator Start()
    {
        InitializeGame();
        timer = 3f;
        while(timer > 0.1f)
        {
            timer-=Time.deltaTime;
            timerUI.fillAmount = 0;
            timerLabel.text = Mathf.CeilToInt(timer).ToString() ;
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        timerLabel.text = "";
        gameStarted = true;
        timer = timerDuration;
        ShowComparisonBubbles();

    }

    void Update()
    {
        if (!gameStarted)
        {
            return;
        }
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
        
        for (int i = 0; i < bubbles.Count; i++)
        {
            originalBubbles.Add( bubbles[i]);
            var startpos = transform.position - (Vector3.right * 8f) + (Vector3.fwd * 15f) - Vector3.up * 6f + Vector3.right * i * 2.25f;           
            var newCat= Instantiate(catStep,startpos, Quaternion.identity).GetComponent<CatRigController>();
            newCat.initialPos = startpos ;
            newCat.rootOffset= Vector3.up*bubbles[i];
            newCat.offset= bubbles[i];
            cats.Add(newCat);
        }
    }

    void ResetUnusedList()
    {
        int countUnsorted = bubbles.Count - sortedItems;
        if (countUnsorted < 1) return;
        unusedBubbles = bubbles.Take(countUnsorted).OrderBy(x => rnd.Next()).ToList();
    }

    void bubbleSort()
    {
        for (int i = 0; i < originalBubbles.Count - 1; i++)
        {
            if (originalBubbles[i] > originalBubbles[i + 1])
            {
                var tmp = originalBubbles[i + 1];
                originalBubbles[i + 1] = originalBubbles[i];
                originalBubbles[i] = tmp;
            }
        }
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
        if (!gameStarted)
        {
            return;
        }
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
            StartCoroutine(animateSteps(0.5f,bubbles));
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
        if (!gameStarted)
        {
            return;
        }
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
            StartCoroutine(animateSteps(0.5f,bubbles));
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

    public IEnumerator AnimateBubblesort()
    {
        for (int j   = 0; j < originalBubbles.Count; j++)
        {
            for (int i = 0; i < originalBubbles.Count - 1; i++)
            {
                if (originalBubbles[i] > originalBubbles[i + 1])
                {
                    var tmp = originalBubbles[i + 1];
                    originalBubbles[i + 1] = originalBubbles[i];
                    originalBubbles[i] = tmp;
                    yield return animateSteps(0.35f,originalBubbles);
                }
 

            }
         }

        


    }
    public IEnumerator animateSteps(float duration, List<int> data)
    {
        var delta = 0f;
        List<float> originalOffsets= new List<float>();
        List<Vector3> originalRootOffsets= new List<Vector3>();
        for (int x = 0; x < cats.Count; x++)
        {
            originalOffsets.Add(cats[x].offset);
            originalRootOffsets.Add(cats[x].rootOffset);
        }
        while (delta < duration)
        {
            
            for (int x = 0; x < cats.Count; x++)
            {
                // steps[x].transform.localScale = Vector3.Lerp(steps[x].transform.localScale, Vector3.one + Vector3.up * bubbles[x],Time.deltaTime);
                var newCat= cats[x];
                newCat.rootOffset = Vector3.Lerp(originalRootOffsets[x], Vector3.up * data[x],delta/duration);
                newCat.offset = Mathf.Lerp(originalOffsets[x], data[x], delta/duration );
                delta += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public IEnumerator animateStepsEnding(int index)
    {
        var duration = cats[index].offset/ UnityEngine.Random.Range(0.5f, 2.5f);
        var delta = 0f;
        float originalOffset= cats[index].offset;
          
         
        while (delta < duration)
        {

            var newCat = cats[index];
            newCat.offset = Mathf.Lerp(originalOffset, 0, delta / duration);
            delta += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(bubbleExplosion)
        {
            cats[index].gameObject.SetActive(false);
            Destroy( Instantiate(bubbleExplosion, cats[index].rootBone.position, Quaternion.identity),3f);
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

        if (timerUI)
        {
            timerUI.gameObject.SetActive(false);
        }
        if (timerLabel)
        {
            timerLabel.gameObject.SetActive(false);
        }
        if(currentBubbleGO)
            Destroy(currentBubbleGO);
        if (nextBubbleGO)
            Destroy(nextBubbleGO);
        {
            
        }
        if (label)
            label.text = "ゲーム　オバー";
 
        isGameDone= true;
        for (int j = 0; j < cats.Count; j++)
        {
            StartCoroutine(animateStepsEnding(j));
        }


        StartCoroutine(onGameComplete());


    }

    void Win()
    {
        if (isGameDone) return;
        StartCoroutine(AnimateBubblesort());
        if (timerUI)
        {
            timerUI.gameObject.SetActive(false);
        }
        if (timerLabel)
        {
            timerLabel.gameObject.SetActive(false);
        }
        if (currentBubbleGO)
            Destroy(currentBubbleGO);
        if (nextBubbleGO)
            Destroy(nextBubbleGO);
         
        if (label)
            label.text = "バッブリ";
        
         isGameDone = true;
    }



    IEnumerator onGameComplete()
    {
         
        yield return new WaitForSeconds(6f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnLeftClick();
            if(source && leftClip && rightClip)
            {
                source.Stop();
                source.PlayOneShot(leftClip);
            }
        }
    }

    public void OnRightClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (source && leftClip && rightClip)
            {
                source.Stop();
                source.PlayOneShot(rightClip);
            }
            OnRightClick();
        }
    }
}