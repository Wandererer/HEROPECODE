using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;

public class UnityAdsManager : NEMonoBehaviour {
	//unity ads enabled안 되있으면 오류 발생 되므로 enable 시켜야함
    private static UnityAdsManager instance;

    public static UnityAdsManager Instance
    {
        get { return instance; }
        set { instance=value; }
    }
	#if UNITY_IOS
	private string gameId="1118783";
	#else
	private string gameId="1118782";
	#endif

	private List<GameObject> models;
    public override void Awake()
    {
        base.Awake();
        instance=this;
       Advertisement.Initialize(gameId);
		models = new List<GameObject> ();
    }

    public void ShowAd()
    {
		if(Advertisement.IsReady("rewardedVideo"))
        {
			Advertisement.Show("rewardedVideo",new ShowOptions(){resultCallback=HandleAdResult});
			//Advertisement.Show(); //기 본 설 정 된 거
		}

		else
		{
			Game.Instance.gameScene.CratePopUpUiForAdIsNotReady ();
		}

        /*
         if(Advertisement.IsReady("Vidio Integration ID))
        {
			//Advertisement.Show("Video Integration ID",new ShowOptions(){resultCallback=HandleAdResult});
			Advertisement.Show(); //기 본 설 정 된 거
		}


         */
    }

    private void HandleAdResult(ShowResult result)
    {
		
        switch(result)
        {
            //결과 처리
		case ShowResult.Finished:
			Debug.Log ("Finish ads");

			Game.Instance.gameScene.CreatePoPupUIForAdResultFinished ();
				break;

		case ShowResult.Skipped:
			Debug.Log ("skipped ads");
		
			Game.Instance.gameScene.CratePopUpUiForAdResultSkipped ();
				break;

		case ShowResult.Failed:
			Debug.Log ("Failed ads");
			
			Game.Instance.gameScene.CreatePopUpUiForAdResultFailed ();
				break;
		}

    }



}
