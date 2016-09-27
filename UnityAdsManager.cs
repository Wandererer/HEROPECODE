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

	private string gameId="";
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
        if(Advertisement.IsReady())
        {
			Advertisement.Show("rewardedVideo",new ShowOptions(){resultCallback=HandleAdResult});
			//Advertisement.Show(); //기 본 설 정 된 거
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
		initModelList ();
        switch(result)
        {
            //결과 처리
		case ShowResult.Finished:
			Debug.Log ("Finish ads");
			SetUIByAdResult (0);
			GameObject.Find ("SuccessLabel").GetComponent<UILabel> ().text = "Ad Success";
				break;

		case ShowResult.Skipped:
			Debug.Log ("skipped ads");
			SetUIByAdResult (2);
			GameObject.Find ("SkippedLabel").GetComponent<UILabel> ().text = "Ad Skipped";
				break;

		case ShowResult.Failed:
			Debug.Log ("Failed ads");
			SetUIByAdResult (1);
			GameObject.Find ("FailLabel").GetComponent<UILabel> ().text = "Ad Failed";
				break;
		}

    }


	public void initModelList()
	{
		Transform obj = GameObject.Find ("AdResultMenu").GetComponent<Transform> ();

		foreach(Transform t in obj)
		{
			models.Add (t.gameObject);
		}
	}

	private void SetUIByAdResult(int index)
	{
		models [index].SetActive(true);
	}
		
}
