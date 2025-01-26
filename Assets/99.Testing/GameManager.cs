using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
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

    public AudioClip leftClip, rightClip, correctClip, wrongClip, clearClip;
    public AudioSource fxSource, bgmSource;

    public GameObject catDissapearTL;
    bool gameStarted = false;
    public TableReference tableReference;
    public TableEntryReference winReference, looseReference, questionReference, bubblyReference;

    public TMPro.TMP_FontAsset romajiAsset, nihongoAsset, thaiAsset, arabicAsset;

    public List<TMPro.TextMeshProUGUI> labels;

    public int valuesOffset = 2;

    public float leftOffset = -500, rightOffset = 140.3f;
    public float stepsSpacing = 2.25f;

    public GameObject replayIcon, gameOverLabel, gameClearLabel, gameOverVideo, gameDescriptorLabel;
    void setFont(string locale)
    {
        labels = FindObjectsByType<TMPro.TextMeshProUGUI>(FindObjectsSortMode.None).ToList();
        switch (locale)
        {
            case "Japanese (ja)":
                foreach (var label in labels)
                {
                    label.font = nihongoAsset;
                }
                break;
            case "Thai (th)":
                foreach (var label in labels)
                {
                    label.font = thaiAsset;
                }
                break;
            case "Arabic (ar)":
                foreach (var label in labels)
                {
                    label.font = arabicAsset;
                }
                break;
            default:
                foreach (var label in labels)
                {
                    label.font = arabicAsset;
                }
                break;


        }

    }
    IEnumerator Start()
    {
        InitializeGame();
        timer = 3f;
        // setFont(LocalizationSettings.SelectedLocale.LocaleName);

        while (timer > 0.1f)
        {
            //label.text = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference, winReference);

            timer -= Time.deltaTime;
            timerUI.fillAmount = 0;
            timerLabel.text = Mathf.CeilToInt(timer).ToString();
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        timerLabel.text = "";
        gameStarted = true;
        timer = timerDuration;
        ShowComparisonBubbles();

        if (gameDescriptorLabel)
            gameDescriptorLabel.SetActive(true);

    }

    void Update()
    {
        if (!gameStarted)
        {
            return;
        }
        timer -= Time.deltaTime;
        timerUI.fillAmount = timer / timerDuration;
        if (timerLabel)
            timerLabel.text = Mathf.RoundToInt(timer).ToString();
        if (timer <= 0)
        {
            GameOver();
        }

        //delete
         

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
            originalBubbles.Add(bubbles[i]);
            var startpos = transform.position - (Vector3.right * 14f) + (Vector3.fwd * 15f) - Vector3.up * 7.5f + Vector3.right * i * stepsSpacing + stepOffset;
            var newCat = Instantiate(catStep, startpos, Quaternion.identity).GetComponent<CatRigController>();
            newCat.initialPos = startpos;
            newCat.rootOffset = Vector3.up * bubbles[i];
            newCat.offset = bubbles[i];
            cats.Add(newCat);
        }
    }
    public Vector3 stepOffset;
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
        currentBubbleScript.mapBubble(bubbles[currentIndex] * valuesOffset);
        currentBubbleScript.offsetParent(Vector3.right * leftOffset);
        currentBubbleScript.upScaleParent(1.2f);
        nextBubbleGO = Instantiate(bubblePrefabs[rnd], rightPos, Quaternion.identity);
        var nextBubbleScript = nextBubbleGO.GetComponent<Bubble>();
        nextBubbleScript.mapBubble(bubbles[nextIndex] * valuesOffset);
        label.text = currentBubbleScript.mondai;
        nextBubbleScript.offsetParent(Vector3.right * rightOffset);
        nextBubbleScript.upScaleParent(1.2f);
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
            if (fxSource && correctClip)
            {
                fxSource.Stop();
                fxSource.PlayOneShot(correctClip);
            }
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
            StartCoroutine(animateSteps(0.5f, bubbles));
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
            if (fxSource && wrongClip)
            {
                fxSource.Stop();
                fxSource.PlayOneShot(wrongClip);
            }
            GameOver();
        }
    }

    public bool hasWon()
    {
        for (int x = 0; x < bubbles.Count - 1; x++)
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
            if (fxSource && correctClip)
            {
                fxSource.Stop();
                fxSource.PlayOneShot(correctClip);
            }
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
            StartCoroutine(animateSteps(0.5f, bubbles));
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
            if (fxSource && wrongClip)
            {
                fxSource.Stop();
                fxSource.PlayOneShot(wrongClip);
            }
            GameOver();
        }
    }

    public IEnumerator AnimateBubblesort()
    {
        if (fxSource && clearClip)
        {
            fxSource.PlayOneShot(clearClip);
        }

        for (int j = 0; j < originalBubbles.Count; j++)
        {
            for (int i = 0; i < originalBubbles.Count - 1; i++)
            {
                if (originalBubbles[i] > originalBubbles[i + 1])
                {
                    var tmp = originalBubbles[i + 1];
                    originalBubbles[i + 1] = originalBubbles[i];
                    originalBubbles[i] = tmp;
                    yield return animateSteps(0.15f, originalBubbles);
                }


            }
        }




    }
    public IEnumerator animateSteps(float duration, List<int> data)
    {
        var delta = 0f;
        List<float> originalOffsets = new List<float>();
        List<Vector3> originalRootOffsets = new List<Vector3>();
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
                var newCat = cats[x];
                newCat.rootOffset = Vector3.Lerp(originalRootOffsets[x], Vector3.up * data[x], delta / duration);
                newCat.offset = Mathf.Lerp(originalOffsets[x], data[x], delta / duration);
                delta += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public IEnumerator animateStepsEnding(int index)
    {
        var duration = UnityEngine.Random.Range(0.5f, 1f);
        var delta = 0f;
        float originalOffset = cats[index].offset;


        while (delta < duration)
        {

            var newCat = cats[index];
            newCat.offset = Mathf.Lerp(originalOffset, 0, delta / duration);
            delta += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (catDissapearTL)
        {
            Instantiate(catDissapearTL, cats[index].rootBone.position, Quaternion.identity);
            cats[index].gameObject.SetActive(false);

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
        nextIndex = currentIndex + 1;
    }

    IEnumerator stopMusic()
    {
        while (bgmSource.volume > 0)
        {

            bgmSource.volume = Mathf.Lerp(bgmSource.volume, 0f, Time.deltaTime);
            yield return null;
        }
    }
        void GameOver()
    {
        if(isGameDone) return;

        if (bgmSource != null)
        {
            StartCoroutine(stopMusic());
        }
        if (gameDescriptorLabel)
            gameDescriptorLabel.SetActive(false);
        if (gameOverLabel)
            gameOverLabel.SetActive(true);

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
        if (gameDescriptorLabel)
            gameDescriptorLabel.SetActive(false);
        if (gameClearLabel)
            gameClearLabel.SetActive(true);
        if (replayIcon)
            replayIcon.SetActive(true);
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
            
        
        yield return new WaitForSeconds(2f);
        if (gameOverVideo)
            gameOverVideo.SetActive(true);
        yield return new WaitForSeconds(1.3f);
            if(gameOverLabel)
            gameOverLabel.SetActive(false);
        yield return new WaitForSeconds(2.3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void OnLeftClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnLeftClick();
            if(fxSource && leftClip && rightClip)
            {
                fxSource.Stop();
                fxSource.PlayOneShot(leftClip);
            }
        }
    }

    public void OnRightClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (fxSource && leftClip && rightClip)
            {
                fxSource.Stop();
                fxSource.PlayOneShot(rightClip);
            }
            OnRightClick();
        }
    }
}