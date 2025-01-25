using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Control_CatScaler_Linear : MonoBehaviour
{
    [SerializeField] private GameObject cat_;

    [SerializeField, Range(0.0f, 5.0f)] private float fatness = 0.0f;
    [SerializeField, Range(0.0f, 5.0f)] private float tallness = 0.0f;
    [SerializeField, Range(0.0f, 5.0f)] private float longness = 0.0f;

    private Vector3 BaseBodyScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 BasePosition = Vector3.zero;

    private Vector3 FatBodyScale { get => new(0.5f, 0, 0); }
    private Vector3 FatHeadScale { get => new(0.5f, 0, 0); }
    private Vector3 FatFootsScale { get => new(0.5f, 0, 0); }
    private Vector3 FatLeftHandPos { get => new(-30, 0, 0); }
    private Vector3 FatRightHandPos { get => new(45, 0, 0); }
    private Vector3 FatTailPos { get => new(-30, 0, 0); }

    private Vector3 TallBodyScale { get => new(0, 1, 0); }
    private Vector3 TallBodyPos { get => new(0, 100, 0); }
    private Vector3 TallBodyHandsPos { get => new(0, 130, 0); }
    private Vector3 TallBodyHeadPos { get => new(0, 120, 0); }

    private Vector3 LongHandsScale { get => new(4, 0, 0); }
    private Vector3 LongLeftHandPos { get => new(290, 0, 0); }
    private Vector3 LongRightHandPos { get => new(-345, 0, 0); }

    void Update()
    {
        UpdateCatScaling();
    }

    void UpdateCatScaling()
    {
        // Reset scales and positions to base values
        ResetScaling();

        // Apply fatness scaling
        cat_.transform.Find("CatBody").gameObject.transform.localScale = BaseBodyScale + fatness * FatBodyScale;
        cat_.transform.Find("CatHead").gameObject.transform.localScale = BaseBodyScale + fatness * FatHeadScale;
        cat_.transform.Find("CatFoots").gameObject.transform.localScale = BaseBodyScale + fatness * FatFootsScale;
        cat_.transform.Find("CatHands/CatLeftHand").gameObject.transform.localPosition = BasePosition + fatness * FatLeftHandPos;
        cat_.transform.Find("CatHands/CatRightHand").gameObject.transform.localPosition = BasePosition + fatness * FatRightHandPos;
        cat_.transform.Find("CatTail").gameObject.transform.localPosition = BasePosition + fatness * FatTailPos;

        // Apply tallness scaling
        GameObject bodyObj = cat_.transform.Find("CatBody").gameObject;
        bodyObj.transform.localScale += tallness * TallBodyScale;
        bodyObj.transform.localPosition = BasePosition + tallness * TallBodyPos;
        cat_.transform.Find("CatHands").gameObject.transform.localPosition = BasePosition + tallness * TallBodyHandsPos;
        cat_.transform.Find("CatHead").gameObject.transform.localPosition = BasePosition + tallness * TallBodyHeadPos;

        // Apply longness scaling
        GameObject leftHandsObj = cat_.transform.Find("CatHands/CatLeftHand").gameObject;
        GameObject rightHandsObj = cat_.transform.Find("CatHands/CatRightHand").gameObject;
        leftHandsObj.transform.localScale = BaseBodyScale + longness * LongHandsScale;
        rightHandsObj.transform.localScale = BaseBodyScale + longness * LongHandsScale;
        leftHandsObj.transform.localPosition += longness * LongLeftHandPos;
        rightHandsObj.transform.localPosition += longness * LongRightHandPos;
    }

    void ResetScaling()
    {
        cat_.transform.Find("CatHands").gameObject.transform.localScale = Vector3.one;
        cat_.transform.Find("CatFoots").gameObject.transform.localScale = Vector3.one;
        foreach (Image childlen in cat_.transform.GetComponentsInChildren<Image>())
        {
            childlen.transform.localScale = Vector3.one;
        }

        cat_.transform.Find("CatHands/CatLeftHand").gameObject.transform.localPosition = BasePosition;
        cat_.transform.Find("CatHands/CatRightHand").gameObject.transform.localPosition = BasePosition;
        cat_.transform.Find("CatTail").gameObject.transform.localPosition = BasePosition;
        cat_.transform.Find("CatBody").gameObject.transform.localPosition = BasePosition;
        cat_.transform.Find("CatHands").gameObject.transform.localPosition = BasePosition;
        cat_.transform.Find("CatHead").gameObject.transform.localPosition = BasePosition;
    }
}
