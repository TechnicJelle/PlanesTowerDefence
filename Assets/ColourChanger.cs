using UnityEngine;

[ExecuteInEditMode]
public class ColourChanger : MonoBehaviour
{
	[SerializeField] private Color color = Color.white;

#if UNITY_EDITOR

	private void OnValidate()
	{
		foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.color = color;
		}
	}
#endif
}
