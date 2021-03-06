using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWayLever : MonoBehaviour
{
    Interactable interactable;

    GameObject player;

    Animator animator;

    AudioSource audioSource;

    [HideInInspector]
    public int currentPosition;

    public int leverID = 0;

    public delegate void LeverPulled(int ID, int newPos);
    public static event LeverPulled leverPulled;

    private void Awake()
    {
        GameManager.playerMade += SetPlayer;
        GameState.updateThreeWayLeverPos += UpdatePos;

        InputManager.savedControls.Game.Interact.performed += _ => TestPull();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
        audioSource = GetComponent<AudioSource>();

        currentPosition = Random.Range(1, 4);
        animator.SetInteger("Stance", currentPosition);
    }

    private void OnDestroy()
    {
        GameManager.playerMade -= SetPlayer;
        GameState.updateThreeWayLeverPos -= UpdatePos;
        InputManager.savedControls.Game.Interact.performed -= _ => TestPull();
    }

    void TestPull()
    {
        if(player != null)
        {
            if (interactable.lookingAtTarget)
            {
                currentPosition += 1;
                if (currentPosition > 3)
                {
                    currentPosition = 1;
                }
                animator.SetInteger("Stance", currentPosition);


                audioSource.Play();

                leverPulled?.Invoke(leverID, currentPosition);
            }
        }
    }

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }

    void UpdatePos(UpdateThreeWayLeverPositionMessage message)
    {
        if (message.leverID == leverID)
        {
            currentPosition = message.leverPosition;
            animator.SetInteger("Stance", currentPosition);

            audioSource.Play();

        }
    }
}
