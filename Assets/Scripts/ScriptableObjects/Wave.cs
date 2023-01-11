using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
	public class Wave : ScriptableObject
	{
		[FormerlySerializedAs("spawnSeconds")] public int delaySincePreviousWave;
		[SerializeField] private int enemyCount;
		[SerializeField] private GameObject enemyPrefab;

		public void StartWave()
		{
			for (int i = 0; i < enemyCount; i++)
			{
				Instantiate(enemyPrefab);
			}
		}
	}
}
