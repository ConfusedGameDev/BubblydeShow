using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
	public void StageSelect(int StageID)
	{
		// シーン切り替え
		SceneManager.LoadScene(StageID);
	}
}
