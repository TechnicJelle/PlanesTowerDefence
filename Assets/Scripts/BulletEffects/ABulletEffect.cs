using Entities;
using UnityEngine;

namespace BulletEffects
{
	[RequireComponent(typeof(Bullet))]
	public abstract class ABulletEffect : MonoBehaviour
	{
		protected Bullet Bullet { get; private set; }

		private void Awake()
		{
			Bullet = GetComponent<Bullet>();
			Bullet.OnEnemyHit += OnEnemyHit;
		}

		protected abstract void OnEnemyHit(Enemy enemy);
	}
}
