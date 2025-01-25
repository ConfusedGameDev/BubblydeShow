using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
	public void StageSelect(int StageID)
	{
		// ƒV[ƒ“Ø‚è‘Ö‚¦
		SceneManager.LoadScene(StageID);
	}
}
