using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	[SerializeField]
	string mouseHoverSound = "ButtonHover";

	[SerializeField]
	string buttonPressSound = "ButtonPress";

	AudioManager audioManager;

	void Start()
    {
		audioManager = AudioManager.instance;
    }

	public void Quit ()
	{
		audioManager.PlaySound(buttonPressSound);

		Debug.Log("Quit Game!");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void Retry ()
	{
		audioManager.PlaySound(buttonPressSound);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnMouseOver()
    {
		audioManager.PlaySound(mouseHoverSound);
    }
	
}
