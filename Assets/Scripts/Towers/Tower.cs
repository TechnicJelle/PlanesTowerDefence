using Entities;
using JetBrains.Annotations;
using UnityEngine;

namespace Towers
{
	public class Tower : MonoBehaviour
	{
		[SerializeField] public int price = 100;
		[Range(1, 10)] [SerializeField] private float range = 3f;
		[SerializeField] private float fireRate = 1f;
		[SerializeField] private GameObject bulletPrefab;

		private Enemy _target;

		private void Start()
		{
			if(bulletPrefab == null) Debug.LogError("No bullet prefab set!");

			InvokeRepeating(nameof(Fire), 0f, 1f/fireRate);
		}

		private void Fire()
		{
			if(_target == null) return;
			Transform myTransform = transform;
			Instantiate(bulletPrefab, myTransform.position, myTransform.rotation);
		}

		private void Update()
		{
			Transform myTransform = transform;

			// Find closest enemy
			_target = null;
			Enemy target = GetClosestEnemy();
			if(target == null) return;
			Vector3 targetPosition = target.transform.position;
			if(Vector3.Distance(myTransform.position, targetPosition) > range) return;
			_target = target;

			// Rotate towards enemy
			myTransform.up = targetPosition - myTransform.position;
		}

		[CanBeNull]
		private Enemy GetClosestEnemy()
		{
			//TODO: Refactor this
			Enemy[] enemies = FindObjectsOfType<Enemy>();
			Enemy closestEnemy = null;
			float closestDistance = Mathf.Infinity;
			foreach (Enemy enemy in enemies)
			{
				float distance = Vector3.Distance(transform.position, enemy.transform.position);
				if (distance > closestDistance) continue;
				closestDistance = distance;
				closestEnemy = enemy;
			}

			return closestEnemy;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(transform.position, range);
		}
	}
}
