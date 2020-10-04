using JL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	[SerializeField] GameObject _menu;
	[SerializeField] TMPro.TMP_InputField _volume;
	[SerializeField] TMPro.TMP_InputField _mouseX;
	[SerializeField] TMPro.TMP_InputField _mouseY;

	string lastVolumeText;
	string lastMouseXText;
	string lastMouseYText;

	bool _menuVisible = true;

	void Start()
	{
		lastVolumeText = _volume.text;
		lastMouseXText = _mouseX.text;
		lastMouseYText = _mouseY.text;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
		{
			_menuVisible = !_menuVisible;
			RotateCam.cursorLock = !_menuVisible;
			_menu.SetActive(_menuVisible);
		}
	}

	public void OnVolumeChanged(string temp)
	{
		string volumeTxt = _volume.text;
		if (float.TryParse(volumeTxt, out float volume))
		{
			volume = Mathf.Clamp01(volume);
			lastVolumeText = volume.ToString();
			AudioListener.volume = volume;
		}
		else
		{
			_volume.text = lastVolumeText;
		}
	}
	public void OnMouseXChanged(string temp)
	{
		string valueTxt = _mouseX.text;
		if (float.TryParse(valueTxt, out float value))
		{
			lastMouseXText = _mouseX.text;
			RotateCam.sensitivity.x = value;
		}
		else
		{
			_mouseX.text = lastMouseXText;
		}
	}
	public void OnMouseYChanged(string temp)
	{
		string valueTxt = _mouseY.text;
		if (float.TryParse(valueTxt, out float value))
		{
			lastMouseYText = _mouseY.text;
			RotateCam.sensitivity.y = value;
		}
		else
		{
			_mouseY.text = lastMouseYText;
		}
	}

}
