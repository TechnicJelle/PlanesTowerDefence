using System.Collections;
using ScriptableObjects;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	[SerializeField] private Wave[] waves;

    private void Start()
    {
	   	StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
	    foreach (Wave wave in waves)
	    {
		    for (int i = 0; i < wave.delaySincePreviousWave; i++)
		    {
			    Debug.Log(wave.delaySincePreviousWave - i);
				yield return new WaitForSeconds(1);
		    }
		    wave.StartWave();
	    }
    }
}
