using UnityEngine;
using System.Collections;

public class DragIntteration : MonoBehaviour {

	public Lightning light=null;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Lightning> () == null) {
			light.Target = other.transform; //타겟은 부딧치는 것으로 지정
			other.gameObject.AddComponent<Lightning> (); //Lightning을 부딧 치는 곳에 넣어 줌
			light = other.gameObject.GetComponent<Lightning> (); //light를 충돌체거 가져옴
			light.Target = this.transform; //충돌체 에서 light 타겟은 이 DragInteraction
			light.Steps = 5; //line renderer 옵션 지정 
			light.Width = 1; //line renderer 옵션 지정 
			light.Delay = 0.2f; //line renderer 옵션 지정 
			Game.Instance.gameScene.AddLightAndRendererList (other.gameObject, light);//관리를 위해 리스트에 추가 
//			Debug.Log ("hit");
		}
	}

}
