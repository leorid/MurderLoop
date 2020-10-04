using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JL
{
	public class HeadHealth : MonoBehaviour
	{
		[SerializeField] BodyPart head;
		[SerializeField] int damagePerPart = 10;
		[SerializeField] List<BodyPart> bodyParts = new List<BodyPart>();
		bool _consumed = false;
		public int value = 1;
		[SerializeField] List<TextMeshPro> textMeshPros = new List<TextMeshPro>();
		[SerializeField] AudioSource _headCombineAudio;

		IEnumerator Start()
		{
			UpdateText();

			foreach (TextMeshPro tmp in textMeshPros)
			{
				tmp.gameObject.SetActive(false);
			}

			while (head.health > 0)
			{
				for (int i = bodyParts.Count - 1; i >= 0; i--)
				{
					BodyPart current = bodyParts[i];
					if (current.health <= 0)
					{
						head.Damage(damagePerPart);
						bodyParts.RemoveAt(i);
					}
				}
				yield return null;
			}

			foreach (TextMeshPro tmp in textMeshPros)
			{
				tmp.gameObject.SetActive(true);
			}
		}

		public void UpdateText()
		{
			foreach (TextMeshPro tmp in textMeshPros)
			{
				tmp.text = value.ToString();
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_consumed || head.health > 0) return;
			if (collision.collider.TryGetComponent(out HeadHealth other))
			{
				if (other.head.health > 0) return;

				other._consumed = true;
				value += other.value;

				UpdateText();

				if (_headCombineAudio) _headCombineAudio.Play();

				Destroy(other.gameObject);
			}
		}
	}
}
