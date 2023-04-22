using Entities;
using UnityEngine;

namespace BulletEffects
{
	public class Debuff : ABulletEffect
	{
		[Range(0.0f, 1.0f)] [SerializeField] private float speedFactor = 0.5f;
		[SerializeField] private float duration = 2f;
		[SerializeField] private GameObject visualEffectPrefab;

		protected override void OnEnemyHit(Enemy enemy)
		{
			enemy.Debuff(speedFactor, duration, visualEffectPrefab);
		}
	}
}
