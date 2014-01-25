using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour
{
	public int cardHandIndex;
	public Renderer glowRenderer;
	public Material[] cardIconMaterials;

	Slider cardSelectedSlider;

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

	public bool selected;

	void Awake()
	{
		T = transform;
		initialScale = T.localScale;
		expandedScale = 1.1f * initialScale;
		cardSelectedSlider = GetComponent<Slider>();
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

	public void SetSelected(bool newSelected)
	{
		if (newSelected == selected) return;

		cardSelectedSlider.reverseSlide = !newSelected;

		cardSelectedSlider.StartSlide();

		selected = newSelected;
	}

	public void SetCardIcon(CardIcons icon)
	{
		int materialIndex = (int)icon;
		if (null == cardIconMaterials || materialIndex >= cardIconMaterials.Length)
		{
			Debug.LogError("No defined card icon material for card icon type: " + icon);
			return;
		}
		renderer.material = cardIconMaterials[materialIndex];
	}
}
