using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Interactable))]
public class TwoWayLever : MonoBehaviour
{
    private Interactable interactable;

    private GameObject player;

    private Animator animator;

    private AudioSource audioSource;

    [HideInInspector]
    public int currentPosition;

    public int leverID = 0;

    public delegate void LeverPulled(int ID, int newPos);
    public static event LeverPulled leverPulled;

    protected void Awake()
    {
        GameManager.playerMade += SetPlayer;
        GameState.updateTwoWayLeverPos += UpdatePos;

        InputManager.savedControls.Game.Interact.performed += _ => TestPull();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();
        audioSource = GetComponent<AudioSource>();

        currentPosition = Random.Range(0, 2);
        animator.SetInteger("position", currentPosition);
    }

    private void OnDestroy()
    {
        GameManager.playerMade -= SetPlayer;
        GameState.updateTwoWayLeverPos -= UpdatePos;
        InputManager.savedControls.Game.Interact.performed -= _ => TestPull();
    }

    //check if the lever is pulled and play the correct position
    void TestPull()
    {
        if (player != null)
        {
            if (interactable.lookingAtTarget)
            {
                currentPosition += 1;
                if (currentPosition > 1)
                {
                    currentPosition = 0;
                }
                animator.SetInteger("position", currentPosition);

                leverPulled?.Invoke(leverID, currentPosition);


                audioSource.Play();
            }
        }
    }

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }

    //updates the lever position when another player pulled it
    void UpdatePos(UpdateTwoWayLeverPositionMessage message)
    {
        if (message.leverID == leverID)
        {
            currentPosition = message.leverPosition;
            animator.SetInteger("position", currentPosition);

            audioSource.Play();
        }
    }
}
