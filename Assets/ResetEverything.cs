using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetEverything : MonoBehaviour
{
    public void DoReset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
