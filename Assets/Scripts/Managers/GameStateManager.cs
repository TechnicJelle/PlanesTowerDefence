using System;
using TMPro;
using UI;
using UnityEngine;

namespace Managers
{
	internal enum GameState
	{
		Pregame,
		Running,
		End,
	}

	public class GameStateManager : MonoBehaviour
	{
		public static GameStateManager Instance;

		public Action OnGameStart;
		public Action OnGameEnd;
		public Action OnResetGame;

		[SerializeField] private View inGameView;
		[SerializeField] private View gameEndView;
		[SerializeField] private TMP_Text endGameText;
		[SerializeField] private string gameOverText = "Game Over!";
		[SerializeField] private string gameWinText = "You Win!";

		private GameState _state;

		private string _endGameText;

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
			_endGameText = endGameText.text;

			if (inGameView == null) Debug.LogError("In Game View is null!");
			if (gameEndView == null) Debug.LogError("Game End View is null!");
		}

		public bool IsPregame => _state == GameState.Pregame;
		public bool IsRunning => _state == GameState.Running;
		public bool IsEnded => _state == GameState.End;

		public void StartGame()
		{
			if (_state != GameState.Pregame) return; //can only start game from pregame state
			Debug.Log("Starting game!");
			_state = GameState.Running;
			OnGameStart?.Invoke();
		}

		public void GameOver()
		{
			if (_state != GameState.Running) return; //can only end game from running state
			Debug.Log("Game Over!");
			endGameText.text = string.Format(_endGameText, gameOverText);
			GameEnd();
		}

		public void GameWin()
		{
			if (_state != GameState.Running) return; //can only end game from running state
			Debug.Log("Game Win!");
			endGameText.text = string.Format(_endGameText, gameWinText);
			GameEnd();
		}

		private void GameEnd()
		{
			_state = GameState.End;
			OnGameEnd?.Invoke();
			inGameView.Hide();
			gameEndView.Show();
		}

		public void RestartGame()
		{
			if (_state != GameState.End) return; //can only restart game from end state
			Debug.Log("Restarting game!");
			_state = GameState.Pregame;
			OnResetGame?.Invoke();
		}
	}
}
