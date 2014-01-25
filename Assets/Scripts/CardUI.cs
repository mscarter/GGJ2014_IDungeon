using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour
{
	public int cardHandIndex;
	public Renderer glowRenderer;

	public bool glow
	{
		set
		{
			glowRenderer.gameObject.SetActive(value);
		}
	}

	Transform T;
	Vector3 initialScale;
	Vector3 expandedScale;

	void Awake()
	{
		T = transform;
		initialScale = T.localScale;
		expandedScale = 1.1f * initialScale;
		glowRenderer.gameObject.SetActive(false);
	}

	public void SetExpanded(bool expanded)
	{
		if (expanded)
		{
			T.localScale = expandedScale;
		}
		else
		{
			T.localScale = initialScale;
		}
	}
}
