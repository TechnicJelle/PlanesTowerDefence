using Entities;
using UnityEngine;

namespace BulletEffects
{
	[RequireComponent(typeof(Bullet))]
	public class Debuff : MonoBehaviour
	{
		[Range(0.0f, 1.0f)] [SerializeField] private float speedFactor = 0.5f;
		[SerializeField] private float duration = 2f;
		[SerializeField] private GameObject visualEffectPrefab;

		private Bullet _bullet;

		private void Awake()
		{
			_bullet = GetComponent<Bullet>();
			_bullet.OnEnemyHit += OnEnemyHit;
		}

		private void OnEnemyHit(Enemy enemy)
		{
			enemy.Debuff(speedFactor, duration, visualEffectPrefab);
		}
	}
}
