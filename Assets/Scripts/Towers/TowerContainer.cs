using JetBrains.Annotations;
using Managers;
using UI;
using UnityEngine;

namespace Towers
{
	public class TowerContainer : MonoBehaviour
	{
		private SpriteRenderer _highlightSprite;
		private TowerSelector _towerSelector;
		// private RadialSelector _upgradeSelector;

		[CanBeNull] private GameObject _tower;

		private void Awake()
		{
			_towerSelector = GetComponent<TowerSelector>();

			// _upgradeSelector = GetComponent<RadialSelector>();
			// _upgradeSelector.TowerContainer = this;

			_highlightSprite = GetComponent<SpriteRenderer>();
			OnMouseExit(); //disable any highlights from the start
		}

		private void Start()
		{
			GameStateManager.Instance.OnResetGame += ResetTower;

			WaveManager.Instance.OnWaveStart += _towerSelector.Hide; //disable any open buy menus
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
			if (!_towerSelector.IsOpen)
				OnMouseEnter();
		}

		private void OnMouseUp()
		{
			if (!GameStateManager.Instance.IsRunning) return;
			if (WaveManager.Instance.IsWaveRunning) return;
			if (_tower == null)
			{
				if (_towerSelector.IsOpen)
					_towerSelector.Hide();
				else
					_towerSelector.Show();
			}
			else
			{
				// if(_upgradeSelector.IsOpen)
				// 	_upgradeSelector.Hide();
				// else
				// 	_upgradeSelector.Show();
			}
		}

		private void OnMouseExit()
		{
			Color color = _highlightSprite.color;
			color.a = 0f;
			_highlightSprite.color = color;
		}

		public void SetTower(GameObject towerPrefab)
		{
			if(_tower != null) Debug.LogError("Tower already exists!");
			_tower = Instantiate(towerPrefab, transform);
		}

		private void ResetTower()
		{
			Destroy(_tower);
			_tower = null;
		}
	}
}
