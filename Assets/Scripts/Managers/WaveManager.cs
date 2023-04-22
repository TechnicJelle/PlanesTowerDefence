using System;
using System.Collections;
using Entities;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Managers
{
	public class WaveManager : MonoBehaviour
	{
		public static WaveManager Instance;

		public Action OnWaveStart;
		public Action OnWaveEnd;
		public bool IsWaveRunning { get; private set; }

		[SerializeField] private string strPrepareToBeAttackedByWave = "Prepare to be attacked by wave {0}!";
		[SerializeField] private string strTimeTillNextWave = "Time till next wave: {0}";
		[SerializeField] private string strCurrentWave = "Current wave: {0}";

		[SerializeField] private Wave[] waves;

		[SerializeField] private TextMeshProUGUI waveText;
		[SerializeField] private TextMeshProUGUI waveCountdownText;

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else
				Debug.LogError("Multiple WaveManagers in scene!");
		}

		private void Start()
		{
			GameStateManager.Instance.OnGameStart += StartWaves;
			GameStateManager.Instance.OnGameEnd += StopAllCoroutines;
			GameStateManager.Instance.OnResetGame += ResetWaves;

			OnWaveStart += () => IsWaveRunning = true;
			OnWaveEnd += () => IsWaveRunning = false;
		}

		private void StartWaves()
		{
			StartCoroutine(SpawnWaves());
		}

		private IEnumerator SpawnWaves()
		{
			for (int waveNr = 0; waveNr < waves.Length; waveNr++)
			{
				int humanWaveNr = waveNr + 1;
				waveText.text = string.Format(strPrepareToBeAttackedByWave, humanWaveNr);

				//Wait until wave starts
				Wave wave = waves[waveNr];
				for (int i = 0; i < wave.delaySincePreviousWave + 1; i++)
				{
					waveCountdownText.text = string.Format(strTimeTillNextWave, wave.delaySincePreviousWave - i);
					yield return new WaitForSeconds(1);
				}

				//Spawn wave
				wave.StartWave();
				OnWaveStart?.Invoke();
				waveCountdownText.text = "";
				waveText.text = string.Format(strCurrentWave, humanWaveNr);

				yield return WaitForAllEnemiesDefeated();

				OnWaveEnd?.Invoke();
			}

			//All waves have spawned, so clear the text
			waveCountdownText.text = "";

			yield return WaitForAllEnemiesDefeated();

			GameStateManager.Instance.GameWin();
		}

		/// <summary>
		/// Wait until all enemies are defeated
		/// </summary>
		private static IEnumerator WaitForAllEnemiesDefeated()
		{
			while (GetEnemies().Length > 0)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private void ResetWaves()
		{
			OnWaveEnd?.Invoke();
			foreach (Enemy enemy in GetEnemies())
			{
				Destroy(enemy.gameObject);
			}
		}

		public static Enemy[] GetEnemies()
		{
			//TODO: Improve this, probably by keeping track of enemies in a list, manually
			return FindObjectsOfType<Enemy>();
		}
	}
}
