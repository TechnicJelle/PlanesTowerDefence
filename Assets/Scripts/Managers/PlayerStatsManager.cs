using System;
using TMPro;
using UnityEngine;

namespace Managers
{
	public class PlayerStatsManager : MonoBehaviour
	{
		public static PlayerStatsManager Instance;

		[SerializeField] private int startHealth = 20;
		[SerializeField] private int startMoney = 200;

		[SerializeField] private TextMeshProUGUI statsText;

		private string _statsText;

		private int _currentHealth;

		private int _currentMoney;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Debug.LogError("Multiple PlayerStatsManagers in scene!");
			}

			GameStateManager.Instance.OnGameStart += StartGame;

			_statsText = statsText.text;
		}

		private void StartGame()
		{
			_currentHealth = startHealth;
			_currentMoney = startMoney;
			UpdateText();
		}

		private void UpdateText() => statsText.text = string.Format(_statsText, _currentHealth, _currentMoney);

		public void TakeDamage(int damage)
		{
			_currentHealth -= damage;
			UpdateText();
			if (_currentHealth <= 0)
			{
				GameStateManager.Instance.GameOver();
			}
		}

		public void AddMoney(int amount)
		{
			if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
			_currentMoney += amount;
			UpdateText();
		}

		public void Buy(int price)
		{
			if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
			_currentMoney -= price;
			UpdateText();
		}

		public bool HaveEnoughMoneyFor(int price)
		{
			if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
			return _currentMoney >= price;
		}
	}
}
