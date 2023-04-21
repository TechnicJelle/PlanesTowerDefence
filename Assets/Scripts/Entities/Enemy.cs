using System;
using System.Collections;
using JetBrains.Annotations;
using Managers;
using Route;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Entities
{
	public class Enemy : MonoBehaviour
	{
		[Header("Route")]
		[SerializeField] private float speed = 5f;
		private static Node _beginNode;
		private static Node _endNode;

		private Node _currentNode;
		private Node _nextNode;

		[Header("Health Bar")]
		[SerializeField] private float startHealth = 100f;
		private Slider _healthBar;
		private float _health;

		[Header("Body")]
		[Tooltip("Lower is slower")] [Range(0f, 0.2f)] [SerializeField] private float bodyCornerTurnSpeed = 0.1f;
		private Transform _body;

		[Header("Turret")]
		[Tooltip("Lower is slower")] [Range(0f, 0.2f)] [SerializeField] private float turretCornerTurnSpeed = 0.1f;
		[SerializeField] private Vector2 turretRotationSpeedMinMax = new(1f, 2f);
		[Range(0f, 180f)] [SerializeField] private float turretRotationAmount = 45f;

		private Transform _turret;
		private float _turretRotationSpeed;
		private float _turretOffset;

		[Header("Money")]
		[SerializeField] private int moneyValue = 10;

		//==Debuff==
		private bool _isDebuffed;

		private void Start()
		{
			//==Route==
			//vary speed just a tiny tad, to prevent enemies from clumping up
			speed += Random.Range(-0.1f, 0.1f);

			if (_beginNode == null)
			{
				_beginNode = GameObject.Find("Start").GetComponent<Node>();
				if (_beginNode == null) Debug.LogError("Begin node not found");
			}

			if (_endNode == null)
			{
				_endNode = GameObject.Find("End").GetComponent<Node>();
				if (_endNode == null) Debug.LogError("End node not found");
			}

			transform.position = _beginNode.transform.position;
			_currentNode = _beginNode;
			_nextNode = ChooseNextNode();

			//==Health Bar==
			_healthBar = GetComponentInChildren<Slider>();
			if (_healthBar == null) Debug.LogError("Health bar not found");
			_healthBar.maxValue = startHealth;
			_health = startHealth;
			_healthBar.value = _health;

			//==Body==
			_body = transform.Find("Body");

			//==Turret==
			_turret = transform.Find("Turret");
			if (_turret == null) Debug.LogError("Turret not found");
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
				if (_currentNode == _endNode)
				{
					PlayerStatsManager.Instance.TakeDamage(1);
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

		private Node ChooseNextNode()
		{
			// if(_currentNode == null) DPrint("Current node is null");
			// if(_currentNode.nextNodes == null) DPrint("Current node has no next nodes");
			// if(_currentNode.nextNodes.Count == 0) DPrint("No next nodes for node " + _currentNode.name);

			//choose next node as a random neighbour to start off with...
			Node nextNode = _currentNode.nextNodes[Random.Range(0, _currentNode.nextNodes.Count)];

			//...then choose actual next node with least amountOfTimesVisited
			foreach (Node node in _currentNode.nextNodes)
				if (node.amountOfTimesVisited < nextNode.amountOfTimesVisited)
					nextNode = node;

			nextNode.amountOfTimesVisited++;
			return nextNode;
		}

		public void Damage(float amount)
		{
			if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
			_health -= amount;
			if (_health > 0f) return;
			Die();
		}

		public void Debuff(float factor, float duration, [CanBeNull] GameObject visual = null)
		{
			if (_isDebuffed) StopCoroutine(nameof(DebuffCoroutine)); //stop previous debuff, to reapply it for the full duration anew
			StartCoroutine(DebuffCoroutine(factor, duration, visual));
		}

		private IEnumerator DebuffCoroutine(float factor, float duration, [CanBeNull] GameObject visual = null)
		{
			_isDebuffed = true;
			GameObject visualInstance = visual != null ? Instantiate(visual, transform) : null;
			speed *= factor;

			yield return new WaitForSeconds(duration);

			speed /= factor;
			if (visualInstance != null) Destroy(visualInstance);
			_isDebuffed = false;
		}

		private void Die()
		{
			PlayerStatsManager.Instance.AddMoney(moneyValue);
			Destroy(gameObject);
		}
	}
}
