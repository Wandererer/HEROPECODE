using UnityEngine;
using System.Collections;

public class CharacterNameController : MonoBehaviour {

	private UISprite[] nameList;

	void Start()
	{
		nameList = transform.GetComponentsInChildren<UISprite> ();
		for(int i=0;i<nameList.Length;i++)
		{
			nameList [i].enabled = false;
		}

		nameList [0].enabled=true;
	}


	public void ActiveByBoughtInfo(bool bought, int index)
	{
		if(!bought)
		{
			QuestionMarkApear ();
		}


	}

	public void EnableFalseAll()
	{
		for(int i=0;i<nameList.Length;i++)
		{
			nameList [i].enabled = false;
		}
	}
	public void EnableTrue(int index)
	{
		nameList [index].enabled = true;
	}

	public void QuestionMarkApear()
	{
		nameList [nameList.Length - 1].enabled = true;
	}

}
