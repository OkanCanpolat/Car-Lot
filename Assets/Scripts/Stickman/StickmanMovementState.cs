using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickmanMovementState : IStickmanState
{
    public List<Tile> TargetPath;
    public bool IsMoving;
    private Queue<List<Tile>> paths;
    private Stickman stickman;
    private Animator animator;
    private float speed = 3f;

    public StickmanMovementState(Stickman stickman)
    {
        this.stickman = stickman;
        animator = stickman.GetComponent<Animator>();
        paths = new Queue<List<Tile>>();
    }
    public void OnEnter()
    {
        paths.Enqueue(TargetPath);

        if (!IsMoving)
        {
            stickman.StartCoroutine(StartMovement());
        }
    }

    public void OnExit()
    {
    }
    private IEnumerator StartMovement()
    {
        animator.SetBool("Run", true);
        IsMoving = true;

        while (paths.Count > 0)
        {
            List<Tile> nextPath = paths.Dequeue();

            for (int i = 0; i < nextPath.Count - 1; i++)
            {
                Vector3 start = nextPath[i].transform.position;
                Vector3 end = nextPath[i + 1].transform.position;
                stickman.transform.LookAt(end);
                float t = 0;

                while (t < 1)
                {
                    stickman.transform.position = Vector3.Lerp(start, end, t);
                    t += Time.deltaTime * speed;
                    yield return null;
                }

                stickman.transform.position = end;
            }
        }

        IsMoving = false;
        animator.SetBool("Run", false);
    }
}
