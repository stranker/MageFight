using UnityEngine;

public class DebugBeginRoundBtn : MonoBehaviour {

	public void BeginRound(){
		transform.parent.gameObject.SetActive(false);
		GameManager.Instance.InitializeRound();
	}
}
