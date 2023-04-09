using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class ButtonViewNavigation : MonoBehaviour
	{
		[SerializeField] private View gotoView;

		protected Button ThisButton;
		private View _parentView;

		protected void Awake()
		{
			if (gotoView == null)
				Debug.LogError("Enable View is null");
			ThisButton = GetComponent<Button>();
			_parentView = transform.parent.GetComponentInParent<View>();
		}

		protected void SwitchPanel()
		{
			_parentView.Hide();
			gotoView.Show();
		}
	}
}
