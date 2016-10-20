using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
	LineRenderer Line; // 라인렌더러
	public Transform Target; // 전기가 도달할 목적지의 Transform 오브젝트
	public int Steps ; // 전기가 꺾어지는 지점의 개수
	public float Width ; // 전기가 꺾어지는 간격 (값이 클수록 변화가 크다)
	public float Delay ; // 각 지점의 값을 리셋하는 타이머 (적을수록 빨리 변화한다)
	void Start()
	{
		Line = GetComponent<LineRenderer>(); // 라인렌더러 컴포넌트를 가져온다.
		Line.SetVertexCount(Steps); // 전기를 구성할 버텍스 개수를 지정한다.
		StartCoroutine(UpdateLightning()); // 전기의 방향을 변화시킬 코루틴을 실행한다.
	}

	IEnumerator UpdateLightning()
	{
		while (true && Target!=null)
		{
			// 버텍스와 버텍스 사이의 간격을 구한다.
			float dist = Vector3.Distance(transform.position, Target.position) / Steps;

			// 시작점과 끝점을 제외한 나머지 지점만 변화시킨다.
			for (int i = 1; i < Steps - 1; i++)
			{
				// 시작점부터 끝점까지 dist 간격으로 이동시킨다.
				Vector3 pos = Vector3.MoveTowards(transform.position, Target.position, i * dist);
				pos += transform.up * Random.Range(-Width, Width); // 상하로 랜덤하게 변화시킨다.
				pos += transform.right * Random.Range(-Width, Width); // 좌우로 랜덤하게 변화시킨다.
				Line.SetPosition(i, pos); // 변화된 버텍스 좌표를 설정한다.
			}
			yield return new WaitForSeconds(Delay); // 지정된 시간만큼 딜레이시킨다.
		}
	}

	void Update()
	{
		if(Target!=null)
		{
			Line.SetPosition(0, transform.position); // 시작점의 좌표를 고정한다.
			Line.SetPosition(Steps - 1, Target.position); // 끝점의 좌표를 목표물에 고정한다.
		}
	}


}
