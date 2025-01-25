using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_CatGenerator : MonoBehaviour
{
    public class Cat
    {
        public int Index; 
        public float ScaleX; 
        public float HeadScaleY; 
        public float FrontLegsScaleY;
        public float BellyScaleY;
        public float BackLegsScaleY;
        public float TailScaleY; 
    }

    public int numberOfCats = 5;
    public float baseValue = 1.0f;
    public float randomMin = 0.5f;
    public float randomMax = 1.5f;

    private List<Cat> cats = new List<Cat>();

    void Update()
    {
        // Space key to generate
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCats();
            PrintCats();
        }
    }

    void GenerateCats()
    {
        cats.Clear();

        for (int i = 0; i < numberOfCats; i++)
        {
            Cat newCat = new Cat();
            newCat.Index = i;
            newCat.ScaleX = Random.Range(randomMin, randomMax) * baseValue;
            newCat.HeadScaleY = Random.Range(randomMin, randomMax) * baseValue;
            newCat.FrontLegsScaleY = Random.Range(randomMin, randomMax) * baseValue;
            newCat.BellyScaleY = Random.Range(randomMin, randomMax) * baseValue;
            newCat.BackLegsScaleY = Random.Range(randomMin, randomMax) * baseValue;
            newCat.TailScaleY = Random.Range(randomMin, randomMax) * baseValue;

            cats.Add(newCat);
        }
    }

    void PrintCats()
    {
        foreach (Cat cat in cats)
        {
            Debug.Log($"Cat {cat.Index}: ScaleX={cat.ScaleX}, HeadScaleY={cat.HeadScaleY}, FrontLegsScaleY={cat.FrontLegsScaleY}, BellyScaleY={cat.BellyScaleY}, BackLegsScaleY={cat.BackLegsScaleY}, TailScaleY={cat.TailScaleY}");
        }
    }
}
