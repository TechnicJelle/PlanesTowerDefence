using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Mathf;

public class UIRadialSelector : MonoBehaviour
{
	private static GameObject _canvasPrefab;
	private static GameObject _buttonPrefab;

	[SerializeField] private Choice[] choices;

	public TowerContainer TowerContainer { get; set; }

	private Canvas _canvas;

	private void Start()
	{
		if (_canvasPrefab == null)
		{
			_canvasPrefab = Resources.Load<GameObject>("PopupCanvas");
			if (_canvasPrefab == null) DebugPrinter.DPrint("Canvas prefab not found");
		}

		_canvas = Instantiate(_canvasPrefab, TowerContainer.transform).GetComponent<Canvas>();
		_canvas.worldCamera = Camera.main;

		if (_buttonPrefab == null)
		{
			_buttonPrefab = Resources.Load<GameObject>("Choice Button");
			if (_buttonPrefab == null) DebugPrinter.DPrint("Choice Button prefab not found");
		}

		// Create buttons
		for (int i = 0; i < choices.Length; i++)
		{
			Choice choice = choices[i];
			GameObject instance = Instantiate(_buttonPrefab, _canvas.transform);
			instance.name = "btn_" + choice.label;

			instance.transform.localScale = new Vector3(1f, 1f, 1f);

			// Set button position
			float angle = 2f * PI / choices.Length * i - PI - PI / choices.Length;
			instance.transform.localPosition = new Vector3(Cos(angle), Sin(angle), 0);

			// Set button texts
			TextMeshProUGUI[] texts = instance.GetComponentsInChildren<TextMeshProUGUI>();

			foreach (TextMeshProUGUI text in texts)
			{
				text.text = text.name switch
				{
					{ } a when a.IndexOf("Name", StringComparison.OrdinalIgnoreCase) >= 0 => choice.label,
					{ } a when a.IndexOf("Price", StringComparison.OrdinalIgnoreCase) >= 0 => choice.gameObject.GetComponent<Tower>().price.ToString(),
					_ => text.text,
				};
			}

			// Set button image
			Image image = instance.GetComponent<Image>();
			image.sprite = choice.gameObject.GetComponent<SpriteRenderer>().sprite;

			// Set button click event
			Button button = instance.GetComponentInChildren<Button>();
			button.onClick.AddListener(() => OnChoiceClicked(choice));
		}

		Hide();
	}

	private void OnChoiceClicked(Choice choice)
	{
		TowerContainer.tower = Instantiate(choice.gameObject, TowerContainer.transform);
		Hide();
	}

	public void Show()
	{
		_canvas.enabled = true;
	}

	public void Hide()
	{
		_canvas.enabled = false;
	}
}

[Serializable]
internal class Choice
{
	public GameObject gameObject;
	public string label;
}
