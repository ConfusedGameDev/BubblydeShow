using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
	public void StageSelect(int StageID)
	{
		// �V�[���؂�ւ�
		SceneManager.LoadScene(StageID);
	}
}
