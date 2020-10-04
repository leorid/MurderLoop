using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class RotateCam : MonoBehaviour
	{
		public static Vector2 sensitivity = new Vector2(300, 300);
		[SerializeField] Vector2 _xAxisLimits;
		public static bool cursorLock;

		// Start is called before the first frame update
		void Start()
		{
			cursorLock = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.L))
			{
				cursorLock = !cursorLock;
			}

			if (!cursorLock)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				return;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			Vector2 mouseInput =
				new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

			float yAxisRotation = mouseInput.x * Time.deltaTime * sensitivity.x;
			transform.Rotate(0, yAxisRotation, 0, Space.World);

			float xAxisRotation = mouseInput.y * Time.deltaTime * -sensitivity.y;
			transform.Rotate(xAxisRotation, 0, 0, Space.Self);

			// keep in angle
			if (Vector3.Angle(transform.forward, Vector3.up) > _xAxisLimits.x ||
				Vector3.Angle(transform.forward, Vector3.up) < _xAxisLimits.y)
			{
				transform.Rotate(-xAxisRotation, 0, 0, Space.Self);
			}
		}
	}
}
