using UnityEngine;
using UnityEngine.UI;

using static DebugPrinter;

public class Enemy : MonoBehaviour
{
	public float speed = 5f;

	public float startHealth = 100f;

	private static RouteNode _beginNode;
	private static RouteNode _endNode;

	private RouteNode _currentNode;
	private RouteNode _nextNode;

	private Slider _healthBar;
	private float _health;

	private RouteNode ChooseNextNode()
	{
		if(_currentNode == null) DPrint("Current node is null");
		if(_currentNode.nextNodes == null) DPrint("Current node has no next nodes");
		if(_currentNode.nextNodes.Count == 0) DPrint("No next nodes for node " + _currentNode.name);

		//choose next node as a random neighbour to start off with...
		RouteNode nextNode = _currentNode.nextNodes[Random.Range(0, _currentNode.nextNodes.Count)];

		//...then choose actual next node with least amountOfTimesVisited
		foreach (RouteNode node in _currentNode.nextNodes)
			if (node.amountOfTimesVisited < nextNode.amountOfTimesVisited)
				nextNode = node;

		nextNode.amountOfTimesVisited++;
		return nextNode;
	}

	private void Start()
	{
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

		_healthBar = GetComponentInChildren<Slider>();
		if(_healthBar == null) Debug.LogError("Health bar not found");
		_healthBar.maxValue = startHealth;
		_health = startHealth;
		_healthBar.value = _health;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, _nextNode.transform.position, speed * Time.deltaTime);
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

		_healthBar.value = _health;
	}

	private void Damage(float amount)
	{
		_health -= amount;
		if (_health > 0f) return;
		DPrint("Enemy died");
		Destroy(gameObject);
	}
}
