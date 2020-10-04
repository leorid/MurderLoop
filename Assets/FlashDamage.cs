using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JL
{
	public class FlashDamage : MonoBehaviour
	{
		static FlashDamage instance;
		[SerializeField] Image _image;
		bool _flashRunning;

		private void Awake()
		{
			instance = this;
		}

		public static void Flash()
		{
			instance.DoFlashInternal();
		}

		public void DoFlashInternal()
		{
			if (_flashRunning) return;

			StartCoroutine(DoFlashCoroutine());
			_flashRunning = true;
		}

		IEnumerator DoFlashCoroutine()
		{
			_image.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.2f);

			_image.gameObject.SetActive(false);
			_flashRunning = false;
		}
	}
}
