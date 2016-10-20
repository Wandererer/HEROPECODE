using UnityEngine;
using System.Collections;

public class DragIntteration : MonoBehaviour {

	public Lightning light=null;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Lightning> () == null) {
			light.Target = other.transform;
			other.gameObject.AddComponent<Lightning> ();
			light = other.gameObject.GetComponent<Lightning> ();
			light.Target = this.transform;
			light.Steps = 5;
			light.Width = 1;
			light.Delay = 0.2f;
			Game.Instance.gameScene.AddLightAndRendererList (other.gameObject, light);
//			Debug.Log ("hit");
		}
	}

}
