using System.Collections.Generic;
using UnityEngine;

namespace Route
{
	public class Node : MonoBehaviour
	{
		public List<Node> nextNodes = new();
		private Color _ownColour;
		public int amountOfTimesVisited;

		private void Awake()
		{
			_ownColour = Color.HSVToRGB(Random.Range(0.3f, 0.9f), 1f, 1f);
		}

		private void Update()
		{
			foreach (Node nextNode in nextNodes)
			{
				DrawArrow(transform.position, nextNode.transform.position);
			}
		}

		private void DrawArrow(Vector3 startPos, Vector3 endPos)
		{
			const float angle = 20;
			const float headLength = 0.4f;

			Vector3 direction = endPos - startPos;
			direction.Normalize();
			Vector3 a = endPos - Quaternion.Euler(0, 0, angle) * direction * headLength;
			Vector3 b = endPos - Quaternion.Euler(0, 0, -angle) * direction * headLength;
			Debug.DrawLine(startPos, endPos, _ownColour);
			Debug.DrawLine(endPos, a, _ownColour);
			Debug.DrawLine(endPos, b, _ownColour);
		}
	}
}
