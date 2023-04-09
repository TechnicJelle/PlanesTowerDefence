using TMPro;
using UnityEngine;

namespace Managers
{
	public class PlayerStatsManager : MonoBehaviour
	{
		public static PlayerStatsManager Instance;

		[SerializeField] private int startHealth = 20;
		[SerializeField] private TextMeshProUGUI healthText;

		private int _currentHealth;

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
		}

		private void StartGame()
		{
			_currentHealth = startHealth;
			TakeDamage(0); //Update the text
		}

		public void TakeDamage(int damage)
		{
			_currentHealth -= damage;
			healthText.text = $"Health: {_currentHealth}";
			if (_currentHealth <= 0)
			{
				GameStateManager.Instance.GameOver();
			}
		}
	}
}
