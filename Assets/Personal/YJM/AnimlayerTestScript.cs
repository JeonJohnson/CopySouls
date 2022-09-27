using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimlayerTestScript : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            print("N");
            animator.SetTrigger("isAttack");
        }
    }
}
