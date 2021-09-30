using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLever : MonoBehaviour
{
    public float playerRange;

    TwoWayLeverInteractable leverInteractable;

    GameObject player;

    Animator animator;

    [HideInInspector]
    public int currentPosition;

    public string name;

    private void Start()
    {
        GameManager.playerMade += SetPlayer;

        animator = GetComponent<Animator>();
        leverInteractable = GetComponent<TwoWayLeverInteractable>();

        currentPosition = Random.Range(0, 2);
        animator.SetInteger("position", currentPosition);
    }

    private void Update()
    {
        if(player != null)
        {
            TestPull();
        }
    }

    void TestPull()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < playerRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && leverInteractable.lookingAtTarget)
            {
                currentPosition += 1;
                if (currentPosition > 1)
                {
                    currentPosition = 0;
                }
                animator.SetInteger("position", currentPosition);
                Debug.Log(currentPosition);
            }
        }
    }

    void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}
