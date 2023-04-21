using System;
using JetBrains.Annotations;
using Managers;
using UI;
using UnityEngine;

namespace Towers
{
	public class TowerContainer : MonoBehaviour
	{
		private SpriteRenderer _highlightSprite;
		private RadialSelector _choiceSelector;

		[HideInInspector] [CanBeNull] public GameObject tower;

		private void Awake()
		{
			_choiceSelector = GetComponent<RadialSelector>();
			_choiceSelector.TowerContainer = this;
			_highlightSprite = GetComponent<SpriteRenderer>();
			OnMouseExit(); //disable any highlights from the start
		}

		private void Start()
		{
			GameStateManager.Instance.OnResetGame += ResetTower;

			WaveManager.Instance.OnWaveStart += _choiceSelector.Hide; //disable any open buy menus
			WaveManager.Instance.OnWaveStart += OnMouseExit; //disable any active highlights
		}

		private void OnMouseEnter()
		{
			if (!GameStateManager.Instance.IsRunning) return;
			if (WaveManager.Instance.IsWaveRunning) return;
			Color color = _highlightSprite.color;
			color.a = 1f;
			_highlightSprite.color = color;
		}

		private void OnMouseOver()
		{
			//for when the mouse is already over the container when it becomes active
			if (!_choiceSelector.IsOpen)
				OnMouseEnter();
		}

		private void OnMouseUp()
		{
			if (!GameStateManager.Instance.IsRunning) return;
			if (WaveManager.Instance.IsWaveRunning) return;
			if (tower == null)
			{
				if (_choiceSelector.IsOpen)
					_choiceSelector.Hide();
				else
					_choiceSelector.Show();
			}
			else
			{
				Debug.Log("Sell or upgrade?");
				// TODO: Sell or upgrade tower menu
			}
		}

		private void OnMouseExit()
		{
			Color color = _highlightSprite.color;
			color.a = 0f;
			_highlightSprite.color = color;
		}

		private void ResetTower()
		{
			Destroy(tower);
			tower = null;
		}
	}
}
