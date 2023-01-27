using UnityEngine;
using UnityEngine.UI;

using static DebugPrinter;

public class Enemy : MonoBehaviour
{
	//==Route==
	[SerializeField] private float speed = 5f;
	private static RouteNode _beginNode;
	private static RouteNode _endNode;

	private RouteNode _currentNode;
	private RouteNode _nextNode;

	//==Health Bar==
	[SerializeField] private float startHealth = 100f;
	private Slider _healthBar;
	private float _health;

	//==Body==
	[Tooltip("Lower is slower")] [Range(0f, 0.2f)] [SerializeField] private float bodyCornerTurnSpeed = 0.1f;
	private Transform _body;

	//==Turret==
	[Tooltip("Lower is slower")] [Range(0f, 0.2f)] [SerializeField] private float turretCornerTurnSpeed = 0.1f;
	[SerializeField] private Vector2 turretRotationSpeedMinMax = new(1f,2f);
	[Range(0f, 180f)] [SerializeField] private float turretRotationAmount = 45f;

	private Transform _turret;
	private float _turretRotationSpeed;
	private float _turretOffset;

	private void Start()
	{
		//==Route==
		//vary speed just a tiny tad, to prevent enemies from clumping up
		speed += Random.Range(-0.1f, 0.1f);

		if (_beginNode == null)
		{
			_beginNode = GameObject.Find("Start").GetComponent<RouteNode>();
			if (_beginNode == null) Debug.LogError("Begin node not found");
		}
		if (_endNode == null)
		{
			_endNode = GameObject.Find("End").GetComponent<RouteNode>();
			if (_endNode == null) Debug.LogError("End node not found");
		}

		transform.position = _beginNode.transform.position;
		_currentNode = _beginNode;
		_nextNode = ChooseNextNode();

		//==Health Bar==
		_healthBar = GetComponentInChildren<Slider>();
		if(_healthBar == null) Debug.LogError("Health bar not found");
		_healthBar.maxValue = startHealth;
		_health = startHealth;
		_healthBar.value = _health;

		//==Body==
		_body = transform.Find("Body");

		//==Turret==
		_turret = transform.Find("Turret");
		if(_turret == null) Debug.LogError("Turret not found");
		_turretRotationSpeed = Random.Range(turretRotationSpeedMinMax.x, turretRotationSpeedMinMax.y);
		_turretOffset = Random.Range(0f, 1000f);
	}

	private void Update()
	{
		//==Route==
		Vector3 myPosition = transform.position;
		Vector3 nextNodePosition = _nextNode.transform.position;
		myPosition = Vector3.MoveTowards(myPosition, nextNodePosition, speed * Time.deltaTime);
		transform.position = myPosition;
		if (transform.position == _nextNode.transform.position)
		{
			_currentNode = _nextNode;
			if(_currentNode == _endNode)
			{
				DPrint("Enemy reached end node");
				Destroy(gameObject);
				return;
			}
			_nextNode = ChooseNextNode();
		}

		//==Health Bar==
		_healthBar.value = _health;

		//==Body==
		Vector3 dir = nextNodePosition - myPosition;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		_body.rotation = Quaternion.Lerp(_body.rotation,
			Quaternion.Euler(0, 0, angle), //target rotation
			bodyCornerTurnSpeed);

		//==Turret==
		_turret.rotation = Quaternion.Lerp(_turret.rotation,
			Quaternion.Euler(0, 0, angle + 90f + Mathf.Sin(_turretRotationSpeed * (Time.time + _turretOffset)) * turretRotationAmount), //target rotation
			turretCornerTurnSpeed);
	}

	private RouteNode ChooseNextNode()
	{
		// if(_currentNode == null) DPrint("Current node is null");
		// if(_currentNode.nextNodes == null) DPrint("Current node has no next nodes");
		// if(_currentNode.nextNodes.Count == 0) DPrint("No next nodes for node " + _currentNode.name);

		//choose next node as a random neighbour to start off with...
		RouteNode nextNode = _currentNode.nextNodes[Random.Range(0, _currentNode.nextNodes.Count)];

		//...then choose actual next node with least amountOfTimesVisited
		foreach (RouteNode node in _currentNode.nextNodes)
			if (node.amountOfTimesVisited < nextNode.amountOfTimesVisited)
				nextNode = node;

		nextNode.amountOfTimesVisited++;
		return nextNode;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		Bullet bullet = col.gameObject.GetComponent<Bullet>();
		if (bullet == null) return;
		Damage(bullet.damage);
		Destroy(bullet.gameObject);
	}

	private void Damage(float amount)
	{
		_health -= amount;
		if (_health > 0f) return;
		DPrint("Enemy died");
		Destroy(gameObject);
	}
}
