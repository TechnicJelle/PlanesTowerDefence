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

		[SerializeField] private Wave[] waves;

		[SerializeField] private TextMeshProUGUI waveText;
		[SerializeField] private TextMeshProUGUI waveCountdownText;

		private const string STR_PREPARE_TO_BE_ATTACKED_BY_WAVE = "Prepare to be attacked by wave {0}!";

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
			GameStateManager.Instance.OnGameOver += StopAllCoroutines;
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
				waveText.text = string.Format(STR_PREPARE_TO_BE_ATTACKED_BY_WAVE, humanWaveNr);

				//Wait until wave starts
				Wave wave = waves[waveNr];
				for (int i = 0; i < wave.delaySincePreviousWave + 1; i++)
				{
					waveCountdownText.text = $"Time till next wave: {wave.delaySincePreviousWave - i}";
					yield return new WaitForSeconds(1);
				}

				//Spawn wave
				wave.StartWave();
				OnWaveStart?.Invoke();
				waveCountdownText.text = "";
				waveText.text = $"Current wave: {humanWaveNr}";

				//Wait until all enemies are defeated
				while (FindObjectsOfType<Enemy>().Length > 0)
				{
					yield return new WaitForSeconds(0.5f);
				}

				OnWaveEnd?.Invoke();
			}

			//All waves have spawned, so clear the text
			// waveText.text = "No waves left";
			waveCountdownText.text = "";
		}

		private void ResetWaves()
		{
			OnWaveEnd?.Invoke();
			foreach (Enemy enemy in FindObjectsOfType<Enemy>())
			{
				Destroy(enemy.gameObject);
			}
		}
	}
}
