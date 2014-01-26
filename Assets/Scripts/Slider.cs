using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour
{
	public Vector3 fromPosition;
	public Vector3 toPosition;

	public float slideDuration;

	public bool reverseSlide;

	Transform T;
	bool slideCanceled;

	bool slideRunning;

	void Awake()
	{
		T = transform;
		fromPosition = T.localPosition;
	}
	
	public void UseFromPosition()
	{
		fromPosition = T.localPosition;
	}

	void OnEnable()
	{
		slideRunning = false;
	}

	void OnDisable()
	{
		slideRunning = false;
	}

	public void StartSlide()
	{
		if (!slideRunning)
		{
			slideRunning = true;
			StartCoroutine(DoSlide());
		}
	}

	IEnumerator DoSlide()
	{
		slideCanceled = false;

		float startTime = Time.time;
		float endTime = startTime + slideDuration;

		while (Time.time < endTime)
		{
			if (slideCanceled)
			{
				T.localPosition = fromPosition;
				break;
			}

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

		if (reverseSlide ^ slideCanceled)
		{
			T.localPosition = fromPosition;
		}
		else
		{
			T.localPosition = toPosition;
		}

		slideRunning = false;
	}

	public void CancelSlide()
	{
		slideCanceled = true;
		T.localPosition = fromPosition;
	}
}
