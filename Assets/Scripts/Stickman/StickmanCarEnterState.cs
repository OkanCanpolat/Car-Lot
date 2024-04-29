using System.Collections;
using UnityEngine;

public class StickmanCarEnterState : IStickmanState
{
    public Car Car;
    private Stickman stickman;
    private Animator animator;
    private IEmotion emotion;
    public StickmanCarEnterState(Stickman stickman)
    {
        this.stickman = stickman;
        emotion = stickman.GetComponent<IEmotion>();
        animator = stickman.GetComponent<Animator>();
    }
    public void OnEnter()
    {
        stickman.StartCoroutine(MoveEnterPos());
    }
    public void OnExit()
    {
    }
    private IEnumerator MoveEnterPos()
    {
        Vector3 targetPos = Car.GetEnterPosition(stickman.TargetTile).position;
        Vector3 currentPos = stickman.transform.position;
        Vector3 relTargetPos = new Vector3(targetPos.x, currentPos.y, targetPos.z);
        Quaternion lookAt = Quaternion.LookRotation(relTargetPos - currentPos);

        animator.SetBool("Run", true);

        float t = 0;

        while (t < 1)
        {
            stickman.transform.position = Vector3.Lerp(currentPos, relTargetPos, t);
            stickman.transform.rotation = Quaternion.Slerp(stickman.transform.rotation, lookAt, t);
            t += Time.deltaTime * 4f;
            yield return null;
        }

        stickman.transform.position = relTargetPos;
        animator.SetBool("Run", false);

        Vector3 enterLookAt = Car.GetEnterLookAt(stickman.TargetTile).position;
        Vector3 relEnterLookAt = new Vector3(enterLookAt.x, currentPos.y, enterLookAt.z);

        yield return stickman.StartCoroutine(LookAtSmooth(relEnterLookAt));
        animator.SetTrigger("Drive");
        yield return Car.StartCoroutine(Car.OpenDoor(stickman.TargetTile));
        yield return stickman.StartCoroutine(EnterCar());
        emotion.NoEmotion();
        yield return Car.StartCoroutine(Car.CloseDoor(stickman.TargetTile));
        yield return new WaitForSeconds(0.5f);
        Object.Destroy(stickman.gameObject);
        Car.StateMachine.ChangeState(Car.IdleState);
    }

    private IEnumerator LookAtSmooth(Vector3 target)
    {
        Quaternion look = Quaternion.LookRotation(target - stickman.transform.position);
        float t = 0;

        while (t < 1)
        {
            stickman.transform.rotation = Quaternion.Slerp(stickman.transform.rotation, look, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }
        stickman.transform.rotation = look;
    }
    private IEnumerator EnterCar()
    {
        animator.SetTrigger("Crouch");

        float t = 0;
        Vector3 start = stickman.transform.position;
        Vector3 target = Car.SeatPosition.position;

        while (t < 1)
        {
            stickman.transform.position = Vector3.Lerp(start, target, t);
            t += Time.deltaTime * 2;
            yield return null;
        }
    }
}
