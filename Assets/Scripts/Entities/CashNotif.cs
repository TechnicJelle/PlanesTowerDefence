using System.Collections;
using TMPro;
using UnityEngine;

namespace Entities
{
	public class CashNotif : MonoBehaviour
	{
		[SerializeField] private float timeToDisplay = 1;
		[SerializeField] private float moveUpSpeed = 1;

		private TMP_Text _text;

		private void Awake()
		{
			_text = GetComponentInChildren<TMP_Text>();
		}

		public void SetAmount(int amount)
		{
			_text.text = $"+{amount}";
			StartCoroutine(Disappear());
		}

		private void Update()
		{
			transform.position += moveUpSpeed * Time.deltaTime * Vector3.up;
		}

		private IEnumerator Disappear()
		{
			yield return new WaitForSeconds(timeToDisplay);
			Destroy(gameObject);
		}
	}
}
