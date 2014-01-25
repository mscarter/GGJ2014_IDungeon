using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour
{
	public Vector3 fromPosition;
	public Vector3 toPosition;

	public float slideDuration;

	public bool reverseSlide;

	Transform T;

	void Awake()
	{
		T = transform;
		fromPosition = T.localPosition;
	}
	
	public void UseFromPosition()
	{
		fromPosition = T.localPosition;
	}

	public void StartSlide()
	{
		StartCoroutine(DoSlide());
	}

	IEnumerator DoSlide()
	{
		float startTime = Time.time;
		float endTime = startTime + slideDuration;

		while (Time.time < endTime)
		{
			var t = (Time.time - startTime) / slideDuration;

			if (reverseSlide)
			{
				T.localPosition = Vector3.Lerp(toPosition, fromPosition, t);
			}
			else
			{
				T.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
			}
			yield return null;
		}

		if (reverseSlide)
		{
			T.localPosition = fromPosition;
		}
		else
		{
			T.localPosition = toPosition;
		}
	}
}
