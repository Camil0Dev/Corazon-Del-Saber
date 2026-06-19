using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetHit()
    {
        animator.SetTrigger("Hit");
    }
}