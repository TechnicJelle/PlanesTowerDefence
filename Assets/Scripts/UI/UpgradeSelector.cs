using System;
using JetBrains.Annotations;
using Towers;
using UnityEngine;

namespace UI
{
	public class UpgradeSelector : RadialSelector
	{
		[SerializeField] private SellChoice sellChoice;
		[SerializeField] private UpgradeChoice upgradeChoice;

		private void Start()
		{
			IChoice[] choices = {upgradeChoice, sellChoice,};

			Init(choices);
		}

		protected override void OnChoiceClicked(IChoice choice)
		{
			base.OnChoiceClicked(choice);
			switch (choice)
			{
				case UpgradeChoice upChoice:
					if (upChoice.NextTowerPrefab != null)
						TowerContainer.BuyTower(upChoice.NextTowerPrefab);
					break;
				case SellChoice:
					TowerContainer.SellContainedTower();
					break;
				default:
					Debug.LogError("Unknown choice type!");
					break;
			}
		}

		public void SetNextUpgrade([CanBeNull] GameObject nextTowerPrefab)
		{
			upgradeChoice.SetUpgrade(nextTowerPrefab);
		}
	}

	[Serializable]
	internal class UpgradeChoice : IChoice
	{
		[SerializeField] private string buttonLabelUpgrade = "Upgrade";
		[SerializeField] private string buttonLabelMax = "Max";
		[SerializeField] private Sprite maxSprite;

		[NonSerialized] [CanBeNull] public GameObject NextTowerPrefab;

		[NonSerialized] private string _buttonLabel;
		[NonSerialized] private int _price;
		[NonSerialized] private Sprite _sprite;

		public string Label => _buttonLabel;
		public int Price => _price;
		public Sprite Sprite => _sprite;

		public void SetUpgrade([CanBeNull] GameObject nextTowerPrefab)
		{
			if (nextTowerPrefab == null)
			{
				_buttonLabel = buttonLabelMax;
				NextTowerPrefab = null;
				_price = 0;
				_sprite = maxSprite;
			}
			else
			{
				_buttonLabel = buttonLabelUpgrade;
				NextTowerPrefab = nextTowerPrefab;
				_price = nextTowerPrefab.GetComponent<Tower>().price;
				_sprite = nextTowerPrefab.GetComponent<SpriteRenderer>().sprite;
			}
		}
	}

	[Serializable]
	internal class SellChoice : IChoice
	{
		[SerializeField] private string buttonLabel = "Sell";
		[SerializeField] private Sprite sprite;

		public string Label => buttonLabel;
		public int Price => 0;
		public Sprite Sprite => sprite;
	}
}
