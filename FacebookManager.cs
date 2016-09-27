using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FacebookManager : NEMonoBehaviour {
	//TODO: 나중을 위해 다른기능들도 작성 
    private static FacebookManager instance;

    public static FacebookManager Instance
    {
        get { return instance; }
        set { instance=value; }
    }
		
	private bool isLogedIn;
	public string AppLink;


    public override void Awake()
    {
        base.Awake();
		instance = this;
		FB.Init (InitCallBack); //페이스북 api 시작 sdk 씬에 로드 시 킴 
		//원 래 는 init 여 기 에 
    }

	public void InitFaceBook()
	{
		//TODO: 로그인과 앱링크 가져오는 용도 
		if(!FB.IsInitialized)
		{
			Login ();
		}
		FB.GetAppLink(GetAppLinkFromAPI);
	//	isLogedIn = FB.IsLoggedIn;
	}

	void InitCallBack()
	{
		Debug.Log ("Fb has been init");
	}

	public bool IsFacebookLogIn()
	{
		return isLogedIn;
	}

	public void Login()
	{
		if (!FB.IsLoggedIn) 
		{
			//FB.LogInWithReadPermissions (new List<string> { "user_friends" }, LogInCallBack); //읽 어 오 기 만 가 능 public_profile 도 가능 
			FB.LogInWithReadPermissions (new List<string> { "public_profile" }, LogInCallBack);
		}
		isLogedIn = true;
	}

	void LogInCallBack(ILoginResult result)
	{
		if(result.Error==null)
		{
			Debug.Log ("FB has Logged in");	
			ShowPicture ();
		}
		else
		{
			Debug.Log ("Error doing login: "+result.Error);
			Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
		}
	}

	void ShowPicture()
	{
		if(FB.IsLoggedIn)
		{
			//FB.API ("me/picture?width100&height=100", HttpMethod.GET, ProfilePictureCallBack); //자기 사진 가져 옴 
			//FB.API ("me?fields=first_name", HttpMethod.GET, NameCallBack); // 내 이 름 가 져 옴 
			//FB.API ("me/friends", HttpMethod.GET, FriendCallBack);// 내 친 구 들 리 스 트 가 지 고 옴 
			FB.GetAppLink(GetAppLinkFromAPI);
		}
	}

	void GetAppLinkFromAPI(IAppLinkResult result)
	{
		if (!string.IsNullOrEmpty (result.Url)) {
			AppLink = result.Url;
		} else
			AppLink = "google.ljlkj";
	}

	void ProfilePictureCallBack(IGraphResult result)
	{
		Texture2D image = result.Texture;
		Sprite.Create (image, new Rect (0, 0, 100, 100), new Vector2 (0.5f, 0.5f));
	}

	void NameCallBack(IGraphResult result)
	{
		IDictionary<string,object> profile = result.ResultDictionary;
		//name.GetComponent<UILabel> ().text = profile ["first_name"].ToString();
	}

	public void InviteRequest()
	{
		FB.AppRequest(message: "이 게 임 해 봐 ",title: "이 거 엄 청 나 다 ");
	}

	public void Share()
	{
		//FB.ShareLink (new System.Uri ("http://게임 주 소 "), "게임 쩔 어 용", "게임 설 명 ", new System.Uri ("pictureurl"));
		if (FB.IsLoggedIn) {
			//첫 uri에 AppLink 나중에 들어 가야 함 
			//두번째는 띄울 이미지 사
			//GameObject.Find("Label").GetComponent<UILabel>().text="LogIn"; //확 인 용 
			FB.FeedShare (string.Empty,
				new System.Uri ("https://play.google.com/store/apps/details?id=com.mobirix.devilbreaker"), 
				"Herope",
				"용사가 지구를 지키며 우주로 나아간",
				"난 "+Game.Instance.gameScene.meter.ToString("f1")+"m 올라갔어~~ 이 게임 해서 기록 깨~~",
				new System.Uri ("http://image.rakuten.co.jp/mischief/cabinet/ss16_3/10047666.jpg"),
				string.Empty,
				ShareCallBack
			);
		}
	}

	public void AppInvite()
	{
		FB.Mobile.AppInvite(
			new System.Uri("AppLink"),
			new System.Uri("img uri"),
			InviteCallBack
		);
	}

	void FriendCallBack(IGraphResult result)
	{
		IDictionary<string, object> data = result.ResultDictionary;
		List<object> friends = (List<object>)data ["data"];
		foreach(object obj in friends)
		{
			Dictionary<string, object> dicto = (Dictionary<string ,object>)obj;
			Debug.Log (dicto ["name"].ToString () + " , " + dicto ["id"].ToString ());
		}
	}

	void ShareCallBack(IResult result)
	{//TODO: 나중에 share 끝 나면 다 시 렌 더 링 키 게 
		if(result.Cancelled)
		{
			Debug.Log ("share cancelled");
			Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
			//GameObject.Find("Label").GetComponent<UILabel>().text="Share Cancelled"; //확인용 나 중 에 삭 제 
		}
		else if(!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log ("Error on share");
			Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
			//GameObject.Find("Label").GetComponent<UILabel>().text="SError on share";
		}
		else if(!string.IsNullOrEmpty(result.RawResult))
		{
			Debug.Log ("Success");
			Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
			//GameObject.Find("Label").GetComponent<UILabel>().text="Success";
		}
	}

	void InviteCallBack(IResult result)
	{
		if(result.Cancelled)
		{
			Debug.Log ("Invite cancelled");
		}
		else if(!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log ("Error on Invite");
		}
		else if(!string.IsNullOrEmpty(result.RawResult))
		{
			Debug.Log ("Success On Invite");
		}
	}


	public void ShareWithUsers()
	{
		FB.AppRequest (
			"이거 기 록 한 번 깨 봐",
			null,
			new List<object>(){"app_users"},
			null,
			null,
			null,
			null,
			ShareWithUsersCallBack
		);

	}

	void ShareWithUsersCallBack(IAppRequestResult result)
	{
		if(result.Cancelled)
		{
			Debug.Log ("Chalenge cancelled");
			Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
		}
		else if(!string.IsNullOrEmpty(result.Error))
		{
			Debug.Log ("Error on Chalenge");
			Game.Instance.gameScene.CharacterRenderingOnForExitPopUp ();
		}
		else if(!string.IsNullOrEmpty(result.RawResult))
		{
			Debug.Log ("Success On Chalenge");
		}
	}
}
