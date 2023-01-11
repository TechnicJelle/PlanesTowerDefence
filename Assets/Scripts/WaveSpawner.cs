using System.Collections;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	[SerializeField] private Wave[] waves;

	[SerializeField] private TextMeshProUGUI waveText;
	[SerializeField] private TextMeshProUGUI waveCountdownText;

	private void Start()
	{
		StartCoroutine(SpawnWaves());
	}

	private IEnumerator SpawnWaves()
	{
		for (int waveNr = 0; waveNr < waves.Length; waveNr++)
		{
			Wave wave = waves[waveNr];
			waveText.text = "Current wave: " + waveNr;
			for (int i = 0; i < wave.delaySincePreviousWave + 1; i++)
			{
				waveCountdownText.text = $"Time till next wave: {wave.delaySincePreviousWave - i}";
				yield return new WaitForSeconds(1);
			}

			wave.StartWave();
		}

		//All waves have spawned, so clear the text
		waveText.text = "No waves left";
		waveCountdownText.text = "";
	}
}
