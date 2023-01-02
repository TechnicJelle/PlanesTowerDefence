using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed = 5f;

	private static RouteNode _beginNode;
	private static RouteNode _endNode;

	private RouteNode _currentNode;
	private RouteNode _nextNode;

	private RouteNode ChooseNextNode()
	{
		if(_currentNode == null) Debug.LogError("Current node is null");
		if(_currentNode.nextNodes == null) Debug.LogError("Current node has no next nodes");
		if(_currentNode.nextNodes.Count == 0) Debug.LogError("No next nodes for node " + _currentNode.name);

		//choose next node with least amountOfTimesVisited
		RouteNode nextNode = _currentNode.nextNodes[Random.Range(0, _currentNode.nextNodes.Count)];

		foreach (RouteNode node in _currentNode.nextNodes)
			if (node.amountOfTimesVisited < nextNode.amountOfTimesVisited)
				nextNode = node;

		nextNode.amountOfTimesVisited++;
		return nextNode;
	}

	private void Start()
	{
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
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, _nextNode.transform.position, speed * Time.deltaTime);
		if (transform.position == _nextNode.transform.position)
		{
			_currentNode = _nextNode;
			_nextNode = ChooseNextNode();
		}
	}
}
