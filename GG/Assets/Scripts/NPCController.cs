using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Animator animator;
    public AnimationClip sitClip;
    public AnimationClip clapClip;

    public float minDelay = 1f;
    public float maxDelay = 3f;

    private float nextAnimationTime;
    private bool isSitting = false;
    private float sitStartTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ScheduleNextAnimation();
    }

    private void ScheduleNextAnimation()
    {
        float delay = Random.Range(minDelay, maxDelay);
        nextAnimationTime = Time.time + delay;
    }

    private void Update()
    {
        if (Time.time >= nextAnimationTime)
        {
            PlayRandomAnimation();
            ScheduleNextAnimation();
        }

        if (isSitting && Time.time - sitStartTime >= 30f)
        {
            StopSitting();
        }
    }

    private void PlayRandomAnimation()
    {
        bool shouldClap = Random.value < 0.5f;

        if (shouldClap)
        {
            if (isSitting)
            {
                animator.SetBool("SitClapTrigger", true);
                animator.Play(clapClip.name);
                isSitting = false;
            }
            else
            {
                animator.SetBool("SitClapTrigger", true);
                animator.Play(clapClip.name);
            }
        }
        else
        {
            if (!isSitting)
            {
                animator.SetBool("SitClapTrigger", false);
                animator.Play(sitClip.name);
                isSitting = true;
                sitStartTime = Time.time;
            }
            else
            {
                animator.SetBool("SitClapTrigger", false);
                animator.Play(sitClip.name);
            }
        }
    }

    private void StopSitting()
    {
        isSitting = false;
        animator.SetBool("SitClapTrigger", true);
        animator.Play(clapClip.name);
    }
}
