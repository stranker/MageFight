using UnityEngine;

public class DebugBeginRoundBtn : MonoBehaviour {

	public void BeginRound(){
		GameManager.Instance.InitializeRound();
	}
}
