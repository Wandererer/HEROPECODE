using System.Collections.Generic;
using UnityEngine;

public class SoundManager : NEMonoBehaviour {

	private static SoundManager instance; //싱 글 톤 선 언 

	public static SoundManager Instance
	{
		get {return instance;}
		set {instance = value;}
	}
	AudioClip temp;
	private List<AudioSource> actionSoundList = new List<AudioSource> (); //효 과 음 리 스 트
	private List<AudioSource> bgmList = new List<AudioSource> ();// 배 경 음 악 리 스 트

	private AudioSource[] audioSource;


    public bool buttonSound; //버 튼 사 운 드 하 는 지 마 는 지 

	private float volume=1.0f;

	public override void Awake ()
	{
		base.Awake ();
		instance = this;
	}

	// Use this for initialization
	void Start () {
		audioSource = this.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetActionSoundFalse()
	{
		//액 션 사 운 드 off
		for (int i = 0; i < actionSoundList.Count; i++) {
			if(actionSoundList[i].isPlaying)
			{
				actionSoundList [i].volume = 0f;
			}
		}
        buttonSound=false;
	}




    public void SetBgmSoundFasle()
	{
		// 배 경 사 운 드 끔 
		for (int i = 0; i < bgmList.Count; i++) {
			if(bgmList[i].isPlaying)
			{
				bgmList [i].volume = 0f;
			}
		}
        Debug.Log("Bgm false");

	}


	public AudioSource FindEmptyAudioSource()
	{
		//빈 오 디 오 소 스 찾 
		AudioSource temp = null;
		for (int i = 0; i < audioSource.Length; i++) {
			if(audioSource[i].clip==null)
			{
				temp = audioSource [i];
				break;
			}
		}

		return temp;
	}

	public void SetActionSoundTrue()
	{
		//액 션 사 운 드 다 시 
        buttonSound=true;

		for (int i = 0; i < actionSoundList.Count; i++)
        {
			if(actionSoundList[i].isPlaying)
			{
				actionSoundList [i].volume = volume;
			}
		}
	}

	public void SetBgmSoundTrue()
	{
		//배 경 사 운 드 다 시 
		for (int i = 0; i < bgmList.Count; i++)
		{
			if(bgmList[i].isPlaying)
			{
				bgmList [i].volume = volume;
			}
		}
        Debug.Log("Bgm True");
	}

	public void SetPlaySoundStop()
	{
		//모 든 사 운 드 정 
		for (int i = 0; i < bgmList.Count; i++)
		{
			if(bgmList[i].isPlaying)
			{
				bgmList [i].Stop ();
			}
		}

		for (int i = 0; i < actionSoundList.Count; i++) 
		{
			if(actionSoundList[i].isPlaying)
			{
				actionSoundList [i].Stop ();
			}
		}
	}

	public void SetPauseSoundPlay()
	{
		//일 시 정 지 사 운 드 다 시 시 
		for (int i = 0; i < bgmList.Count; i++)
		{
				bgmList [i].Play ();
		}



		for (int i = 0; i < actionSoundList.Count; i++) 
		{
				actionSoundList [i].Play ();
		}
	}

	public void SetAudioSourceClipNull()
	{
		//플 레 이 가 아 닌 건 오 디 오 소 스 클 립 null로 
		for (int i = 0; i < audioSource.Length; i++) {
			if (audioSource [i].isPlaying)
				continue;

			audioSource [i].clip = null;
		}
	}

	public void PlayActionSound(string key, bool isOn)
	{
		//액 션 사 운 드 가 져 와 서 실 행 똑 같 은 게 있 을 경 우 그 걸 시 작 
		if (key == null || key == "")
			return;

	//	Debug.Log (isOn);

        bool isReplite=false;
		object sound = Resources.Load (string.Format ("Sounds/{0}", key), typeof(AudioClip));
		AudioClip audioClip = (AudioClip)sound;
		AudioSource tempAudioSource = FindEmptyAudioSource ();

        for(int i=0;i<actionSoundList.Count;i++)
        {//같은게 있을 경우 그걸로 씀
            if(actionSoundList[i].clip==audioClip)
            {
                tempAudioSource=actionSoundList[i];
                isReplite=true;
                break;
            }
        }

		//현재 볼륨이 On 상태이면 볼륨을 키고 아니면 볼륨을 끈상태로 추가
        if(isOn)
            tempAudioSource.volume = volume;
        else
            tempAudioSource.volume = 0;


		tempAudioSource.spatialBlend = 0;
		tempAudioSource.ignoreListenerVolume = true;
		tempAudioSource.Stop ();
		tempAudioSource.clip = audioClip;
		tempAudioSource.loop = false;
		tempAudioSource.playOnAwake = false;

        if(!isReplite)
		{
			actionSoundList.Add(tempAudioSource);
		}
		tempAudioSource.PlayOneShot (tempAudioSource.clip);
	}

	public void DestroyActionSoundIfNotPlay()
	{
		// 플 레 이 아 닌 액 션 사 운 드 삭 제 및 리 스 트 에 서 삭 제 
		for (int i = 0; i < actionSoundList.Count; i++) {
			if (actionSoundList [i].isPlaying)
				continue;

			actionSoundList.RemoveAt (i);
		}

		SetAudioSourceClipNull ();
	}

	public void PlayBgmSound(string key, bool isOn)
	{
		//배 경  사 운 드 가 져 와 서 실 행 똑 같 은 게 있 을 경 우 그 걸 시 작 
        if (key == null || key == "")
            return;

        bool isReplite=false;

		object sound = Resources.Load(string.Format("Sounds/Bgm/{0}", key), typeof(AudioClip));
		AudioClip audioClip = (AudioClip)sound;
        AudioSource tempAudioSource = FindEmptyAudioSource ();

        for (int i=0;i<bgmList.Count;i++)
        {
            if(bgmList[i].clip==audioClip)
            {//똑같은게 있을 경우 리턴
                CheckVolumeOnOff(isOn,bgmList[i]);
                bgmList[i].Play();//다시 시작
                return;
            }
        }


        //현재 볼륨이 On 상태이면 볼륨을 키고 아니면 볼륨을 끈상태로 끔
        if(isOn)
			tempAudioSource.volume = volume;
        else
            tempAudioSource.volume = 0;

		tempAudioSource.spatialBlend = 0;
		tempAudioSource.ignoreListenerVolume = true;
		tempAudioSource.Stop ();
		tempAudioSource.clip = audioClip;
		tempAudioSource.loop = true;
		tempAudioSource.playOnAwake = false;

		if(!isReplite)
		{
			bgmList.Add(tempAudioSource);
		}
		tempAudioSource.Play ();
	}

    void CheckVolumeOnOff(bool isOn,AudioSource audio)
    {
		//볼 륨 꺼 진 상 태 에 따 라 볼 륨 설 정 
        if(isOn)
            audio.volume = volume;
        else
            audio.volume = 0;
    }

	public void DestroyBgmSoundIfNotPlay()
	{
		// 플 레 이 중 이 지 않 으 면 오 디 오 클 립 없 애 고 리 스 트 에 서 도 없 앰
		for (int i = 0; i < bgmList.Count; i++) {
			if (bgmList [i].isPlaying)
				continue;
			bgmList.RemoveAt (i);
		}

		SetAudioSourceClipNull ();
	}
}
