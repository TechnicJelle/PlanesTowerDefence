using System;
using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
	public class Wave : ScriptableObject
	{
		[SerializeField] public int delaySincePreviousWave;
		[SerializeField] private WavePart[] waveParts;

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
