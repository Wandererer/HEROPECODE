using UnityEngine;
using System.Collections;
public class NGUIApi : MonoBehaviour {

    UIPlaySound[] uiSounds;

    void Start()
    {
        uiSounds=GetComponentsInChildren<UIPlaySound>();
    }

    void Update()
    {
       //Debug.Log(SoundManager.Instance.buttonSound);
        VolumeOnOffButtonSound();
    }

    void VolumeOnOffButtonSound()
    {
        //button sound는 따로 버튼마다 있으므로 버튼마다 접근하여 제어해 주기 위해
       if(SoundManager.Instance.buttonSound==true)
        {
            for(int i=0;i<uiSounds.Length;i++)
            {
                uiSounds[i].volume=1f;
            }
        }
        else
        {
            for(int i=0;i<uiSounds.Length;i++)
            {
                uiSounds[i].volume=0f;
            }
        }
    }

	public void OnClickStartButton()
	{
		//스 타 트 버 튼 누 른 경 
		Game.Instance.gameScene.gameState = GameState.Play;
		Game.Instance.gameScene.DestroyStartUI ();
	}

	public void OnClickBgmOnOffToggleButton()
	{
		//BGM on off 버 튼 클 릭 
		UIToggle current = UIToggle.current;
	
        if(current.name=="BGM Button") //name으로 접근 안하면 토글버튼 멋대로 접근함
		{
            Debug.Log("BgmBUtton Clicked");
			if (current.value == false)
			{
				//비 활 성 화 상 태
				SoundManager.Instance.SetBgmSoundFasle();
			}
			else
			{
				//활 성 화 상 태 checkmark on
				SoundManager.Instance.SetBgmSoundTrue();
			}
			Game.Instance.gameInfo.gameData.SaveBgmToggleInfo(current.value);

		}

	}

	public void OnClickEffectOnOffToggleButton()
	{
		//effect on off 버 튼 클 릭 
		UIToggle current = UIToggle.current;//name으로 접근 안하면 토글버튼 멋대로 접근함
        if(current.name=="Effect Button")
		{
			if (current.value == false)
			{
				//비 활 성 화 상 태
				SoundManager.Instance.SetActionSoundFalse();
			}
			else
			{
				//활 성 화 상 태 checkmark on
				SoundManager.Instance.SetActionSoundTrue();
			}
			Game.Instance.gameInfo.gameData.SaveEffectToggleInfo(current.value);
		}
	}

	public void OnClickCharacterChangeButton()
	{
		//캐 릭 터 변 경 클 릭 시
		Debug.Log ("CharacterChange Click");
		Game.Instance.gameScene.DestroyStartUI ();
		Game.Instance.gameScene.gameState = GameState.Store;
	}

	public void OnClickCreditButton()
	{
		Debug.Log ("Credit Click");
		Game.Instance.gameScene.DestroyStartUI ();
		Game.Instance.gameScene.gameState = GameState.Credit;
	}

	public void OnClickPauseButton()
	{
		Debug.Log ("Pause Click");
		Game.Instance.gameScene.SetPrevGameState (Game.Instance.gameScene.gameState);
		if (Game.Instance.gameScene.gameState == GameState.Fall)
			Game.Instance.gameScene.SetIsKinemeticTrueForPauseWhileFalling ();
		Game.Instance.gameScene.CameraDepthFarForPause ();
		Game.Instance.gameScene.DestroyPlayUI ();
		Game.Instance.gameScene.gameState = GameState.Pause;
	}

	public void OnClickReplayButtonInPause()
	{
		Game.Instance.gameScene.ResetObjectForReplyGameInPause();
		Debug.Log ("Replay Click");
        Game.Instance.gameInfo.Reset();
		Game.Instance.gameScene.ResetValue ();
		Game.Instance.gameScene.DestroyGamePauseUI ();
		Game.Instance.gameScene.CamearaDepthNormalForPlay();
        Game.Instance.gameScene.gameState = GameState.Play;

	}

