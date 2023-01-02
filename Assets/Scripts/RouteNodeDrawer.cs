using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RouteNodeDrawer : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private float nodeSize = 0.1f;
	[SerializeField] private int fontSize = 20;

	private IEnumerable<Transform> _points;

	private IEnumerable<Transform> GetRoutePoints() => GetComponentsInChildren<Transform>().Where(t => t != transform);

	private void Awake()
	{
		_points = GetRoutePoints();
	}

	private void Update()
	{
		if (EditorApplication.isPlaying) return;
		_points = GetRoutePoints();
	}

	private void OnDrawGizmos()
	{
		if (_points == null) return;
		foreach (Transform point in _points)
		{
			if(point == null) continue;
			Gizmos.color = Color.red;
			Vector3 position = point.position;
			Gizmos.DrawSphere(position, nodeSize);
			Handles.Label(position, point.name, new GUIStyle{fontSize = fontSize});
		}
	}
#endif
}
