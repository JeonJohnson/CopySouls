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
