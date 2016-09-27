using UnityEngine;
using System.Collections;

public class Earth : NEMonoBehaviour {

	public UISlider m_hpBar; //hp bar 용
	int m_currHP; //현 재 체 력
	int m_maxHP; // 최 대 체 력 

	private static Earth instance;

	public static Earth Instance
	{
		get { return instance; }
	}

	public override void Awake ()
	{
		base.Awake ();
		instance = this;
		m_currHP = 2;
		m_maxHP = 2;
	}

	void Start()
	{
		
	}

	public override void OnDestroy ()
	{
		base.OnDestroy ();
	}


	public void SetDamage(int damage)
	{
		if(m_currHP<0)
		{
			return;
		}

		m_currHP -= damage;
		m_hpBar.sliderValue = (float)m_currHP / (float)m_maxHP;
	}
		

	public void Reset()
	{
		m_currHP = 2;
		m_maxHP = 2;
	}

	public int currHP
	{
		get { return m_currHP; }
		set { m_currHP = value; }
	}

	public int maxHP
	{
		get {return m_maxHP; }
		set { m_maxHP = value; }
	}


}
