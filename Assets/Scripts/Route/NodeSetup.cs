using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Route
{
	public class NodeSetup : MonoBehaviour
	{
		private static bool IsAboutEqual(float a, float b) => Mathf.Abs(a - b) < 0.01f;

		private void Awake()
		{
			Node[] nodes = GetComponentsInChildren<Node>().ToArray();

			foreach (Node node in nodes)
			{
				if(node.nextNodes.Count != 0) continue; //don't overwrite manually set connections

				List<Node> rightNodes = new();
				List<Node> bottomNodes = new();

				foreach (Node otherNode in nodes)
				{
					if(node == otherNode) continue; //don't connect to self

					//if otherPoint is to the right of this point
					if (IsAboutEqual(node.transform.position.y, otherNode.transform.position.y) && otherNode.transform.position.x > node.transform.position.x)
					{
						rightNodes.Add(otherNode);
					}
					//if otherPoint is to the bottom of this point
					else if (IsAboutEqual(node.transform.position.x, otherNode.transform.position.x) && node.transform.position.y > otherNode.transform.position.y)
					{
						bottomNodes.Add(otherNode);
					}
				}

				//sort by x position
				rightNodes.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
				//sort by y position
				bottomNodes.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

				if(rightNodes.Count > 0) node.nextNodes.Add(rightNodes[0]);
				if(bottomNodes.Count > 0) node.nextNodes.Add(bottomNodes[0]);
			}
		}
	}
}
