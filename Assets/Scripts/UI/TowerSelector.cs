using System;
using Towers;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(TowerContainer))]
	public class TowerSelector : RadialSelector
	{
		[SerializeField] private TowerChoice[] towerChoices;

		private TowerContainer _towerContainer;

		private void Start()
		{
			_towerContainer = GetComponent<TowerContainer>();
			Init(towerChoices);
		}

		protected override void OnChoiceClicked(IChoice choice)
		{
			base.OnChoiceClicked(choice);
			if (choice is TowerChoice towerChoice)
				_towerContainer.SetTower(towerChoice.prefab);
		}
	}

	[Serializable]
	internal class TowerChoice : IChoice
	{
		public GameObject prefab;
		public string buttonLabel;

		public Tower Tower => prefab.GetComponent<Tower>();

		public string Label => buttonLabel;
		public int Price => Tower.price;
		public Sprite Sprite => prefab.GetComponent<SpriteRenderer>().sprite;
	}
}
