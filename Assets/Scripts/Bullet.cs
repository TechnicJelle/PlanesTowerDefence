using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] public int damage = 10;
	[SerializeField] private float speed = 10f;

	private void Update()
	{
		Transform myTransform = transform;
		myTransform.position += Time.deltaTime * speed * myTransform.up;

		if (Mathf.Abs(myTransform.position.x) > 15f || Mathf.Abs(myTransform.position.y) > 8f)
		{
			Destroy(gameObject);
		}
	}
}
