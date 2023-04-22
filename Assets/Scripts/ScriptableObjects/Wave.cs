using System;
using Entities;
using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
	public class Wave : ScriptableObject
	{
		[Tooltip("Time for building phase")] [SerializeField] public int delaySincePreviousWave;
		[SerializeField] private WavePart[] waveParts;

		private void OnValidate()
		{
			foreach (WavePart wavePart in waveParts)
			{
				if (wavePart.enemyPrefab == null) Debug.LogError("Enemy prefab is null!");
				if (wavePart.enemyPrefab.GetComponent<Enemy>() == null) Debug.LogError($"Enemy prefab \"{wavePart.enemyPrefab.name}\" does not have an Enemy component!");
			}
		}

		public void StartWave()
		{
			foreach (WavePart wavePart in waveParts)
			{
				for (int i = 0; i < wavePart.enemyCount; i++)
				{
					Instantiate(wavePart.enemyPrefab);
				}
			}
		}
	}

	[Serializable]
	public class WavePart
	{
		[SerializeField] public GameObject enemyPrefab;
		[SerializeField] public int enemyCount;
	}
}
