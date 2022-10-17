using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
	//제네뤽 클래스
	//C++의 템플릿 클래스 비슷한거임

	private static T instance = null;

	public static void InstantiateManager()
	{
		if (instance == null)
		{
			GameObject obj = GameObject.Find(typeof(T).Name);

			if (obj == null)
			{
				obj = new GameObject(typeof(T).Name);
				instance = obj.AddComponent<T>();
			}
			else
			{
				instance = obj.GetComponent<T>();
			}
		}
	}
	public static void InstantiateManagerByPrefab(string prefabFolderPath)
	{
		if (instance == null)
		{
			GameObject prefab = Resources.Load(prefabFolderPath + typeof(T).Name) as GameObject;

			if (prefab == null)
			{
				Debug.LogError(typeof(T).Name + "'s prefab is not exist");
				prefab = new GameObject(typeof(T).Name);
				instance = prefab.AddComponent<T>();

			}
			else
			{
				instance = Instantiate(prefab).GetComponent<T>();

				if (instance == null)
				{
					Debug.LogError(typeof(T).Name + "'s prefab don't include" + typeof(T).Name + " Script!!");
				}
			}

			instance.gameObject.name = instance.gameObject.name.Replace("(Clone)", "");
		}
	}

	public static void InstantiateManagerByPrefab(GameObject prefab, GameObject box/* = null*/)
	{
		if (instance == null)
		{
			if (box != null)
			{
				instance = Instantiate(prefab, box.transform).GetComponent<T>();
			}
			else 
			{
				instance = Instantiate(prefab).GetComponent<T>();
			}

			if (instance == null)
			{
				Debug.LogError(typeof(T).Name + "'s prefab don't include" + typeof(T).Name + " Script!!");
			}

			instance.gameObject.name = instance.gameObject.name.Replace("(Clone)", "");
		}
	}


	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject obj = GameObject.Find(typeof(T).Name);

				if (obj == null)
				{
					obj = new GameObject(typeof(T).Name);
					instance = obj.AddComponent<T>();
				}
				else
				{
					instance = obj.GetComponent<T>();
				}
			}
			return instance;
		}
	}


}
