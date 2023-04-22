using System;
using Towers;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(TowerContainer))]
	public class TowerSelector : RadialSelector
	{
		[SerializeField] private TowerChoice[] towerChoices;

		private void Start()
		{
			Init(towerChoices);
		}

		protected override void OnChoiceClicked(IChoice choice)
		{
			base.OnChoiceClicked(choice);
			if (choice is TowerChoice towerChoice)
				TowerContainer.BuyTower(towerChoice.prefab);
			else
				Debug.LogError("Unknown choice type!");
		}
	}

	[Serializable]
	internal class TowerChoice : IChoice
	{
		public GameObject prefab;
		[SerializeField] private string buttonLabel;

		public Tower Tower => prefab.GetComponent<Tower>();

		public string Label => buttonLabel;
		public int Price => Tower.price;
		public Sprite Sprite => prefab.GetComponent<SpriteRenderer>().sprite;
	}
}
