using System;
using Managers;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class RadialSelector : MonoBehaviour
	{
		private static GameObject _canvasPrefab;
		private static GameObject _buttonPrefab;

		[SerializeField] private Choice[] choices;

		public TowerContainer TowerContainer { get; set; }

		private Canvas _canvas;
		private Button[] _buttons;

		private void Awake()
		{
			if (_canvasPrefab == null)
			{
				_canvasPrefab = Resources.Load<GameObject>("PopupCanvas");
				if (_canvasPrefab == null) Debug.LogError("Canvas prefab not found");
			}

			_canvas = Instantiate(_canvasPrefab, TowerContainer.transform).GetComponent<Canvas>();
			_canvas.worldCamera = Camera.main;

			if (_buttonPrefab == null)
			{
				_buttonPrefab = Resources.Load<GameObject>("Choice Button");
				if (_buttonPrefab == null) Debug.LogError("Choice Button prefab not found");
			}

			// Create buttons
			_buttons = new Button[choices.Length];
			for (int i = 0; i < choices.Length; i++)
			{
				Choice choice = choices[i];
				GameObject buttonInstance = Instantiate(_buttonPrefab, _canvas.transform);
				buttonInstance.name = "btn_" + choice.label;

				int price = choice.Tower.price;

				buttonInstance.transform.localScale = new Vector3(1f, 1f, 1f);

				// Set button position
				float angle = 2f * Mathf.PI / choices.Length * i - Mathf.PI - Mathf.PI / choices.Length;
				buttonInstance.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

				// Set button texts
				TextMeshProUGUI[] texts = buttonInstance.GetComponentsInChildren<TextMeshProUGUI>();

				foreach (TextMeshProUGUI text in texts)
				{
					text.text = text.name switch
					{
						{ } a when a.IndexOf("Name", StringComparison.OrdinalIgnoreCase) >= 0 => choice.label,
						{ } a when a.IndexOf("Price", StringComparison.OrdinalIgnoreCase) >= 0 => price.ToString(),
						_ => text.text,
					};
				}

				// Set button image
				Image image = buttonInstance.GetComponent<Image>();
				image.sprite = choice.gameObject.GetComponent<SpriteRenderer>().sprite;

				// Set button click event
				Button button = buttonInstance.GetComponentInChildren<Button>();
				button.onClick.AddListener(() => OnChoiceClicked(choice));

				_buttons[i] = button;
			}

			Hide();
		}

		private void OnChoiceClicked(Choice choice)
		{
			PlayerStatsManager.Instance.Buy(choice.Tower.price);
			TowerContainer.tower = Instantiate(choice.gameObject, TowerContainer.transform);
			Hide();
		}

		public void Show()
		{
			_canvas.enabled = true;
			for (int i = 0; i < choices.Length; i++)
				_buttons[i].interactable = PlayerStatsManager.Instance.HaveEnoughMoneyFor(choices[i].Tower.price);
		}

		public void Hide()
		{
			_canvas.enabled = false;
		}

		public bool IsOpen => _canvas.enabled;
	}

	[Serializable]
	internal class Choice
	{
		public GameObject gameObject;
		public Tower Tower => gameObject.GetComponent<Tower>();
		public string label;
	}
}
