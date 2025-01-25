using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bubblethingy : MonoBehaviour
{
    public List<int> bubbles, unusedIndex;
    public List<GameObject> bubblesGO = new List<GameObject>();
    public int size = 10;
    public GameObject prefab;

    int leftIndex;
    int rightIndex;

    public TMPro.TextMeshProUGUI leftLabel, rightLabel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbles = new List<int>();
        unusedIndex = new List<int>();
        for (int i = 0; i < size; i++)
        {
            bubbles.Add(i * UnityEngine.Random.Range(1, 99));
            unusedIndex.Add(i);

        }
 
        leftIndex= Random.Range(0, bubbles.Count);
        rightIndex = Random.Range(0, bubbles.Count);
        if (rightIndex == leftIndex)
        {
            rightIndex = (rightIndex + 1) % bubbles.Count;
        }
        if (leftLabel)
            leftLabel.text = bubbles[leftIndex].ToString();
        if (rightLabel)
            rightLabel.text = bubbles[rightIndex].ToString();
        
    }
    public void CreateBubble()
    {
        var i = Random.Range(0, bubbles.Count);
        var x = Instantiate(prefab, transform.position + Vector3.one * Random.Range(-5f, 5f) + transform.forward * 5f, transform.rotation);
        x.transform.localScale = Vector3.one * bubbles[i];

       
    }


    void bubbleSort()
    {
        for (int i = 0; i < bubbles.Count - 1; i++)
        {
            if (bubbles[i] > bubbles[i + 1])
            {
                var tmp = bubbles[i+1];
                bubbles[i + 1] = bubbles[i];
                bubbles[i] = tmp;
                 }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
         //   for (int i = 0; i < bubbles.Count; i++)
            {

           //     bubbleSort();
            }
            onSelectLeft();
 
        }
        if (Input.GetMouseButtonDown(1))
        { onSelectRight(); }
    }

    public void onSelectLeft()
    {
        Debug.Log("left");


        

        Debug.Log(bubbles[leftIndex]);
        Debug.Log(bubbles[ rightIndex]);  
            Debug.Log(bubbles[leftIndex]>bubbles[rightIndex]?"ok!":"bubun");


        
            
    }

    public int getUnusedRandom()
    {
        return 0;
    }
    public void onSelectRight()
    {
        Debug.Log("right");
        Debug.Log(bubbles[leftIndex]);
        Debug.Log(bubbles[rightIndex]);
        {
            Debug.Log(bubbles[rightIndex] > bubbles[leftIndex] ? "ok" : "bubun");
        }

    }
}
