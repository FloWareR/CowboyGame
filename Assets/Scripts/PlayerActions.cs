using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private AnimatorManager _animatorManager;
    private PlayerLocomotion _playerLocomotion;

    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void HandlePrimaryAction()
    {
        _animatorManager.PlayTargetAnimation("SayainAttack", true);
        _playerLocomotion.RotateAtLookAt();
    }
}
