using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CatDeforming : MonoBehaviour {
	public Sprite[] sprites_;
	private Image image_;
	private int count_ = 0;
	private const float INTERVAL = 1f / 24f;

	void Awake() {
		image_ = GetComponent<Image>();
		StartCoroutine(DeformingAnim());
	}

	IEnumerator DeformingAnim() {
		image_.sprite = sprites_[count_++];
		yield return new WaitForSeconds(INTERVAL);
		if (count_ < sprites_.Length) StartCoroutine("DeformingAnim");
		else Destroy(this.gameObject);
	}
}
