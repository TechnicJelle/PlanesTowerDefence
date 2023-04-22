using Entities;
using Managers;
using UnityEngine;

namespace BulletEffects
{
	[RequireComponent(typeof(Bullet))]
	public class AOE : MonoBehaviour
	{
		[SerializeField] private float range = 2;

		private Bullet _bullet;

		private void Awake()
		{
			_bullet = GetComponent<Bullet>();
			_bullet.OnEnemyHit += OnEnemyHit;
		}

		private void OnEnemyHit(Enemy enemy)
		{
			Enemy[] enemies = WaveManager.GetEnemies();

			//find all enemies within the range
			foreach (Enemy e in enemies)
			{
				if (e == enemy) continue; //don't damage the enemy that was hit by the bullet (the one that called this method)

				if (Vector3.Distance(enemy.transform.position, e.transform.position) < range)
				{
					e.Damage(_bullet.damage);
				}
			}
		}
	}
}
