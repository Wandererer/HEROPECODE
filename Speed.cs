using UnityEngine;
using System;
public class Speed  {

	float startValue=1f; //시 작 값
	float maxValue=3f; // 최 대 값

	float climbValue=0.5f; // tap 시 증 가

    float slope=1500f;

	float normalSpeed;
	public void SetNormalSpeed(float time)
	{
		//각 초 마 다 기 준 속 도 지 
		if (Mathf.Log (time, slope) == -Mathf.Infinity)
			normalSpeed = startValue;
		else
		normalSpeed = ( Mathf.Log(time,slope) )+ startValue;
	}

	public float AutoSpeed()
	{
		//자 동 으 로 올 라 가 는 속 도 반 환 
		float tempSpeed=normalSpeed;

		if (tempSpeed <= maxValue)
			return tempSpeed;
		else
			return maxValue;
		
	}

	public float TapSpeed()
	{
		// 탭 시 올 라 가 는 속 도 반 환
		float tempSpeed=normalSpeed+ (normalSpeed * climbValue);

        return tempSpeed;
	}
}
