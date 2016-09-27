using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		DestroyCoinObject ();
	}

	void DestroyCoinObject()
	{ //일 정 아 래 로 내 려 오 면 코 인 증 가 하 고 효 과 음 나 오 고 파 괴 
		if(transform.position.y<-10)
		{
			Game.Instance.gameInfo.gameData.AddMoney (1);
			SoundManager.Instance.PlayActionSound ("Coin", Game.Instance.gameInfo.Effect);
			Destroy (gameObject);
		}
	}
}
