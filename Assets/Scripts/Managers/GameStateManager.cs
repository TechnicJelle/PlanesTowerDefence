using System;
using UI;
using UnityEngine;

namespace Managers
{
	internal enum GameState
	{
		Pregame,
		Running,
		Over,
	}

	public class GameStateManager : MonoBehaviour
	{
		public static GameStateManager Instance;

		public Action OnGameStart;
		public Action OnGameOver;
		public Action OnResetGame;

		[SerializeField] private View inGameView;
		[SerializeField] private View gameOverView;

		private GameState _state;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Debug.LogError("Multiple GameStateManagers in scene!");
			}

			_state = GameState.Pregame;

			if (inGameView == null) Debug.LogError("In Game View is null!");
			if (gameOverView == null) Debug.LogError("Game Over View is null!");
		}

		public bool IsPregame => _state == GameState.Pregame;
		public bool IsRunning => _state == GameState.Running;
		public bool IsOver => _state == GameState.Over;

		public void StartGame()
		{
			if (_state != GameState.Pregame) return;
			Debug.Log("Starting game!");
			_state = GameState.Running;
			OnGameStart?.Invoke();
		}

		public void GameOver()
		{
			if (_state != GameState.Running) return;
			Debug.Log("Game Over!");
			_state = GameState.Over;
			OnGameOver?.Invoke();
			inGameView.Hide();
			gameOverView.Show();
		}

		public void RestartGame()
		{
			if (_state != GameState.Over) return;
			Debug.Log("Restarting game!");
			_state = GameState.Pregame;
			OnResetGame?.Invoke();
		}
	}
}
