using System.Collections;
using Entities;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Managers
{
	public class WaveManager : MonoBehaviour
	{
		[SerializeField] private Wave[] waves;

		[SerializeField] private TextMeshProUGUI waveText;
		[SerializeField] private TextMeshProUGUI waveCountdownText;

		private const string WAVE_TEXT = "Prepare to be attacked!";

		private void Start()
		{
			GameStateManager.Instance.OnGameStart += StartWaves;
			GameStateManager.Instance.OnGameOver += StopAllCoroutines;
			GameStateManager.Instance.OnResetGame += ResetWaves;
		}

		private void StartWaves()
		{
			StartCoroutine(SpawnWaves());
		}

		private IEnumerator SpawnWaves()
		{
			waveText.text = WAVE_TEXT;
			for (int waveNr = 0; waveNr < waves.Length; waveNr++)
			{
				Wave wave = waves[waveNr];
				if(waveNr > 0) waveText.text = "Current wave: " + waveNr;
				for (int i = 0; i < wave.delaySincePreviousWave + 1; i++)
				{
					waveCountdownText.text = $"Time till next wave: {wave.delaySincePreviousWave - i}";
					yield return new WaitForSeconds(1);
				}

				wave.StartWave();
			}

			//All waves have spawned, so clear the text
			// waveText.text = "No waves left";
			waveCountdownText.text = "";
		}

		private void ResetWaves()
		{
			Debug.Log("Resetting game");
			foreach (Enemy enemy in FindObjectsOfType<Enemy>())
			{
				Destroy(enemy.gameObject);
			}
		}
	}
}
