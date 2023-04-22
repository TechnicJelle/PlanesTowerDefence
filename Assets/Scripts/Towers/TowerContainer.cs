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
		private TowerSelector _towerSelector;
		private UpgradeSelector _upgradeSelector;

		[CanBeNull] private GameObject _containedTower;

		private void Awake()
		{
			_towerSelector = GetComponent<TowerSelector>();
			_upgradeSelector = GetComponent<UpgradeSelector>();

			_highlightSprite = GetComponent<SpriteRenderer>();
			OnMouseExit(); //disable any highlights from the start
		}

		private void Start()
		{
			GameStateManager.Instance.OnResetGame += RemoveContainedTower;

			WaveManager.Instance.OnWaveStart += _towerSelector.Hide; //disable any open buy menus
			WaveManager.Instance.OnWaveStart += _upgradeSelector.Hide; //disable any open upgrade menus
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
			//and there's not already a selector panel open
			if (!_towerSelector.IsOpen && !_upgradeSelector.IsOpen)
				OnMouseEnter();
		}

		private void OnMouseUp()
		{
			if (!GameStateManager.Instance.IsRunning) return;
			if (WaveManager.Instance.IsWaveRunning) return;
			if (_containedTower == null)
			{
				if (_towerSelector.IsOpen)
					_towerSelector.Hide();
				else
					_towerSelector.Show();
			}
			else
			{
				if (_upgradeSelector.IsOpen)
					_upgradeSelector.Hide();
				else
					_upgradeSelector.Show();
			}
		}

		private void OnMouseExit()
		{
			Color color = _highlightSprite.color;
			color.a = 0f;
			_highlightSprite.color = color;
		}

		public void BuyTower([NotNull] GameObject towerPrefab)
		{
			if (_containedTower != null) Destroy(_containedTower); //destroy potential old tower...
			_containedTower = Instantiate(towerPrefab, transform); //...to replace it with the new one

			if (_containedTower == null) throw new NullReferenceException("Tower was null even after instantiating a new one!");

			Tower tower = _containedTower.GetComponent<Tower>();
			PlayerStatsManager.Instance.Buy(tower.price);
			_upgradeSelector.SetNextUpgrade(tower.GetNextUpgrade());
			_upgradeSelector.UpdateButtons();
		}

		public void SellContainedTower()
		{
			if (_containedTower == null) throw new Exception("Tried to sell a tower that doesn't exist!");
			Tower tower = _containedTower.GetComponent<Tower>();
			PlayerStatsManager.Instance.Sell(tower.price);
			RemoveContainedTower();
		}

		private void RemoveContainedTower()
		{
			Destroy(_containedTower);
			_containedTower = null;
		}
	}
}