    public void OnClickReplayButtonInGameOver()
    {
		Game.Instance.gameScene.RemoveObjectForRestartGame ();
        Game.Instance.gameScene.ResetValue ();
        Game.Instance.gameScene.DestroyGameOverUI ();
        Game.Instance.gameScene.gameState = GameState.Play;
    }

	public void OnClickContinueButtonInPauseUI()
	{
		Debug.Log ("Continue Click");

		Game.Instance.gameScene.DestroyGamePauseUI ();
		Game.Instance.gameScene.CamearaDepthNormalForPlay();
		Game.Instance.gameScene.gameState = Game.Instance.gameScene.GetPrevGameState ();

	}

	public void OnClickCoinMinuseButton()
	{
		Debug.Log ("CoinMinuse Click");
		if (Game.Instance.gameInfo.Money >= 10) {
			Game.Instance.gameInfo.gameData.MinusMoney (10);
			Game.Instance.gameScene.ResetValueForContinue ();
			Game.Instance.gameScene.DestroyGameOverUI ();
			Game.Instance.gameScene.RemoveObjectForRestartGame ();
			Game.Instance.gameScene.isContinue = true;
			Game.Instance.gameScene.gameState = GameState.Play;
		}

	}

	public void OnClickADButton()
	{
		Debug.Log ("AD Click");
		Game.Instance.gameScene.CharacterRendreingOffForPopUp();
		Game.Instance.gameScene.gameState = GameState.Ad;
		Game.Instance.gameScene.DestroyGameOverUI ();
		UnityAdsManager.Instance.ShowAd ();
	}

	public void OnClickShareButton()
	{
		Debug.Log ("Share Click");
		//FacebookManager.Instance.InitFaceBook ();
		Game.Instance.gameScene.CharacterRendreingOffForPopUp();
		FacebookManager.Instance.Login ();
		FacebookManager.Instance.Share ();
	}

	public void OnClickBackButton()
	{
		Debug.Log ("Back Click");
		Game.Instance.gameScene.DestroyStoreUI ();
		Game.Instance.gameScene.gameState = GameState.Ready;

	}

	public void OnClickSelectOrBuyButton()
	{
		Debug.Log ("SelectOrBuy Click");
		StoreManager.Instance.SelectOrBuy ();
	}

	public void OnClickLeftButton()
	{
		Debug.Log ("Left Click");
		StoreManager.Instance.Left ();
	}

	public void OnClickRightButton()
	{
		Debug.Log ("Right Click");
		StoreManager.Instance.Right ();
	}

	public void OnClickExitButton()
	{
		Debug.Log ("Exit Click");
        Game.Instance.gameInfo.gameData.SaveData(Game.Instance.gameInfo.gameData.saveData);
		Application.Quit ();
	}

    public void OnClickGoToStartMenu()
    {
        Debug.Log("Start Menu");
		Game.Instance.gameScene.RemoveObjectForRestartGame ();
        Game.Instance.gameScene.gameState=GameState.Ready;
        Game.Instance.gameScene.DestroyGameOverUI ();
       Game.Instance.gameScene.ResetValue ();
		Game.Instance.gameScene.CreateGameStartGUI();
    }

	public void Login()
	{
		FacebookManager.Instance.Login ();
	}
		

	public void ExitAdResultMenu()
	{
		Game.Instance.gameScene.gameState = GameState.End;
		Game.Instance.gameScene.DestroyAdResultUI ();
		Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
		Game.Instance.gameScene.CreateGameEndMenu ();
	}

	public void ContinueInAdResult()
	{
		Game.Instance.gameScene.DestroyAdResultUI ();
		Game.Instance.gameScene.gameState = GameState.Play;
	}


	public void OnClickBackButtonInCredit()
	{
		Game.Instance.gameScene.gameState = GameState.Ready;
		Game.Instance.gameScene.DestroyCreditUI ();
	}



}
