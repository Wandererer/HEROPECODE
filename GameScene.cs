using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameScene : NEMonoBehaviour {

    private static readonly float WaitInput_Time_Interval = 1.0f;
    private static readonly float AutoMove_Time_Interval = 0.5f;

	public GameState gameState;//게 임 상 태
	public GameState prevGameState;//이 전 상 태 

	public float meter=0;//올 라 가 는 양 
	float playTime = 0f; // 초 당 업 데 이 트 위 해 계 

    private float deltaTime;
    private float autoMoveDeltaTime; //auto move 되 는  시 간
	private float obstacleLifetime; // 몬 스 터 시 간 
    private float waitInputDeltaTime;  //input 기 다 리 는 시 간 
	private float fixTime=0.5f;

	private Vector3 tempPositionForUI;//ui 임 시 위 치 


	int money; //돈 
	int highScore; //최 대 점 수 

	float monsterSpawnTime; //몬 스 터 스 폰 시 간 


	float maxMonsterSeconds=2f;//TODO: 나 중 에 시 간 초 받 아 와 서 설 정
    public float monsterTimeIncrease=0f; //아이템 시간 받아옴
    private float boosterDeltaTime=3f;//TODO: 나중에 부스터 시간 받아와서 입력

	public GameObject[] gamePlayInstantiateObject;//플레이용 게임 오 브 젝 트 생 성 용 배 
	List <string> removeObejct=new List<string>();

	bool isInstantObject=false; //게 임 중 오 브 젝 트 생 성 되 었 나 
    private bool isBoosterOn=false; // 부 스 트 가 켜 졌 나 ?
    private bool isMonsterTimeChange=false; //몬 스 터 시 간 이 변 했 ?
	private bool isHighScore;  //하 이 스 코 어 갱 신 했 나 ?
	public bool isContinue=false; //계 속 하 기 클 릭 시 (돈 쓰 기 or 광 고 보 기 완 료 시 )
	public bool isContinued=false; // 한번 계 속 하 기 되 었 는 지 ... 
	private bool isMonsterInstantiate=false;
	private bool isSetMonsterTimeValue=false;
	private bool isCoinClicked=false;
	private bool isAdClicked=false;
	private bool isAdFinished=false;
	private bool isFacebookClicked=false;
	private bool isNotEnoughCoinClicked=false;
	private bool isDrag=false;
	GameObject UIroot; //Ngui root

	//UI용 게 임 오 브 젝 트 
	public GameObject TitleUI;
	public GameObject TutorialUI;
	public GameObject StartUI;
	public GameObject PlayUI;
	public GameObject GameOverUI;
	public GameObject PauseUI;
	public GameObject StoreUI;
	public GameObject AdUi;
	public GameObject PopUpUi;
	public GameObject CreditUI;
	public GameObject CoinObject;




	private Character myCharacter; //내 캐릭터 

	public Material lightningMaterial;//드 래 그 효 과 용 매 티 리 얼 

	List<GameObject> rendererList=new List<GameObject>(); //랜 더 러 요 
	List<Lightning> lightningList=new List<Lightning>(); // 랜 더 러 용 

	UISlider MonsterSecondBar; //피 버 바 
	private UILabel moneyUiLabel;//돈 Label

	Speed speed;

    private List<Obstacle> obstacles = new List<Obstacle>();


    void Start () {
		highScore = game.gameInfo.HighSocre;
		money = game.gameInfo.Money;


		gamePlayInstantiateObject[0]=(GameObject)Resources.Load(string.Format("Prefabs/{0}","Character"+game.gameInfo.CharacterNumber),typeof(GameObject));
		UIroot = GameObject.Find ("UI Root");
		speed = new Speed ();

		monsterSpawnTime = Game.Instance.obstacleSpawnManager.PickNormalTime ();
	}

	public override void Awake ()
	{
		base.Awake ();
        game.SetGameScene(this);
		gameState = GameState.Title;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(monsterSpawnTime);
		switch(gameState)
		{
		case GameState.Ready:
			speed.SetNormalSpeed (playTime);
			Game.Instance.gameInfo.FirstSetting ();
			break;

		case GameState.Play:
			MakeGameObjectInPlay (); //플 에 이 용  오 브 젝 트 생  
			UpdateMonsterSpawnTime ();
			UpdateTime ();  //시 간 공 식 업 데 이 트 하 거 나 몬 스 터 시 간 설 정 
			CheckHealth (); //HP 확ㅣ
			CheckHighScore (); //하 이 스 코 어 넘 겼 는 지 확 인
            CheckBooster(); //부 스 터 효 과 발 동 하 였 는 지 확 인 
			break;

		case GameState.Fall:
			NoInputAndMoster (); //input 하 고 monster 안 하 게
			SetIsKinemeticTrueForTrueToFalling (); //에 너 지 다 닳 아 서 떨 어 지 게 
			CheckCharacterPositionYForEnd (); //일 정 이 하 내 려 오 면 END를 위 해 변 경
			break;

		case GameState.Pause:


			break;

		case GameState.Store:

			break;

		case GameState.Credit:

			break;

		case GameState.End:
			SetCharacterPositionAndAnimForEnd (); //마 지 막 을 위 해 캐 릭 터 위 치 이 동 및 애 니 메 이 션 변 경 

			break;

		default:

			break;
		}
	}

    private void LateUpdate()
    {
        if (gameState == GameState.Play)
        {
            game.inputManager.UpdateInput();
			ChangeBackgroundSpriteFormeterWhenContinueClicked ();
        }
    }

    void OnGUI()
	{


		switch(gameState)
		{
		case GameState.Title:
			CreateTitleGUI();
			break;
		case GameState.Ready:
			CreateGameStartGUI ();
			break;
		case GameState.Tutorial:
			CreateTutorialGUI();
			break;

		case GameState.Play:
			CreatePlayGUI (); //플 레 이 용 u i  생 성 
			CheckMonsterTime (); //몬 스 터 시 간 다 닳 았 는 지 확 인 
			UpdatePlayUIInfo ();// u i label  업 데 이 트


			break;

		case GameState.Fall:
			UpdatePlayUIInfo (); //UI 정 보 갱 신
			break;
		case GameState.Pause:
			CreatePauseGUI (); //일 시 정 지 메 뉴 생 성
			break;

		case GameState.Store:
			CreateStoreGUI (); // 상 점 UI 생 성
			UpdateStoreMoneyUIInfo (); //상 점 에 서 돈 정 보 갱 신
			break;

		case GameState.Credit:
			CrateCreditUI (); // 만 든 사 람 들 UI 생ㅓ
			break;

		case GameState.End:
			CreateGameEndMenu ();// 마 지 막 화 면 생 성
			UpdateEndMoneyUIInfo (); //끝 화 면 돈 정 보 갱 
			break;

		default:

			break;
		}
	}

	void UpdateMonsterSpawnTime()
	{
		//몬 스 터 스 폰 시 간 줄 어 듬 
		if(!isMonsterInstantiate && !isBoosterOn)
			monsterSpawnTime -= Time.deltaTime;

		InstantiateSpawnMonster ();
	}
	void InstantiateSpawnMonster()
	{
		if(monsterSpawnTime<0 && !isMonsterInstantiate)
		{
			DestroyItem ();
			//몬 스 터 스 폰  시 킴
			Game.Instance.inputManager.isMousePressed=true;
			isMonsterInstantiate = true;

			bool result = Random.Range (0f, 1.0f) < 0.7f;

			if (result)
				Game.Instance.obstacleSpawnManager.SpawnNormalObstacles ();
			else 
				Game.Instance.obstacleSpawnManager.SpawnDragObstacles ();
			
			
			Game.Instance.obstacleSpawnManager.SetNormalTimeForNextSpawn ();
			monsterSpawnTime = Game.Instance.obstacleSpawnManager.PickNormalTime ();
		}
	}

	void UpdateTime()
	{
		//시 간 초 당 함 수 실 행 
		/* game.obstacleSpawnManager.UpdateObstacles();*/

		if (IsExistObstacle ()) 
		{
			SetObstacleLifeTimeandMaxMonsterSeconds ();//몬스터 바 백 류 설 정 
			SetSliderActiveByMonsterCount (); //몬 스 터 개 수 에 따 라 슬 라 이 더 나 타 남
			SetMonsterTimeBarValue (); //몬 스 터 바 정 보 변 경
		
            obstacleLifetime -= Time.deltaTime;
			
		} 
		else
		{
			/*  game.obstacleSpawnManager.UpdateSpawnInterval();*/

            waitInputDeltaTime += Time.deltaTime;

            UpdateNormalSpeed(); //1 초 마 다 공 식 에 의 한 갱 신

            UpdateAutoMove(); //자 동 올 라 가 능 상 태 인 지 확 
		}
	}

    void UpdateNormalSpeed()
    {//1 초 당  따 라 올 라 갈 기 본 속 도 지 정 
        deltaTime += Time.deltaTime;
        if (deltaTime >= 1.0f) {
            deltaTime = 0;
            playTime++;
            speed.SetNormalSpeed (playTime);
        }
    }



    void UpdateAutoMove()
    {//자 동 올 라 가 는 상 태 일 경 
        if(IsEnableAutoMove())
        {
            autoMoveDeltaTime += Time.deltaTime;
            //인 터 벌 당 업 데 이 트 되 도 록
            autoMoveDeltaTime += Time.deltaTime;
            if (autoMoveDeltaTime >= AutoMove_Time_Interval) {
                autoMoveDeltaTime = 0;
                MoveBackGroundForAutoSpeed ();//자 동 으 로 올 라 
            }
        }
    }

	public void SetPrevGameState(GameState state)
	{
		//게 임 진 행 중 이 거 나 떨 어 질 때 일 시 정 지 할 때 다 시 돌 아 오 기 위 
		prevGameState = state;
	}

	public GameState GetPrevGameState()
	{
		// 게 임 상 태 반 환 함 
		return prevGameState;
	}

    bool IsEnableAutoMove()
    {
		//자 동 올 라 가 기 상 태 지 정 
        if(waitInputDeltaTime >= WaitInput_Time_Interval)
        {
            return true;
        }

        return false;
    }

    void ResetWaitInputTime()
    {
		//입 력 시 간 초 기 화 
        waitInputDeltaTime = 0;
    }

	void CheckHealth()
	{
		//체 력 점 검
		if(Earth.Instance.currHP<=0)
		{
			NoInputAndMoster ();
			SoundManager.Instance.SetSpecificSoundStopBySoundName ("Drag");
			DestroyDragInterraction ();
			Earth.Instance.currHP = 0;
			Game.Instance.obstacleSpawnManager.ResetSpawnList ();
			myCharacter.Fall();
			myCharacter.gameObject.AddComponent<Rigidbody2D> ();
			gameState = GameState.Fall;
		}
	}

	void CheckCharacterPositionYForEnd()
	{
		//떨 어 질 때 일 정 이 하 떨 어 지 면 상 태 들 저 장 하 고 끝 상 태 로 이 
		if(myCharacter.transform.position.y<=-10)
		{
			SoundManager.Instance.SetPlaySoundStop (); //재 생 중 인 사 운 드 정 
			RemoveObjectForEndGame ();
			DestroyPlayUI ();
			DestroyRigidBody2DForCharacter ();
			myCharacter.ReadyFix ();//캐 릭 터 애 니 메 이 션 고 치 는 상 태 로 
			gameState = GameState.End;
		}
	}

	void SetCharacterPositionAndAnimForEnd()
	{
		//끝 화 면 으 로 갈 시 캐 릭 터 상 태 고 치 는 상 태 로 바 꾸 고 고 치 는 애 니 메 이 션 느 리 게 
		Vector3 initPos = new Vector3 (0, 2.3f, 0);
		if(initPos!=myCharacter.GetComponent<Transform>().position)
		{
			myCharacter.transform.position = initPos;
		}
		fixTime -= Time.deltaTime;

		if (fixTime <= 0) {
			myCharacter.Fix ();
			fixTime = 0.5f;
		}
		
	}





	void NoInputAndMoster()
	{
		//몬 스 터 등 장 불 가 로 만 
		Game.Instance.inputManager.isMousePressed = false;
		Game.Instance.obstacleSpawnManager.obstacleSpawnDeltaTime = 0f;
	}

	void CheckHighScore()
	{
		//최 대 기 록 체 크 
		if(highScore<meter)
		{
			highScore = (int)meter;
			isHighScore = true;
		}
	}

	void CheckMonsterTime()
	{
		//몬 스 터 나 타 나 는 시 간 이 0 이 면 공 격 받 
		if(obstacleLifetime <=0)
		{
            DisappearUISlider();//슬 라 이 더 안 보 이 게
            if(monsterTimeIncrease>0)
			{
				monsterTimeIncrease = 0f;
				isMonsterTimeChange=false;
			}
			isMonsterInstantiate = false;
			isSetMonsterTimeValue = false;
			AttackedByObstacles (); // 몬 스 터 못 없 애 서 공 격 당 
		}
	}

    void DisappearUISlider()
    {
		//ui slider 없 애 버 림 
        SetUISliderDissapear ();//아이템 사용으로 monster 시간이 증가 하였으면
    }

	public void ResetValue()
	{
        // 기본 변 수 들 초 기 
		Game.Instance.gameInfo.FirstSetting();
		highScore = Game.Instance.gameInfo.HighSocre;
		isSetMonsterTimeValue=false;
        game.gameInfo.Reset ();
		Game.Instance.obstacleSpawnManager.BossHpReset ();
		Game.Instance.inputManager.dragObstacles.Clear ();
		isMonsterInstantiate = false;
		isHighScore = false;
		Earth.Instance.Reset ();
        Game.Instance.obstacleSpawnManager.obstacleSpawnDeltaTime=0;
		Game.Instance.obstacleSpawnManager.ResetSpawnList ();
        playTime=0f;
		obstacleLifetime = 0f;
		isMonsterTimeChange = false;
        speed.SetNormalSpeed (playTime);
		isContinue = false;
		isContinued = false;
		meter = game.gameInfo.Meter;
	}

	public void ResetValueForContinue()
	{
		//계 속 하 기 할 시 일 부 정 보 초 기 화 
		isHighScore = false;
		isCoinClicked = false;
		Game.Instance.obstacleSpawnManager.obstacleSpawnDeltaTime=0;
		Earth.Instance.Reset ();
		speed.SetNormalSpeed (playTime);
	}

	void CreateTitleGUI()
	{
		GameObject titleUI = GameObject.Find("TitleUI");
		if (titleUI == null)
		{
			titleUI = Instantiate(TitleUI);
			titleUI.name = TitleUI.name;
			titleUI.transform.parent = UIroot.transform;
			titleUI.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
			StartCoroutine(DelayChangeGameState());
		}
	}

	void CreateTutorialGUI()
	{
		GameObject tutorialUI = GameObject.Find("TutorialUI");
		if (tutorialUI == null)
		{
			tutorialUI = Instantiate(TutorialUI);
			tutorialUI.name = TutorialUI.name;
			tutorialUI.transform.parent = UIroot.transform;
			tutorialUI.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
		}
	}

	private System.Collections.IEnumerator DelayChangeGameState()
	{
		#if UNITY_IOS
		yield return new WaitForSeconds(6.0f);
		#else
		yield return new WaitForSeconds(2.0f);
		#endif

		DestroyTitleUI();
		gameState = GameState.Ready;
	}

	public void CreateGameStartGUI()
	{
		//start ui 없 음 만 들 어 줌 
		GameObject startUI = GameObject.Find ("StartUI");
		if(startUI==null)
		{
			startUI = Instantiate (StartUI);

            Game.Instance.gameInfo.FirstSetting (); //저장된 상태 받아옴
			ChangeTitleMenuCharacter();
			startUI.name = StartUI.name;
			startUI.transform.parent = UIroot.transform;
			startUI.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 1);

			UILabel highScoreLabel = GameObject.Find ("HighScoreLabel").GetComponent<UILabel> ();
			highScoreLabel.text = Game.Instance.gameInfo.HighSocre.ToString ();

			UILabel coinLabel=GameObject.Find ("Coin").GetComponent<UILabel> ();
			coinLabel.text = Game.Instance.gameInfo.Money.ToString();

			bool current = Game.Instance.gameInfo.BGM;


			UIToggle bgmUIToggle = GameObject.Find ("BGM Button").GetComponent<UIToggle>();
			if(current==true)
			{
				bgmUIToggle.value = current;
			}
			else
			{
				bgmUIToggle.value = current;
			}


			current = Game.Instance.gameInfo.Effect;
			UIToggle effectUIToggle = GameObject.Find ("Effect Button").GetComponent<UIToggle>();

			if(current==true)
			{
				effectUIToggle.value = current;
			}
			else
			{
				effectUIToggle.value = current;
			}

		
		
		}
	}

	void UpdatePlayUIInfo()
	{
		//기 록 변 경 에 따 른 ui update
		Game.Instance.gameInfo.FirstSetting();
		UILabel scoreLabel = GameObject.Find ("Meter").GetComponent<UILabel> ();
		scoreLabel.text = meter.ToString ("f1")+"M";
		UILabel coinLabel=GameObject.Find ("Coin").GetComponent<UILabel> ();
		coinLabel.text = Game.Instance.gameInfo.Money.ToString();
		UILabel hp = GameObject.Find ("HP").GetComponent<UILabel> ();
		hp.text = Earth.Instance.currHP.ToString();
	}

	void UpdateStoreMoneyUIInfo()
	{
		//상 점 용 돈 정 보 갱 신
		Game.Instance.gameInfo.FirstSetting ();
		moneyUiLabel.text = Game.Instance.gameInfo.Money.ToString ();
	}

	void UpdateEndMoneyUIInfo()
	{
		//끝 화 면 용 돈 정 보 갱 
		Game.Instance.gameInfo.FirstSetting ();
		moneyUiLabel.text = Game.Instance.gameInfo.Money.ToString ();
	}

	void CreatePlayGUI()
	{
		GameObject playUI = GameObject.Find ("PlayUI");
		if(playUI==null)
		{
			playUI = Instantiate (PlayUI);
			playUI.name = PlayUI.name;
			playUI.transform.parent = UIroot.transform;
			playUI.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 1);
			UILabel meterLabel = GameObject.Find ("Meter").GetComponent<UILabel> ();
			meterLabel.text = meter.ToString ("");
			UILabel coinLabel=GameObject.Find ("Coin").GetComponent<UILabel> ();
			coinLabel.text = money.ToString ();
			UILabel hp = GameObject.Find ("HP").GetComponent<UILabel> ();
			hp.text = Earth.Instance.currHP.ToString();
            Game.Instance.gameInfo.FirstSetting(); //플레이 유아이 재 시작시 저장상태 불러옴
			MonsterSecondBar = GameObject.Find ("MonsterSecondBar").GetComponent<UISlider> ();
			SoundManager.Instance.PlayBgmSound ("BGM",Game.Instance.gameInfo.BGM,1.0f);
			playUI.transform.FindChild("ItemBoost").GetComponent<UILabel>().enabled = false;
			//SetUISliderDissapear ();
		}
	}

	public void PlayItemBoostUI(string itemName)
	{
		GameObject boostObject = GameObject.Find("ItemBoost");
		boostObject.GetComponent<UILabel>().text = itemName;
		boostObject.GetComponent<UILabel>().enabled = true;
		StartCoroutine(DelayHideBoostUI(1.0f));
	}

	public void playMonsterDragUI()
	{
		GameObject boostObject = GameObject.Find("ItemBoost");
		if (isDrag==true && obstacles[0].isDragType==true) {
			boostObject.GetComponent<UILabel> ().text = "Drag";
			boostObject.GetComponent<UILabel> ().enabled = true;
			isDrag = false;
			StartCoroutine (DelayHideBoostUI (obstacleLifetime / 2f));
		}
	}

	private System.Collections.IEnumerator DelayHideBoostUI(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		GameObject.Find("ItemBoost").GetComponent<UILabel>().enabled = false;
		yield return null;
	}

	void SetSliderActiveByMonsterCount()
	{
		if(MonsterSecondBar.GetComponent<UISlider> ().enabled==false)
		{
			SetUISliderApear ();
		}
	}

	void SetUISliderApear()
	{
		MonsterSecondBar.GetComponent<UISlider> ().enabled = true;
		UISprite[] sprite = MonsterSecondBar.GetComponentsInChildren<UISprite> ();
		sprite [0].enabled = true;
		sprite [1].enabled = true;
		MonsterSecondBar.GetComponentInChildren<UILabel> ().enabled = true;
	}

	void SetUISliderDissapear()
	{
		//MonsterSecondBar = GameObject.Find ("MonsterSecondBar").GetComponent<UISlider> ();
		MonsterSecondBar.GetComponent<UISlider> ().enabled = false;
		UISprite[] sprite = MonsterSecondBar.GetComponentsInChildren<UISprite> ();
		sprite [0].enabled = false;
		sprite [1].enabled = false;
		MonsterSecondBar.GetComponentInChildren<UILabel> ().enabled = false;
	}

	public void SetIsKinemeticTrueForPauseWhileFalling()
	{
		//떨 어 지 는 중 puase일 경 우  kinematic true
		myCharacter.GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	 void SetIsKinemeticTrueForTrueToFalling()
	{
		//떨 어 지 는 중 kinematic true
		if(myCharacter.GetComponent<Rigidbody2D> ().isKinematic==true && myCharacter !=null)
		myCharacter.GetComponent<Rigidbody2D> ().isKinematic = false;
	}

	void DestroyRigidBody2DForCharacter()
	{
		//end 화 면 을 위 해 리 지 드 바 디 삭 
		Destroy (myCharacter.GetComponent<Rigidbody2D> ());
	}

	void SetMonsterTimeBarValue()
	{
		//몬 스 터 시 간 바 재 설 
		MonsterSecondBar.value = obstacleLifetime/ (maxMonsterSeconds+(float)monsterTimeIncrease);
		MonsterSecondBar.GetComponentInChildren<UILabel> ().text = obstacleLifetime.ToString ("f1");
		if(obstacles[0].isDragType==true)
			playMonsterDragUI ();
	}

	public void SetObstacleLifeTimeandMaxMonsterSeconds()
	{
		if (!isSetMonsterTimeValue) {

			if( obstacles[0].isDragType==false && obstacles[0].isBoss==false)
			{
				obstacleLifetime = 1.4f;
				maxMonsterSeconds = 1.4f;
			}
			else if(obstacles[0].isDragType==true && obstacles[0].isBoss==false)
			{
				obstacleLifetime = 1.4f;
				maxMonsterSeconds = 1.4f;
			}
			else if( obstacles[0].isBoss==true)
			{
				obstacleLifetime = 1.0f;
				maxMonsterSeconds = 1.0f;
			}
				
			CheckItemMonsterIncereaseTime ();
			isSetMonsterTimeValue = true;
		}
	}

	public void CreateGameEndMenu()
	{
		GameObject gameOverUI = GameObject.Find ("GameOverUI");
		if(gameOverUI==null)
		{
			gameOverUI = Instantiate (GameOverUI);
			gameOverUI.name = GameOverUI.name;
			gameOverUI.transform.parent = UIroot.transform;
			gameOverUI.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 1);
			moneyUiLabel=GameObject.Find ("CoinLabel").GetComponent<UILabel> ();
			UILabel meterLabel = GameObject.Find ("MeterLabel").GetComponent<UILabel> ();
			meterLabel.text = meter.ToString ("f1")+"m";

			moneyUiLabel.text =money.ToString ();
		
			if(isHighScore)
			{
				GameObject.Find ("HighScore").SetActive (true);
				SoundManager.Instance.PlayActionSound ("HighScore",Game.Instance.gameInfo.Effect);
				game.gameInfo.gameData.UpdateHighScore (highScore); // 저 장 
			}
			else
			{
				GameObject.Find ("HighScore").SetActive (false);
				SoundManager.Instance.PlayActionSound ("Ending",Game.Instance.gameInfo.Effect);
			}

			if(isContinued)
			{
				GameObject[] dissapearList = GameObject.FindGameObjectsWithTag ("Dissapear");
				for(int i=0;i<dissapearList.Length;i++)
				{
					dissapearList [i].SetActive (false);
				}

				Transform movableObject = gameOverUI.transform.FindChild ("ContinueLabel");
				movableObject.gameObject.GetComponent<UILabel>().text="포기하지마! 다시 해보는거야!";
				movableObject.localPosition = new Vector3 (29, -391, 0);
			}

		}
	}

	public void CreatePoPupUI()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			popUpUI.name = PopUpUi.name;
		}
	}


	public void CreatePoPupUIForCoinContinueMenu()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="10코인을 쓰시겠습니까?";
			popUpUI.name = PopUpUi.name;
			CharacterRendererOff ();
			isCoinClicked = true;
		}
	}

	public void CreatePopUpUIForNotEnoughCoin()
	{
		
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="코인이 부족 합니다";
			popUpUI.name = PopUpUi.name;

			PopUpChildButtonSettingFalseAndMove ();
			isNotEnoughCoinClicked = true;
			CharacterRendererOff ();
		}

	}

	public void CratePopUpUiForAdIsNotReady()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="광고가 준비중 입니다";
			popUpUI.name = PopUpUi.name;

			PopUpChildButtonSettingFalseAndMove ();

			CharacterRendererOff ();
			isCoinClicked = true;
		}
	}

	public void CreatePoPupUIForAdResultFinished()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="계속 하시겠습니까?";
			popUpUI.name = PopUpUi.name;
			isAdFinished = true;
		}
	}

	public void CreatePopUiForFacebookLoginFailed()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="FACEBOOK 로그인 실패";
			popUpUI.name = PopUpUi.name;

			PopUpChildButtonSettingFalseAndMove ();

			CharacterRendererOff ();
		}
	}

	public void CreatePopUiForFacebookShareFailed()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="FACEBOOK 공유 실패";
			popUpUI.name = PopUpUi.name;

			PopUpChildButtonSettingFalseAndMove ();

			CharacterRendererOff ();
		}
	}

	public void CreatePopUiForFacebookShareError()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="FACEBOOK 공유 에러";
			popUpUI.name = PopUpUi.name;

			PopUpChildButtonSettingFalseAndMove ();

			CharacterRendererOff ();
		}
	}



	public void CreatePopUiForFacebookShareSuccess()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="FACEBOOK 공유 완료";
			popUpUI.name = PopUpUi.name;

			PopUpChildButtonSettingFalseAndMove ();

			CharacterRendererOff ();
		}
	}

	public void CratePopUpUiForAdResultSkipped()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			PopUpChildButtonSettingFalseAndMove ();
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="광고가 넘겨졌습니다";
			popUpUI.name = PopUpUi.name;
			isAdFinished = true;
		}
	}

	public void CreatePopUpUiForAdResultFailed()
	{
		GameObject popUpUI = GameObject.Find ("PopUpUI");
		if(popUpUI==null)
		{
			PopUpChildButtonSettingFalseAndMove ();
			popUpUI = Instantiate (PopUpUi);
			GameObject label = GameObject.Find ("ChangeLabel");
			label.GetComponent<UILabel>().text="광고가 취소됬습니다";
			popUpUI.name = PopUpUi.name;
			isAdFinished = true;
		}
	}

	public bool IsAdFinished()
	{
		return isAdFinished;
	}

	public void SetAdFinished(bool result)
	{
		isAdFinished = result;
	}

	public bool IsFacebookClicked()
	{
		return isFacebookClicked;
	}

	public void SetFacebookClicked(bool result)
	{
		isFacebookClicked = result;
	}

	private void PopUpChildButtonSettingFalseAndMove()
	{
		GameObject negaButton = GameObject.Find ("NegativeButton");
		negaButton.SetActive (false);

		GameObject postButton = GameObject.Find ("PositiveButton");
		postButton.GetComponent<Transform> ().position = new Vector3 (0, postButton.GetComponent<Transform> ().position.y, 0);

	}

	public bool CoinClicked()
	{
		return isCoinClicked;
	}

	public bool AdClicked()
	{
		return isAdClicked;
	}

	public void AdClicedTrue()
	{
		isAdClicked = true;
	}

	public void AdClickedFlase()
	{
		isAdClicked = false;
	}

	public void NotEnoughCoinFalse()
	{
		isNotEnoughCoinClicked = false;
	}

	public bool NotEnoughCoin()
	{
		return isNotEnoughCoinClicked;
	}

	public void CharacterRendererOff()
	{
		GameObject obj = GameObject.FindGameObjectWithTag ("Character");
		if(obj!=null)
		{
			SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer> ();
			for(int i=0;i<sprites.Length;i++)
			{
				sprites [i].enabled = false;
			}
		}
	}

	public void CharacterRendererOn()
	{
		GameObject obj = GameObject.FindGameObjectWithTag ("Character");
		if(obj!=null)
		{
			SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer> ();
			for(int i=0;i<sprites.Length;i++)
			{
				sprites [i].enabled = true;
			}
		}
	}


	public void ChangePauseUiPosition()
	{
		GameObject obj=GameObject.Find("PauseUI");
		tempPositionForUI = obj.transform.localPosition;
		//Debug.Log ("Here " + tempPositionForUI);
		Vector3 temp = new Vector3 (4000f, 0, 0);
		obj.transform.localPosition = temp;
	}

	public void DestroyPopUpUI()
	{
		GameObject obj=GameObject.Find("PopUpUI");
		if (obj != null) {
			DestroyImmediate (obj);
			CharacterRendererOn ();
		}
	}

	public void DestroyPauseUIandPopUpUI()
	{
		GameObject obj=GameObject.Find("PauseUI");
		DestroyImmediate (obj);
		obj=GameObject.Find("PopUpUI");
		DestroyImmediate (obj);
		isCoinClicked = false;
	}

	public void MovePauseUIToOriginPosition()
	{
		DestroyImmediate (PopUpUi);
		GameObject obj=GameObject.Find("PauseUI");
		obj.transform.localPosition = tempPositionForUI;
		//Debug.Log ("Here2 " + obj.transform.localPosition);
		tempPositionForUI = new Vector3(0,0,0);
	}


	void ChangeBackgroundSpriteFormeterWhenContinueClicked()
	{
		if(isContinue==true)
		{
			BackgroundManager.Instance.SettingBackgroundForContinue (meter);
			isContinued = true;
			isContinue = false;
		}
	}


	public void CreatePauseGUI()
	{
		GameObject puaseUI = GameObject.Find ("PauseUI");
		if(puaseUI==null)
		{
			puaseUI = Instantiate (	PauseUI);
			puaseUI.name = 	PauseUI.name;
			puaseUI.transform.parent = UIroot.transform;
			puaseUI.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 1);

			Game.Instance.gameInfo.FirstSetting ();
			bool current = Game.Instance.gameInfo.BGM;
			UIToggle bgmUIToggle = GameObject.Find ("BGM Button").GetComponent<UIToggle>();
			if(current==true)
			{
				Debug.Log("true");
				bgmUIToggle.value = current;
			}
			else
			{
				Debug.Log("false");
				bgmUIToggle.value = current;
			}


			current = Game.Instance.gameInfo.Effect;
			UIToggle effectUIToggle = GameObject.Find ("Effect Button").GetComponent<UIToggle>();

			if(current==true)
			{
				effectUIToggle.value = current;
			}
			else
			{
				effectUIToggle.value = current;
			}

		

		}
	}



	 void CreateStoreGUI()
	{
		GameObject storeUI = GameObject.Find ("StoreUI");
		if(storeUI==null)
		{
			Game.Instance.gameInfo.FirstSetting ();
			storeUI = Instantiate (	StoreUI);
			storeUI.name = 	StoreUI.name;
			storeUI.transform.parent = UIroot.transform;
			moneyUiLabel = GameObject.Find ("MoneyLabel").GetComponent<UILabel> ();
			moneyUiLabel.text = Game.Instance.gameInfo.Money.ToString ();
			gameObject.AddComponent<StoreManager> ();
			storeUI.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 1);
		}
	}

	void CrateCreditUI()
	{
		GameObject creditUI = GameObject.Find ("CreditUI");
		if(creditUI==null)
		{
			creditUI = Instantiate (CreditUI);
			creditUI.name = CreditUI.name;
			creditUI.transform.parent = UIroot.transform;
			creditUI.GetComponent<Transform> ().localScale = new Vector3 (1, 1, 1);
		}
	}

    public void ActiveBoostState()
    {
		//부 스 터 상 태 시 
        isBoosterOn = true;
        myCharacter.Boost();
    }

    void CheckBooster()
    {
		//부 스 터 인 지 확 인 
        if(isBoosterOn)
		{
			boosterDeltaTime -= Time.deltaTime;
			if (boosterDeltaTime >= 0f)
			{
				Game.Instance.inputManager.isMousePressed = false;
				Game.Instance.obstacleSpawnManager.obstacleSpawnDeltaTime = 0f;
				BackgroundManager.Instance.MoveDown(speed.TapSpeed());    
			}
            else
            {
             
                boosterDeltaTime=3f;
                isBoosterOn=false;
                myCharacter.ReadyClimb();
            }
		}
    }
		
    void MakeGameObjectInPlay()
	{

		if(isInstantObject==false)
		{
			for (int i = 0; i < gamePlayInstantiateObject.Length; i++) {
				GameObject obj=Instantiate (gamePlayInstantiateObject [i]);
				obj.name = gamePlayInstantiateObject [i].name;
				removeObejct.Add (obj.name);
			}

			myCharacter = GameObject.Find ("Character"+Game.Instance.gameInfo.CharacterNumber).GetComponent<Character> ();
            myCharacter.ReadyClimb();

            isInstantObject = true;


		}


	}

    public void ResetObjectForReplyGameInPause()
    {
		RemoveObjectForRestartGame();
		ResetValue ();
		//SetUISliderDissapear ();
      //  MakeGameObjectInPlay();
    }

	public void RemoveObjectForRestartGame()
	{
		//재 시 작 을 위 한 오 브 젝 트 제 
		if (isInstantObject == true) {
			for (int i = 0; i < removeObejct.Count; i++) {
				DestroyImmediate(GameObject.Find(removeObejct[i]));
			}

			isInstantObject = false;
            removeObejct.Clear();
			DestroyItem ();
		}
			

		if(obstacles.Count>0)
		{
			for (int i = 0; i < obstacles.Count; i++) {
				Destroy (obstacles [i].gameObject);
			}

			obstacles.Clear ();
		}





	}


	void DestroyItem()
	{
		//죽 었 을 경 우 아 이 템 이 남 아 있 으 면 삭 
		GameObject itemObj = GameObject.FindGameObjectWithTag ("Item");
		if(itemObj!=null)
		{
			Destroy (itemObj);
		}
	}

	public void RemoveObjectForEndGame()
	{
		//끝 화 면 을 위 해 오 브 젝 트 제 거 
		if (isInstantObject == true) {
			List<string> removeStringList = new List<string> ();
			for (int i = 0; i < removeObejct.Count; i++) 
			{
				if(removeObejct[i]=="Character"+Game.Instance.gameInfo.CharacterNumber)
				{
					continue;
				}
				DestroyImmediate (GameObject.Find (removeObejct [i]));
				removeStringList.Add (removeObejct [i]);
			}
			for (int i = 0; i < removeStringList.Count; i++) {
				removeObejct.Remove (removeStringList [i]);
			}
			removeStringList.Clear ();

			DestroyItem ();
		}
	}

	public void DestroyDragInterraction()
	{
		GameObject obj = GameObject.Find ("DragBackground");
		if(obj!=null)
		{
			Destroy (obj);
		}

		obj= GameObject.Find ("DragInterraction");
		if(obj!=null)
		{
			Destroy (obj);
		}

		rendererList.Clear ();
		lightningList.Clear ();

	}


	public void CameraDepthFarForPause()
	{
		//BackGround Ngui 변 경 으 로 인 하 여 캐 릭 터 들 이 pause 시 보 이 는 걸 해 결 
		Camera.main.depth = -1;
	}

	public void CamearaDepthNormalForPlay()
	{
		//BackGround Ngui 변 경 으 로 인 하 여 캐 릭 터 들 이 play시 다 시 보 이 게 
		Camera.main.depth = 0;
	}


	public void AddLightAndRendererList(GameObject line,Lightning lightning)
	{
		lightningList.Add (lightning);
		rendererList.Add (line);

//		Debug.Log (rendererList.Count);
	}

	public void AddLightinginList(Lightning lightning)
	{
		lightningList.Add (lightning);
	}

	public void DestroyLightningAndResetRenderer()
	{
		for (int i = 0; i < lightningList.Count; i++) {
			lightningList [i].Target = null;
			Destroy (lightningList [i]);
		}

		lightningList.Clear ();

		for (int i = 0; i < rendererList.Count; i++) {
			if (rendererList [i] == null)
				continue;
			DestroyImmediate (rendererList [i].GetComponent<LineRenderer> ());

		}

	}

	public void AddLineRenerer()
	{
		for (int i = 0; i < rendererList.Count; i++) {
			DestroyImmediate (rendererList [i].GetComponent<LineRenderer> ());
			LineRenderer lineRenderer = rendererList [i].AddComponent<LineRenderer> ();
			lineRenderer.SetWidth (0.5f, 0.3f);
			lineRenderer.SetColors (Color.yellow, Color.yellow);
			lineRenderer.material = lightningMaterial;
			lineRenderer.useLightProbes = false;
			lineRenderer.SetVertexCount (0);
			lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			lineRenderer.receiveShadows = false;

		}

		rendererList.Clear ();
	}


    public void MoveBackGroundForAutoSpeed()
    {
        //할일 들 추가 예정 예제용으로 뒷 배경 내려가는거처럼 만듬

        if (isBoosterOn == false)
        {
            myCharacter.Climb ();
        }

		if (myCharacter.IsEnableMoveAction()) {
			//올 라 가 는 동 작 이 완 료 되 면 올 라 가 도 록 
			BackgroundManager.Instance.MoveDown (speed.AutoSpeed ());
			MoveDownItem (speed.AutoSpeed ());
		}
    }

	void MoveDownItem(float speed)
	{
		//아 이 템 있 을 시 누 르 지 않 을 경 우 배 경 과 같 이 내 려 
		GameObject itemObject = GameObject.FindGameObjectWithTag ("Item");
		if(itemObject!=null)
		{
			MoveItemObjectToDown (itemObject, speed);
		}
	}

	void MoveItemObjectToDown(GameObject item, float speed)
	{
		//speed 만 큼 아 래 
		item.transform.position = new Vector3 (
			item.transform.position.x,
			item.transform.position.y-speed,
			0
		);
	}	


	public void MoveBackGroundForTapSpeed()
	{
		//터 치 할 경 우 내 려 
	    if (isBoosterOn == false)
	    {
	        myCharacter.Climb ();
	    }
	
		if (myCharacter.IsEnableMoveAction())
		{
			BackgroundManager.Instance.MoveDown (speed.TapSpeed ());
			MoveDownItem (speed.TapSpeed ());
		}
	}


    void CheckItemMonsterIncereaseTime()
    {
		//몬 스 터 시 간 증 가 아 이 템 사 용 했 을 경 우 
        if(monsterTimeIncrease>0 && isMonsterTimeChange==false)
        {
            obstacleLifetime+=monsterTimeIncrease;
            isMonsterTimeChange=true;
        }
    }

    public void StartTapInput()
    {
		//터 치 가 입 력 됬 을 경 우 
        if (IsExistObstacle())
        {

            return;
        }
		MoveBackGroundForTapSpeed (); 
        ResetWaitInputTime();
    }

    public bool IsExistObstacle()
    {
		//적 이 있 나 없 나 

        return obstacles.Count != 0;
    }

	void AttackedByObstacles()
	{
		//시 간 동 안 몬 스 터 죽 이 지 못 하 면 체 력 깍 임 


		Earth.Instance.currHP -= GetSumOfMonsterHp ();

        if(Earth.Instance.currHP<=0)//게임 끝났음에도 몬스터가 내려오는걸 방지 하기 위해
        {
			SoundManager.Instance.SetSpecificSoundStopBySoundName ("Drag");
            for(int i=0;i<obstacles.Count;i++)
            {
                Obstacle temp=obstacles[i];
                Destroy(temp.gameObject);
            }

			DestroyDragInterraction ();

        }

        else
		{
			for (int i = 0; i < obstacles.Count; i++) {
				obstacles[i].StartFallDown();
			}
			Game.Instance.inputManager.dragObstacles.Clear ();
		}

		obstacles.Clear ();
	}


	int GetSumOfMonsterHp()
	{
		int sum = 0;
		for(int i=0;i<obstacles.Count;i++)
		{
			sum += obstacles [i].hp;
		}

		return sum;
	}


    public void ShotDragObstacles(List<Obstacle> dragObstacles)
    {
		//적 을 드 래 그 함 
        myCharacter.Attack();

        // TODO : 준비된 Obstacle들이 모두 dragObstacles에 있는지 체크 후 맞으면 파괴하는 로직 실행.

		SoundManager.Instance.SetSpecificSoundStopBySoundName ("Drag");
        if (dragObstacles.Count == obstacles.Count(x => x.isDragType))
        {
            for (int i = 0; i < dragObstacles.Count; i++)
            {
				//Debug.Log (dragObstacles [i].gameObject.name);
                ShotObstacle(dragObstacles[i], false);
				//TODO: 드 래 그 된 거 마 다 코 인 뜨 도 록
				//InstantiateCoin(dragObstacles[i].transform.position);
            }
        }


    }

    public void ShotObstacle(Obstacle touchObstacle, bool doAnimation = true)
    {//아이템 마지막 등장을 위해 마지막꺼 받아옴
		//적 을 클 릭 했 을 경 
        if(doAnimation)
        {
            myCharacter.Attack();
        }
		if(obstacles.Count==1 && touchObstacle.hp<=1)
            ItemSystemManager.Instance.SpawnItem(touchObstacle.transform.position);//아이템 스폰

        // TODO : Tap에 의해 Obstacle이 맞으면 파괴하는 로직 실행.

		touchObstacle.hp--;

	
		CheckObstacleHp (touchObstacle);

      

        if(obstacles.Count == 0)
        {
            //다 죽였는데 아이템이 사용됬을 경우
            if(monsterTimeIncrease>0)
            {
                monsterTimeIncrease = 0f;
                isMonsterTimeChange=false;
            }
			isMonsterInstantiate = false;
			isSetMonsterTimeValue = false; //몬 스 터 시 간 재 설 정 용ㅁ
            DisappearUISlider();
        }
    }

	private void CheckObstacleHp(Obstacle obs)
	{
		GameObject Heart = GameObject.FindGameObjectWithTag ("HeartSprite");

		if (obs.hp < 1) {
			if(obs.isBoss==true)
			{
				DeleteBossHpSprite (obs.hp,Heart);
			}

			obstacles.Remove (obs);
			Destroy (obs.GetComponent<BoxCollider> ());
			SoundManager.Instance.PlayActionSound ("Explosion", Game.Instance.gameInfo.Effect);
			obs.StartDie();
			InstantiateCoin (obs.transform.position);
		}

		else
		{
			Debug.Log ("hpasdfpsdfjsadfkj");
			if(Game.Instance.obstacleSpawnManager.GetMonsterNormalPatternNumber()==14)
			{
				SoundManager.Instance.PlayActionSound ("hitSound", Game.Instance.gameInfo.Effect);
				DeleteBossHpSprite (obs.hp,Heart);
			}
			//TODO sound add and hp- string
		}

	}

	private void DeleteBossHpSprite(int hp, GameObject Heart)
	{
		
		SpriteRenderer[] heartsSprite = Heart.GetComponentsInChildren<SpriteRenderer> ();

		heartsSprite [hp].enabled = false;

		switch(hp)
		{

		case 1:
			Heart.transform.localPosition = new Vector3 (2.45f, 0, 0);
			break;

		case 2:
			Heart.transform.localPosition = new Vector3 (1.7f, 0, 0);
			break;

		case 3:
			Heart.transform.localPosition = new Vector3 (0.9f, 0, 0);
			break;
		}

	}

    public void AddObstacle(Obstacle obstacle)
    {
        obstacles.Add(obstacle); //적 데 이 터 추 가 
    }

	public void SetIsDragTrue()
	{
		isDrag = true;
	}

	public void DestroyTitleUI()
	{
		DestroyImmediate(GameObject.Find("TitleUI"));
	}

    public void DestroyStartUI()
	{
		DestroyImmediate (GameObject.Find("StartUI"));
	}

	public void DestroyTutorialUI()
	{
		DestroyImmediate(GameObject.Find("TutorialUI"));
	}

    public void DestroyPlayUI()
	{

		DestroyImmediate (GameObject.Find ("PlayUI"));
	}

    public void DestroyGameOverUI()
	{
		//Destroy (GameObject.Find ("GameOverUI"));
		//Debug.Log("Destroy game over ui");
		Destroy( GameObject.Find ("GameOverUI"));
	}

    public void DestroyGamePauseUI()
	{
		DestroyImmediate(GameObject.Find("PauseUI"));
	}

    public void DestroyStoreUI()
	{
		DestroyImmediate(GameObject.Find("StoreUI"));
	}
		

	public void DestroyCreditUI()
	{
		DestroyImmediate (GameObject.Find ("CreditUI"));
	}

	public void DestroyLineRendererWhenConnected()
	{

	}

	public void ChangeTitleMenuCharacter()
	{
		// 선 택 한 캐 릭 터 에 따 라 첫 메 뉴 의 보 이 는 캐 릭 터 변 
		List<GameObject> models = new List<GameObject> ();
		Transform charArea = GameObject.Find ("CharacterArea").GetComponent<Transform>();
		foreach(Transform t in charArea)
		{
			models.Add (t.gameObject);
			t.gameObject.SetActive(false);
		}

		models [Game.Instance.gameInfo.SelectNumber].SetActive (true);
	}

	public void ChangeInstantiateCharacterSelectedNumber(int number)
	{
		//선 택 한 캐 릭 터 에 따 라 게 임 중 캐 릭 터 도 변 경
		Game.Instance.gameInfo.gameData.SaveMyCharacterNumber (number);
		gamePlayInstantiateObject[0]=(GameObject)Resources.Load(string.Format("Prefabs/{0}","Character"+number),typeof(GameObject));
	}


	void InstantiateCoin(Vector3 pos)
	{
		bool result =Random.Range (0f, 1.0f) <= 0.3f;
		if(result)
		{
			Instantiate (CoinObject, pos, Quaternion.identity);
		}
	}
}
