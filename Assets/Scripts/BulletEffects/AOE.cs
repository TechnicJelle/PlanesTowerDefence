using Entities;
using Managers;
using UnityEngine;

namespace BulletEffects
{
	public class AOE : ABulletEffect
	{
		[SerializeField] private float range = 2;

		protected override void OnEnemyHit(Enemy enemy)
		{
			foreach (Enemy e in WaveManager.GetEnemies())
			{
				if (e == enemy) continue; //don't damage the enemy that was hit by the bullet (the one that called this method)

				//find all enemies within the range
				if (Vector3.Distance(enemy.transform.position, e.transform.position) < range)
				{
					e.Damage(Bullet.damage);
				}
			}
		}
	}
}
