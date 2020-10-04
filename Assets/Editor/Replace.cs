using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JL
{
	public class Replace : Editor
	{
		[MenuItem("Tools/Replace")]
		static void Relacement()
		{
			GameObject root = Selection.activeGameObject;
			GameObject cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
				"Assets/BloodCube.prefab");

			BodyPart[] bodyParts = root.GetComponentsInChildren<BodyPart>();

			foreach (Transform child in root.GetComponentsInChildren<Transform>())
			{
				if (child.gameObject.name.StartsWith("BloodCube"))
				{
					GameObject go = (GameObject)
						PrefabUtility.InstantiatePrefab(cubePrefab, child.parent);
					Undo.RegisterCreatedObjectUndo(go, "Blood Created");
					go.transform.position = child.position;
					go.name = child.gameObject.name;
					go.transform.rotation = child.rotation;

					ReplaceInBodyParts(bodyParts, child.gameObject, go);
				}
			}
		}

		static void ReplaceInBodyParts(BodyPart[] bodyParts,
			GameObject oldGO, GameObject newGO)
		{
			foreach (BodyPart part in bodyParts)
			{
				if (!part) continue;
				Undo.RecordObject(part, "Assign new BloodCube");
				System.Type type = part.GetType();
				System.Reflection.FieldInfo field = type.GetField("_bloodCubes",
					System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetField);
				object arrayObj = field.GetValue(part);

				if (arrayObj != null)
				{
					GameObject[] array = (GameObject[])arrayObj;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] == oldGO) array[i] = newGO;
					}
				}
			}
		}
	}
}
