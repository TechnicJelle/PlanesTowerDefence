using JetBrains.Annotations;
using Managers;
using UI;
using UnityEngine;

namespace Tower
{
	public class TowerContainer : MonoBehaviour
	{
		private SpriteRenderer _spriteRenderer;
		private RadialSelector _choiceSelector;

		[HideInInspector] [CanBeNull] public GameObject tower;

		private bool _buyMenuOpen;

		private void Awake()
		{
			_choiceSelector = GetComponent<RadialSelector>();
			_choiceSelector.TowerContainer = this;
			_spriteRenderer = GetComponent<SpriteRenderer>();
			OnMouseExit();
		}

		private void Start()
		{
			WaveManager.Instance.OnWaveStart += _choiceSelector.Hide;
		}

		private void OnMouseEnter()
		{
			if (!GameStateManager.Instance.IsRunning) return;
			if (WaveManager.Instance.IsWaveRunning) return;
			Color color = _spriteRenderer.color;
			color.a = 1f;
			_spriteRenderer.color = color;
		}

		private void OnMouseUp()
		{
			if (!GameStateManager.Instance.IsRunning) return;
			if (WaveManager.Instance.IsWaveRunning) return;
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
				Debug.Log("Sell or upgrade?");
				// TODO: Sell or upgrade tower menu
			}
		}

		private void OnMouseExit()
		{
			Color color = _spriteRenderer.color;
			color.a = 0f;
			_spriteRenderer.color = color;
		}
	}
}
