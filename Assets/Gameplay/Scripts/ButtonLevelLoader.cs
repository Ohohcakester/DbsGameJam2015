using UnityEngine;
using System.Collections;

public class ButtonLevelLoader : MonoBehaviour {
	public string levelName;

	void OnMouseDown(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (levelName);
	//	Application.LoadL
	}
}
