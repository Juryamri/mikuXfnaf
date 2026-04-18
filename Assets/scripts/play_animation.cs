using UnityEngine;

public class play_animation : MonoBehaviour
{
 
    public Animator animator;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);

        if (moveAmount == 0)
        {
            animator.Play("Idle");
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.Play("Running");
        }
        else
        {
            animator.Play("Walking");
        }
    }
}