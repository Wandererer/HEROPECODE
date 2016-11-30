using UnityEngine;
using System.Collections;
/*상 점 관 리 용 */
public class StoreManager : NEMonoBehaviour {

	private static StoreManager instance;

	public static StoreManager Instance
	{
		get { return instance;}
		set { instance = value;}
	}

	private int maxModelCount; //최 대 캐 릭 터 개 수 
	private CharacterSelect selectCharacter; //선 택 한 캐 릭 터 
	private int currentIndex=0; // 현 재 위 치 

	public override void Awake ()
	{
		base.Awake ();
		instance = this;
	}

	// Use this for initialization
	void Start () {
		selectCharacter = GameObject.Find ("CharacterArea").GetComponent<CharacterSelect>();
		maxModelCount = selectCharacter.GetMaxModelCount ();
		Game.Instance.gameInfo.FirstSetting ();
		selectCharacter.InitCharacterBoughtSecond (Game.Instance.gameInfo.SECOND);
		selectCharacter.InitCharacterBoughtTHIRD (Game.Instance.gameInfo.THIRD);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Right()
	{
		//오 른 쪽 이 동 시 최 대 를 넘 을 경 우 0 부 터 다 시 
		currentIndex++;
		currentIndex %= maxModelCount;
		selectCharacter.MoveIndex (currentIndex);
	}

	public void Left()
	{
		//왼 쪽 이 동 시 최 소를 넘 을 경 우 최 대 에 서 시 작 
		currentIndex--;
		if(currentIndex<0)
		{
			currentIndex = maxModelCount - 1;
		}
		selectCharacter.MoveIndex (currentIndex);
	}

	public void SelectOrBuy()
	{
		//샀 으 면 캐 릭 터 선 택 된 거 고 안 샀 으 면 돈 충 분 히 있 을 경 우 사 게 만 듬
		if(!selectCharacter.GetCharacterBuyInfo(currentIndex))
		{
			if (Game.Instance.gameInfo.Money < selectCharacter.GetCharacterPrice (currentIndex))
				return;

			Game.Instance.gameInfo.gameData.MinusMoney (selectCharacter.GetCharacterPrice (currentIndex));
			selectCharacter.BuyCurrentIndexCharacter (currentIndex);
			Game.Instance.gameInfo.gameData.SaveCharacterBuyInfo (currentIndex);
			Game.Instance.gameInfo.gameData.ChangeTitleImageForSelectCharacter(currentIndex);
			Game.Instance.gameScene.ChangeInstantiateCharacterSelectedNumber (currentIndex);
			Game.Instance.gameScene.DestroyStoreUI ();
			Game.Instance.gameScene.gameState = GameState.Ready;
		}
		else
		{
			//TODO: 현재 선 택 된 캐 릭 터로 앞 으 로 시 작함 
			Game.Instance.gameInfo.gameData.ChangeTitleImageForSelectCharacter(currentIndex);
			Game.Instance.gameScene.ChangeInstantiateCharacterSelectedNumber (currentIndex);
			Game.Instance.gameScene.DestroyStoreUI ();
			Game.Instance.gameScene.gameState = GameState.Ready;
		}
	}
}
