using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class CatScaler : MonoBehaviour {
	[SerializeField] private GameObject cat_;

	bool isFat = false;
	bool isTall = false;
	bool isLong = false;

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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		isFat = false;
		isTall = false;
		isLong = false;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			ResetScaling();
		}

		if (!isFat && Input.GetKeyDown(KeyCode.Alpha1)) {
			BodyScalingUpWidth();
			isFat = true;
		}
		if (!isTall && Input.GetKeyDown(KeyCode.Alpha2)) {
			BodyScalingUpHeight();
			isTall = true;
		}
		if (!isLong && Input.GetKeyDown(KeyCode.Alpha3)) {
			HandsScalingUpLength();
			isLong = true;
		}
	}

	void ResetScaling() {
		cat_.transform.Find("CatHands").gameObject.transform.localScale = Vector3.one;
		cat_.transform.Find("CatFoots").gameObject.transform.localScale = Vector3.one;
		foreach (Image childlen in cat_.transform.GetComponentsInChildren<Image>()) {
			childlen.transform.localScale = Vector3.one;
		}
		if (isFat) {
			cat_.transform.Find("CatHands/CatLeftHand").gameObject.transform.position -= FatLeftHandPos;
			cat_.transform.Find("CatHands/CatRightHand").gameObject.transform.position -= FatRightHandPos;
			cat_.transform.Find("CatTail").gameObject.transform.position -= FatTailPos;
			isFat = false;
		}
		if (isTall) {
			cat_.transform.Find("CatBody").gameObject.transform.position -= TallBodyPos;
			cat_.transform.Find("CatHands").gameObject.transform.position -= TallBodyHandsPos;
			cat_.transform.Find("CatHead").gameObject.transform.position -= TallBodyHeadPos;
			isTall = false;
		}
		if (isLong) {
			cat_.transform.Find("CatHands/CatLeftHand").gameObject.transform.position -= LongLeftHandPos;
			cat_.transform.Find("CatHands/CatRightHand").gameObject.transform.position -= LongRightHandPos;
			isLong = false;
		}
	}

	void BodyScalingUpWidth() {
		cat_.transform.Find("CatBody").gameObject.transform.localScale += FatBodyScale;
		cat_.transform.Find("CatHead").gameObject.transform.localScale += FatHeadScale;
		cat_.transform.Find("CatFoots").gameObject.transform.localScale += FatFootsScale;
		cat_.transform.Find("CatHands/CatLeftHand").gameObject.transform.position += FatLeftHandPos;
		cat_.transform.Find("CatHands/CatRightHand").gameObject.transform.position += FatRightHandPos;
		cat_.transform.Find("CatTail").gameObject.transform.position += FatTailPos;
	}

	void BodyScalingUpHeight() {
		GameObject bodyObj = cat_.transform.Find("CatBody").gameObject;
		bodyObj.transform.localScale += TallBodyScale;
		bodyObj.transform.position += TallBodyPos;
		cat_.transform.Find("CatHands").gameObject.transform.position += TallBodyHandsPos;
		cat_.transform.Find("CatHead").gameObject.transform.position += TallBodyHeadPos;
	}

	void HandsScalingUpLength() {
		GameObject leftHandsObj = cat_.transform.Find("CatHands/CatLeftHand").gameObject;
		GameObject rightHandsObj = cat_.transform.Find("CatHands/CatRightHand").gameObject;
		leftHandsObj.transform.localScale += LongHandsScale;
		rightHandsObj.transform.localScale += LongHandsScale;
		leftHandsObj.transform.position += LongLeftHandPos;
		rightHandsObj.transform.position += LongRightHandPos;
	}
}
