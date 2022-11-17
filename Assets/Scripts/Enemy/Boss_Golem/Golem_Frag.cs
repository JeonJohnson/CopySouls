using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Frag : Enemy_Ragdoll
{
	//이걸 렉돌로 관리해서 쓸꺼임.

	public Animator animCtrl;



	public void Awake()
	{
		animCtrl = GetComponent<Animator>();
	}


	public void Start()
	{
		
	}

	public void Update()
	{
		
	}
	new public void OnEnable()
	{
	}

}
