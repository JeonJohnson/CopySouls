//using System.Collections;
//using System.Collections.Generic;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class Funcs
{
	public static string GetEnumName<T>(int index) where T : struct, IConvertible
	{//where 조건 struct, IConvertible => Enum으로 제한
		return Enum.GetName(typeof(T), index);
	}

	public static string TrimUnderBar(string str)
	{
		return str.Replace('_', ' ');
	}

	public static int B2I(bool boolean)
	{
		//false => 값 무 (0)
		//true => 값 유 
		return Convert.ToInt32(boolean);
	}

	public static bool I2B(int integer)
	{
		return Convert.ToBoolean(integer);
	}

	public static bool IntegerRandomCheck(int percent)
	{
		int rand = UnityEngine.Random.Range(1, 101);

		if (rand > percent)
		{
			return false;
		}

		return true;
	}


	public static int DontOverlapRand(int curNum, int min, int ExclusiveMax)
	{
		int iRand;

		while (true)
		{
			iRand = UnityEngine.Random.Range(min, ExclusiveMax);

			if (iRand != curNum)
			{
				break;
			}
		}

		return iRand;
	}

	public static Vector3 Random(Vector3 min, Vector3 max)
	{
		float x = UnityEngine.Random.Range(min.x, max.x);
		float y = UnityEngine.Random.Range(min.y, max.y);
		float z = UnityEngine.Random.Range(min.z, max.z);

		return new Vector3(x, y, z);
	}

	public static Vector3 Vec3_Random(float min, float max)
	{
		float x = UnityEngine.Random.Range(min, max);
		float y = UnityEngine.Random.Range(min, max);
		float z = UnityEngine.Random.Range(min, max);

		return new Vector3(x, y, z);
	}

	public static Structs.RayResult RayToWorld(Vector2 screenPos)
	{
		//이걸 그냥 충돌한 놈이 그라운드 일때만 리턴하게?
		//아니면 소환하는 곳에서 충돌된 놈이 그라운드가 아니면 그 새기 크기 판단해서 옆에 생성되게?

		Structs.RayResult rayResult = new Structs.RayResult();

		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		RaycastHit castHit;

		if (Physics.Raycast(ray, out castHit))
		{
			rayResult.hitPosition = castHit.point;
			rayResult.hitPosition.y = 0f;
			rayResult.hitObj = castHit.transform.gameObject;
			rayResult.isHit = true;
			rayResult.ray = ray;
			rayResult.rayHit = castHit;
		}
		else
		{
			rayResult.isHit = false;
		}

		return rayResult;
	}

	public static void ChangeMesh(GameObject origin, Mesh mesh)
	{
		MeshFilter tempFilter = origin.GetComponent<MeshFilter>();

		if (tempFilter != null)
		{
			tempFilter.mesh = mesh;
		}
	}



	public static GameObject FindGameObjectInChildrenByName(GameObject Parent, string ObjName)
	{
		if (Parent == null)
		{
			return null;
		}

		//그냥 transform.Find 로 찾으면 한 단계 아래 자식들만 확인함.
		int childrenCount = Parent.transform.childCount;

		GameObject[] findObjs = new GameObject[childrenCount];

		if (Parent.name == ObjName)
		{
			return Parent;
		}

		if (childrenCount == 0)
		{
			return null;
		}
		else
		{
			for (int i = 0; i < childrenCount; ++i)
			{
				findObjs[i] = FindGameObjectInChildrenByName(Parent.transform.GetChild(i).gameObject, ObjName);

				if (findObjs[i] != null && findObjs[i].name == ObjName)
				{
					return findObjs[i];
				}
			}

			return null;
		}
	}

	public static GameObject FindGameObjectInChildrenByTag(GameObject Parent, string ObjTag)
	{
		if (Parent == null)
		{
			return null;
		}

		int childrenCount = Parent.transform.childCount;

		GameObject[] findObjs = new GameObject[childrenCount];

		if (Parent.CompareTag(ObjTag))
		{
			return Parent;
		}

		if (childrenCount == 0)
		{
			return null;
		}
		else
		{
			for (int i = 0; i < childrenCount; ++i)
			{
				findObjs[i] = FindGameObjectInChildrenByTag(Parent.transform.GetChild(i).gameObject, ObjTag);

				if (findObjs[i] != null && findObjs[i].CompareTag(ObjTag))
				{
					return findObjs[i];
				}
			}
			return null;
		}
	}
	public static T FindComponentInNearestParent<T>(Transform curTransform) where T : Component
	{
		if (curTransform == null)
		{
			return null;
		}

		T tempComponent = curTransform.GetComponent<T>();

		if (tempComponent == null)
		{
			if (curTransform.parent != null)
			{
				tempComponent = FindComponentInNearestParent<T>(curTransform.parent);
			}
			else
			{
				return null;
			}
		}

		return tempComponent;
	}


	public static void RagdollObjTransformSetting(Transform originObj, Transform ragdollObj)
	{
		for (int i = 0; i < originObj.childCount; ++i)
		{
			if (originObj.childCount != 0)
			{
				RagdollObjTransformSetting(originObj.GetChild(i), ragdollObj.GetChild(i));
			}

			ragdollObj.GetChild(i).localPosition = originObj.GetChild(i).localPosition;
			ragdollObj.GetChild(i).localRotation = originObj.GetChild(i).localRotation;
		}
	}



	public static bool IsAnimationAlmostFinish(Animator animCtrl, string animationName, float ratio = 0.95f)
	{
		if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
			if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsAnimationCompletelyFinish(Animator animCtrl, string animationName, float ratio = 1.0f)
	{
		if (animCtrl.GetCurrentAnimatorStateInfo(0).IsName(animationName))
		{//여기서 IsName은 애니메이션클립 이름이 아니라 애니메이터 안에 있는 노드이름임
			if (animCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime >= ratio)
			{
				return true;
			}
		}
		return false;
	}

	public static T FindResourceFile<T>(string path) where T : UnityEngine.Object
	{
		T temp = Resources.Load<T>(path);

		if (temp == null)
		{
			Debug.Log(path + "\nhas not exist!");
		}

		return temp;
	}

	public static GameObject CheckGameObjectExist(string name)
	{
		GameObject temp = GameObject.Find(name);

		if (temp == null)
		{
			temp = new GameObject();
			temp.name = name;
		}

		return temp;
	}

	public static GameObject CheckGameObjectExist<T>(string name) where T : Component
	{
		GameObject temp = GameObject.Find(name);

		if (temp == null)
		{
			temp = new GameObject();
			temp.name = name;
		}

		T tempComponent = temp.GetComponent<T>();

		if (tempComponent == null)
		{
			temp.AddComponent<T>();
		}

		return temp;
	}

	public static T CheckComponentExist<T>(string gameObjectName) where T : Component
	{
		GameObject temp = GameObject.Find(gameObjectName);

		if (temp == null)
		{
			temp = new GameObject();
			temp.name = gameObjectName;
		}

		T tempComponent = temp.GetComponent<T>();

		if (tempComponent == null)
		{
			tempComponent = temp.AddComponent<T>();
		}

		return tempComponent;
	}


	public static string ReadTextFile(string filePath)
	{ //StreamingAssets/리소스내의 폴더 위치 적어주면 댐.
	  //유니티가 최종 실행파일을 빌드할때 Resources 내의 모든 파일들을 바이너리화 해서 뽑지 않음.
	  //사용되는것들(ex. 프리팹,모델,스크립트,Resources.load등으로 불러오는것만)만 뽑음
	  //StreamingAssets폴더내의 있는 파일들은 다 뽑아내기때문에
	  //유니티에서 지원하는?? 파일형식 제외하고는 저기 넣어야 안정적이겠즤

		string path = Application.streamingAssetsPath + "/";
		path += filePath + ".txt";


		FileInfo fileInfo = new FileInfo(path);
		string text;
		if (fileInfo.Exists)
		{
			StreamReader reader = new StreamReader(path);
			text = reader.ReadToEnd();
			reader.Close();
		}
		else
		{
			Debug.Log(filePath + "에는 파일이 없읍니다. 확인요망");
			text = "TextFile has no exist";
		}

		return text;
	}

	
	//public static Structs.WeaponStatus WeaponStatusFromText(string filePath)
	//{
	//	Structs.WeaponStatus tempStatus = new Structs.WeaponStatus();

	//	string statusLine = ReadTextFile(filePath);

	//	if (statusLine == null)
	//	{
	//		Debug.Log("무기 스텟 불러오기 실패");
	//	}

	//	string[] splitStr = statusLine.Split(new char[] { ':' });

	//	for (int i = 0; i < splitStr.Length; ++i)
	//	{
	//		string tempStr = "";
	//		for (int k = 0; k < splitStr[i].Length; ++k)
	//		{
	//			if (splitStr[i][k] == '\r' || splitStr[i][k] == '\n')
	//			{
	//				break;
	//			}
	//			tempStr += splitStr[i][k];
	//		}
	//		splitStr[i] = tempStr;
	//	}

	//	//name
	//	tempStatus.name = splitStr[0];
	//	//name

	//	//type
	//	if (splitStr[1] == "AR")
	//	{
	//		tempStatus.type = Enums.eWeaponType.AR;
	//	}
	//	else if (splitStr[1] == "SMG")
	//	{
	//		tempStatus.type = Enums.eWeaponType.SMG;
	//	}
	//	else if (splitStr[1] == "SR")
	//	{
	//		tempStatus.type = Enums.eWeaponType.SR;
	//	}
	//	else if (splitStr[1] == "Shotgun")
	//	{
	//		tempStatus.type = Enums.eWeaponType.Shotgun;
	//	}
	//	else if (splitStr[1] == "Pistol")
	//	{
	//		tempStatus.type = Enums.eWeaponType.Pistol;
	//	}
	//	else if (splitStr[1] == "Melee")
	//	{ tempStatus.type = Enums.eWeaponType.Melee; }
	//	else if (splitStr[1] == "Explosive")
	//	{ tempStatus.type = Enums.eWeaponType.Explosive; }
	//	//type


	//	//slot
	//	if (splitStr[2] == "Primary")
	//	{
	//		tempStatus.slot = Enums.eWeaponSlot.Primary;
	//	}
	//	else if (splitStr[2] == "Secondary")
	//	{
	//		tempStatus.slot = Enums.eWeaponSlot.Secondary;
	//	}
	//	else if (splitStr[2] == "Melee")
	//	{
	//		tempStatus.slot = Enums.eWeaponSlot.Melee;
	//	}
	//	else if (splitStr[2] == "Special")
	//	{
	//		tempStatus.slot = Enums.eWeaponSlot.Special;
	//	}
	//	else if (splitStr[2] == "Grenade")
	//	{
	//		tempStatus.slot = Enums.eWeaponSlot.Grenade;
	//	}
	//	else if (splitStr[2] == "HealPack")
	//	{
	//		tempStatus.slot = Enums.eWeaponSlot.HealPack;
	//	}
	//	//slot


	//	//firemode
	//	if (splitStr[3] == "Auto")
	//	{
	//		tempStatus.fireMode = Enums.eWeaponFireMode.FullAuto;
	//	}
	//	else if (splitStr[3] == "SemiAuto")
	//	{
	//		tempStatus.fireMode = Enums.eWeaponFireMode.SemiAuto;
	//	}
	//	//firemode

	//	tempStatus.zoomX = Convert.ToSingle(splitStr[4]);

	//	tempStatus.fullMag = Convert.ToInt32(splitStr[5]);
	//	tempStatus.curMag = Convert.ToInt32(splitStr[6]);
	//	tempStatus.fullRound = Convert.ToInt32(splitStr[7]);
	//	tempStatus.curRound = Convert.ToInt32(splitStr[8]);
	//	tempStatus.roundCount = Convert.ToInt32(splitStr[9]);
	//	tempStatus.accuracy = Convert.ToInt32(splitStr[10]);
	//	tempStatus.recoil = Convert.ToInt32(splitStr[11]);
	//	tempStatus.rpm = Convert.ToInt32(splitStr[12]);
	//	tempStatus.dmg = Convert.ToInt32(splitStr[13]);
	//	tempStatus.dmg2 = Convert.ToInt32(splitStr[14]);
	//	tempStatus.reloadSpd = Convert.ToInt32(splitStr[15]);

	//	tempStatus.price = Convert.ToInt32(splitStr[16]);
	//	tempStatus.bulletPrice = Convert.ToInt32(splitStr[17]);

	//	tempStatus.curRapidTime = 0f;
	//	tempStatus.fullRapidTime = 1f / (tempStatus.rpm / 60f);
	//	//fullRapidTime

	//	return tempStatus;
	//}

	#region oldProject
	//public static GameObject FindClosestUnit(GameObject me, List<GameObject> objList)
	//{
	//	float lowDist = -1f;
	//	//float searchRange = me.GetComponent<Units>().unitStatus.sightRange;
	//	GameObject closestObj = null;

	//	foreach (GameObject obj in objList)
	//	{
	//		float dist = Vector3.Magnitude(obj.transform.position - me.transform.position);

	//		if (lowDist > dist || lowDist < 0f)
	//		{
	//			lowDist = dist;
	//			closestObj = obj;
	//		}
	//	}

	//	return closestObj;
	//}

	//public static List<GameObject> FindUnitsInSightRange(GameObject me, List<GameObject> objList)
	//{
	//	List<GameObject> unitsInRange = new List<GameObject>();
	//	unitsInRange.Clear();

	//	Units temp = me.GetComponent<Units>();
	//	float searchRange = 0f;

	//	if (temp != null)
	//	{ 
	//		searchRange = temp.unitStatus.sightRange;
	//	}

	//	foreach (GameObject obj in objList)
	//	{
	//		float dist = Vector3.Magnitude(obj.transform.position - me.transform.position);

	//		if (dist <= searchRange)
	//		{
	//			unitsInRange.Add(obj);
	//		}
	//	}

	//	return unitsInRange;
	//}
	#endregion
}

public static class Defines
{
	public const int right = 1;
	public const int left = 0;

	public const int ally = 0;
	public const int enemy = 1;

	public const float winCX = 1600f;
	public const float winCY = 900f;

	public const float gravity = -9.8f;

	public const float PI = 3.14159265f;


	public const int zedBodyDestructionRandPercent = 100;

	public const int BulletTotalCount = 150;


	/// Zed Count
	public const int ClotCount = 50;



	public const int ShopCount = 4;



	/// Zed Count


	//public static Vector3 test { get { return new Vector3(); } }

	public static Vector3[] test =
	{
		new Vector3(0f,0f,0f)
	};

	public static string[] zedBodyPartStr =
	{
		"Head",
		"Arm_L",
		"Arm_R",
		"Leg_L",
		"Leg_R",
	};



	//c# 가변배열(다중배열) 초기화 방법
	#region WeaponStr
	public static string[] primaryWeaponStr =
	{
		"AK47",
		"SW M29 Dual",
		"FN FAL",
		"M14 EBR",
		"M1897 TrenchGun",
		"Winchester M1894"
	};

	public static string[] secondaryWeaponStr =
	{
		"Beretta 92FS",
		"Mac10"
	};

	public static string[] meleeWeaponStr =
	{
		"Axe",
		"Katana",
		"M9 Knife"
	};

	public static string[] specialWeaponStr =
	{
		"AA12",
		"ChainSaw",
		"Barrett M99"
	};

	public static string[] grenadeWeaponStr =
	{
		"Grenade"

	};

	public static string[] healPackWeaponStr =
	{
		"HealPack"
	};



	public static string[][] weaponStr = /*new string[(int)Enums.eWeaponState.End][];*/
	{
		primaryWeaponStr,
		secondaryWeaponStr,
		meleeWeaponStr,
		specialWeaponStr,
		grenadeWeaponStr,
		healPackWeaponStr
	};
	#endregion


	public static string[] tradeDoorStr =
	{
		"Church",
		"OldShop_1",
		"OldShop_2",
		"HeavyGate_L",
		"HeavyGate_R",
		"HalfLight"
	};
}

namespace Enums
{
	public enum eScenes
	{
		Intro,
		Title,
		Menu,
		InGame,
		End
	}


	public enum ePoolingObj
	{
	
	}

    public enum eEnmeyState
    {
		Idle,
		Move,
		Run,
		Atk,
		Hit,
		Death,

		End
    }



    public enum eEnemyType
	{ 
		Normal,
		Elite,
		Boss,
		End
	}

	public enum eWeaponType
	{
        Melee,
        Range,
        Magic,
        End
    }


	public enum ePlayerState
	{
		Idle,
		Move,
		Atk,
		Hit,
		Death,

		End
	}


}

namespace Structs
{
	[System.Serializable]
	public struct RayResult
	{
		public bool isHit;
		public Vector3 hitPosition;
		public GameObject hitObj;
		public Ray ray;
		public RaycastHit rayHit;
	}




	[System.Serializable]
	public struct EnemyStatus
	{
		public string name;

		public Enums.eEnemyType type;

		public int maxHp;
		public int curHp;

		public int maxStamina;
		public int curStamina;

		public int maxMp;
		public int curMp;


		public float moveSpd;
		public float runSpd;

		public float atkRange;
		public float patrolRange;

		public float lookAtSpd;

		public bool isSuperArmor;
		public bool isGroggy;

		//공격력 -> 무기쪽에서

	}

	[System.Serializable]
	public struct WeaponStatus
	{

		//public string name;
		//public Enums.eWeaponName eName;

		//public Enums.eWeaponType type;
		//public Enums.eWeaponSlot slot;
		//public Enums.eWeaponFireMode fireMode;//단발,연발 

		//public float zoomX; //줌배율 (fov 줄일꺼)

		//public int fullMag;
		//public int curMag;

		//public int fullRound;
		//public int curRound; //현재 총알

		//public int roundCount; //한번 발사시 나가는 총알 개수(샷건용)


		//public float accuracy; //명중률

		//public float recoil;

		//public float rpm; // round per minute

		//public float curRapidTime;
		//public float fullRapidTime;

		//public float dmg; //발당 뎀쥐
		//public float dmg2; //특수공격 뎀쥐

		//public float reloadSpd; //기본적으로 애니메이션 따라가는데 직업마다 속도 배속다르게
		//						////Default Status////

		//public int price;
		//public int bulletPrice;

	}

	public struct DamagedStruct
	{
		////폭팔용
		//public DamagedStruct(float _dmg, Vector3 _explosionPos)
		//{
		//	weaponType = Enums.eWeaponType.Explosive;
		//	dmg = _dmg;
		//	hitCol = null;
		//	explosionPos = _explosionPos;
		//	dmgDir = new Vector3();
		//}

		////일반 공격용
		//public DamagedStruct(float _dmg, Collider col, Vector3 dir)
		//{
		//	weaponType = Enums.eWeaponType.AR;
		//	dmg = _dmg;
		//	hitCol = col;
		//	explosionPos = new Vector3();
		//	dmgDir = dir;
		//}
		//public DamagedStruct(float _dmg)
		//{
		//	weaponType = Enums.eWeaponType.AR;
		//	dmg = _dmg;
		//	hitCol = null;
		//	explosionPos = new Vector3();
		//	dmgDir = new Vector3();
		//}

		//public Enums.eWeaponType weaponType;
		//public float dmg;
		//public Collider hitCol;
		//public Vector3 explosionPos;
		//public Vector3 dmgDir;

	}


}

