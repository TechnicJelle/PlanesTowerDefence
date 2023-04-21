using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class RadialSelector : MonoBehaviour
	{
		private static GameObject _canvasPrefab;
		private static GameObject _buttonPrefab;

		private IChoice[] _choices;

		private Canvas _canvas;
		private Button[] _buttons;

		private void Awake()
		{
			if (_canvasPrefab == null)
			{
				_canvasPrefab = Resources.Load<GameObject>("PopupCanvas");
				if (_canvasPrefab == null) Debug.LogError("Canvas prefab not found");
			}

			if (_buttonPrefab == null)
			{
				_buttonPrefab = Resources.Load<GameObject>("Choice Button");
				if (_buttonPrefab == null) Debug.LogError("Choice Button prefab not found");
			}

			_canvas = Instantiate(_canvasPrefab, transform).GetComponent<Canvas>();
			_canvas.worldCamera = Camera.main;

			Hide();
		}

		protected void Init(IChoice[] choices)
		{
			_choices = choices;

			// Create buttons
			_buttons = new Button[_choices.Length];
			for (int i = 0; i < _choices.Length; i++)
			{
				IChoice choice = _choices[i];
				GameObject buttonInstance = Instantiate(_buttonPrefab, _canvas.transform);
				buttonInstance.name = "btn_" + choice.Label;

				buttonInstance.transform.localScale = new Vector3(1f, 1f, 1f);

				// Set button position
				float angle = 2f * Mathf.PI / _choices.Length * i - Mathf.PI - Mathf.PI / _choices.Length;
				buttonInstance.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

				// Set button texts
				TextMeshProUGUI[] texts = buttonInstance.GetComponentsInChildren<TextMeshProUGUI>();

				foreach (TextMeshProUGUI text in texts)
				{
					text.text = text.name switch
					{
						{ } a when a.IndexOf("Name", StringComparison.OrdinalIgnoreCase) >= 0 => choice.Label,
						{ } a when a.IndexOf("Price", StringComparison.OrdinalIgnoreCase) >= 0 => choice.Price == 0 ? "" : choice.Price.ToString(),
						_ => text.text,
					};
				}

				// Set button image
				Image image = buttonInstance.GetComponent<Image>();
				image.sprite = choice.Sprite;

				// Set button click event
				Button button = buttonInstance.GetComponentInChildren<Button>();
				button.onClick.AddListener(() => OnChoiceClicked(choice));

				_buttons[i] = button;
			}
		}

		protected virtual void OnChoiceClicked(IChoice choice)
		{
			PlayerStatsManager.Instance.Buy(choice.Price);
			Hide();
		}

		public void Show()
		{
			_canvas.enabled = true;
		}

		private void Update()
		{
			for (int i = 0; i < _choices.Length; i++)
				_buttons[i].interactable = PlayerStatsManager.Instance.HaveEnoughMoneyFor(_choices[i].Price);
		}

		public void Hide()
		{
			_canvas.enabled = false;
		}

		public bool IsOpen => _canvas.enabled;
	}

	public interface IChoice
	{
		public string Label => null;
		public int Price => -1;
		public Sprite Sprite => null;
	}
}
