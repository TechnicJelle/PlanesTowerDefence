using JetBrains.Annotations;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[Range(1, 6)] [SerializeField] private float range = 15f;
	[SerializeField] private float fireRate = 1f;
	[SerializeField] private GameObject bulletPrefab;

	private void Start()
	{
		if(bulletPrefab == null) Debug.LogError("No bullet prefab set!");
	}

	private void Update()
	{
		Transform myTransform = transform;

		// Find closest enemy
		Enemy target = GetClosestEnemy();
		if(target == null) return;
		Vector3 targetPosition = target.transform.position;
		if(Vector3.Distance(myTransform.position, targetPosition) > range) return;

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
