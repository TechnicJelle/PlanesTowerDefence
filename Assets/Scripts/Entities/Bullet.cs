using System;
using UnityEngine;

namespace Entities
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] public int damage = 10;
		[SerializeField] private float speed = 10f;

		/// <summary>
		/// Subscribe to this event to add special effects to the bullet
		/// </summary>
		public Action<Enemy> OnEnemyHit;

		private bool _hasAlreadyHitEnemy;

		private void Update()
		{
			Transform myTransform = transform;
			myTransform.position += Time.deltaTime * speed * myTransform.up;

			// If off-screen
			if (Mathf.Abs(myTransform.position.x) > 15f || Mathf.Abs(myTransform.position.y) > 8f)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if(_hasAlreadyHitEnemy) return;
			Enemy enemy = col.gameObject.GetComponent<Enemy>();
			if (enemy == null) return;
			_hasAlreadyHitEnemy = true;
			OnEnemyHit?.Invoke(enemy);
			enemy.Damage(damage);
			Destroy(gameObject);
		}
	}
}
