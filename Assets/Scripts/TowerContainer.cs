using JetBrains.Annotations;
using UnityEngine;

public class TowerContainer : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;
	private UIRadialSelector _choiceSelector;

	[HideInInspector] [CanBeNull] public GameObject tower;

	private bool _buyMenuOpen;

	private void Awake()
	{
		_choiceSelector = GetComponent<UIRadialSelector>();
		_choiceSelector.TowerContainer = this;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		OnMouseExit();
	}

	private void OnMouseEnter()
	{
		Color color = _spriteRenderer.color;
		color.a = 1f;
		_spriteRenderer.color = color;
	}

	private void OnMouseUp()
	{
		if (tower == null)
		{
			if (_buyMenuOpen)
			{
				_choiceSelector.Hide();
				_buyMenuOpen = false;
			}
			else
			{
				_choiceSelector.Show();
				_buyMenuOpen = true;
			}
		}
		else
		{
			// TODO: Sell tower
		}
	}

	private void OnMouseExit()
	{
		Color color = _spriteRenderer.color;
		color.a = 0f;
		_spriteRenderer.color = color;
	}
}
