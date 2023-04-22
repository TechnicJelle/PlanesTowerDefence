using System;
using JetBrains.Annotations;
using Towers;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(TowerContainer))]
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
			if (nextTowerPrefab == null)
			{
				upgradeChoice.SetMax();
				upgradeChoice.NextTowerPrefab = null;
				upgradeChoice.price = 0;
			}
			else
			{
				upgradeChoice.SetUpgrade();
				upgradeChoice.NextTowerPrefab = nextTowerPrefab;
				upgradeChoice.price = nextTowerPrefab.GetComponent<Tower>().price;
				upgradeChoice.sprite = nextTowerPrefab.GetComponent<SpriteRenderer>().sprite;
			}
		}
	}

	[Serializable]
	internal class UpgradeChoice : IChoice
	{
		[SerializeField] private string buttonLabelUpgrade = "Upgrade";
		[SerializeField] private string buttonLabelMax = "Max";
		[SerializeField] private Sprite maxSprite;


		[NonSerialized] private string _buttonLabel;
		[NonSerialized] [CanBeNull] public GameObject NextTowerPrefab;
		[NonSerialized] public int price;
		[NonSerialized] public Sprite sprite;

		public string Label => _buttonLabel;
		public int Price => price;
		public Sprite Sprite => sprite;

		public UpgradeChoice() => SetUpgrade();

		public void SetUpgrade() => _buttonLabel = buttonLabelUpgrade;

		public void SetMax()
		{
			_buttonLabel = buttonLabelMax;
			sprite = maxSprite;
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
