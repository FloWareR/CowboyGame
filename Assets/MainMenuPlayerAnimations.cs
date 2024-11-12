using UnityEngine;

public class RandomAnimationController : MonoBehaviour
{
    public Animator animator;

    // Animation parameters
    private readonly string[] animationTriggers = { "Kneel", "Stand", "Point" };
    private float nextActionTime;

    void Start()
    {
        ScheduleNextAnimation();
    }

    void ScheduleNextAnimation()
    {
        float delay = Random.Range(1f, 5f);
        nextActionTime = Time.time + delay;
    }

    void Update()
    {
        if (Time.time >= nextActionTime)
        {
            PlayRandomAnimation();
            ScheduleNextAnimation();
        }
    }

    void PlayRandomAnimation()
    {
        int randomIndex = Random.Range(0, animationTriggers.Length);
        string chosenAnimation = animationTriggers[randomIndex];

        foreach (var trigger in animationTriggers)
        {
            animator.ResetTrigger(trigger);
        }

        animator.SetTrigger(chosenAnimation);
    }
}