using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CharacterSelect : MonoBehaviour {

	private List<GameObject> models;
	private int selectionIndex=0; //기본 인덱스
	private string[] characterName={ "Male","Female"}; //각 index마 다 캐 릭 터 이 름 
	private string[] characterDescription = { "Male\nCharacter","Female\nCharacter" }; //각 index 마 다 캐 릭 터 설 명 
	private bool[] isCharacterBought = { true,false }; //캐 릭 터 샀 나 안 샀 나 
	private int[] priceOfCharacter = { 0, 100 }; // 캐 릭 터 당 가 

	private GameObject characterSelectObject; //캐 릭 터 선 택 UI
	private GameObject characterBuyObject;// 캐 릭 터 사 기 UI

	CharacterNameController charName;//character별 이 름 표 시용 

	// Use this for initialization
	void Start () {
		models = new List<GameObject> ();
		selectionIndex = 0;
		foreach(Transform t in transform)
		{
			models.Add (t.gameObject);
			t.gameObject.SetActive(false);
		}

		models [selectionIndex].SetActive (true);
		charName = GameObject.Find ("CharacterName").GetComponent<CharacterNameController> ();
		characterBuyObject = GameObject.Find ("CharacterBuy");
		characterSelectObject= GameObject.Find ("CharacterSelect");
		characterBuyObject.SetActive (false);
	}

	private void AppearSelectCharacter()
	{
		// 선 택 UI 보 이 게 함 
		characterSelectObject.SetActive (true);
	}

	private void AppearBuyObject(int index)
	{
		//사 기 UI 보 이 게 하 고 해 당 캐 릭 터 가 격 으 로 설 정 
		characterBuyObject.SetActive (true);
		GameObject.Find ("CoinLabel").GetComponent<UILabel> ().text = priceOfCharacter [index].ToString ();
	}

	private void DissapearSelectAndBuy()
	{
		//사기 나 선 택 UI 안 보 이 도 록 
		characterSelectObject.SetActive (false);
		characterBuyObject.SetActive (false);
	}

	public int GetMaxModelCount()
	{
		return models.Count;
	}

	public void InitCharacterBoughtSecond(bool Second)
	{
		isCharacterBought [1] = Second;
	}

	public void MoveIndex(int index)
	{
		// 이 동 에 따 른 캐 릭 터 보 여 주 기 
		if(index==selectionIndex)
		{
			return;
		}

		if(index<0 || index>=models.Count)
		{
			return;
		}
		DissapearSelectAndBuy (); 
		models [selectionIndex].SetActive (false);
		charName.EnableFalseAll ();
		selectionIndex = index;
		models [selectionIndex].SetActive (true);
		if (!isCharacterBought [selectionIndex]) 
		{
			models [selectionIndex].GetComponent<SpriteRenderer> ().color = Color.black;
			charName.QuestionMarkApear ();
			AppearBuyObject (selectionIndex);
		}
		else
		{
			charName.EnableTrue (selectionIndex);
			AppearSelectCharacter ();
		}
		
	}




	public bool GetCharacterBuyInfo(int index)
	{
		// 해 당 index에 캐 릭 터 샀 는 지 정 보 받 아 옴 
		return isCharacterBought [index];
	}

	public void BuyCurrentIndexCharacter(int index)
	{
		// 해 당 index에 캐 릭 터 를 삼 
		isCharacterBought [index] = true;
		CharacterBoughtColorChage ();
		DissapearSelectAndBuy ();
		charName.EnableFalseAll ();
		charName.EnableTrue (index);
		AppearSelectCharacter ();
	}

	public int GetCharacterPrice(int index)
	{
		// 해 당 index에 캐 릭 터 가 격 가 져 
		return priceOfCharacter [index];
	}

	private void CharacterBoughtColorChage()
	{
		// 해 당 index에 캐 릭 터 가 안 샀 었 을 경 우 실 루 엣 해 
		models [selectionIndex].GetComponent<SpriteRenderer> ().color = Color.white;
	}
}
