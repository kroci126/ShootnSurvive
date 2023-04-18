using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LifesCounterUI : MonoBehaviour {

	private Text lifesText;

	void Awake () {
		lifesText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameMaster.RemainingLifes > 1)
        {
			lifesText.text = "Lifes: " + GameMaster.RemainingLifes.ToString();
		}
        else
        {
			lifesText.text = "Life: " + GameMaster.RemainingLifes.ToString();
        }
		
	}
}
