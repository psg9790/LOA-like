using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerStats
{
    public int crit;    // ġ��
    public int specialization;  // Ưȭ
    public int swiftness;   // �ż�

    // public int domination;  // ����
    // public int endurance;   // �γ�
    // public int expertise;   // ����

    [HideInInspector] public const int totalStatPoint = 2100;
    [HideInInspector] public const float critPerPoint = 0.0357f;      // ġ���� => ġ��ŸȮ�� ȯ�����
    [HideInInspector] public const float greenDamagePerPoint = 0.0573f;       // Ưȭ���� => ��ݽ�ų������ ȯ�����
    [HideInInspector] public const float speedPerpoint = 0.0174f;     // �żӽ��� => ���̼� ȯ�����
    [HideInInspector] public const float coolDownPerPoint = 0.0214f;      // �żӽ��� => �� ȯ�����

}
public class Player : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rigid;

    public enum State
    {
        Idle,
        Move
    }
    public State state;
    public Vector3 targetPos;

    public bool blockMove = false;
    public bool isSkillPlaying = false;
    public bool alertTriggered;

    public PlayerStats playerStats;


    private void Start()
    {
        StartCoroutine(Idle());
        // StartCoroutine(MoveCourseCalculator());
    }
    private void Update()
    {
        if (MyInput.instance.mouseRight.IsPressed() && !blockMove && !EventSystem.current.IsPointerOverGameObject())
        {
            targetPos = GetMousePosition_Terrain();
            if (Vector3.Distance(targetPos, transform.position) > 0.1f)
            {
                state = State.Move;
            }
        }
    }

    #region moving state
    private IEnumerator Idle()
    {
        anim.SetBool("Move", false);

        while (true)
        {
            yield return null;
            if (state != State.Idle)
            {
                StartCoroutine(state.ToString());
                break;
            }
        }
    }
    Vector3 moveVec;
    Vector3 fixVec;
    private IEnumerator Move()
    {
        anim.SetBool("Move", true);
        while (true)
        {
            // �����̴� �� ��ü�� �浹�ϸ� astar �ٽ� ��û
            yield return null;
            moveVec = targetPos - transform.position;
            moveVec.y = 0;
            moveVec = Vector3.Normalize(moveVec) * 1.5f;


            // if (Physics.Raycast(transform.position + Vector3.up * 0.5f, moveVec * 0.3f, out RaycastHit hit1, 1))
            // {
            //     Debug.DrawRay(transform.position + Vector3.up * 0.5f, moveVec * 0.3f, Color.green, 0.1f);

            //     if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.Normalize(moveVec + transform.right * 2) * 0.5f, out RaycastHit hit, 1))
            //     {
            //         if (!alertTriggered)
            //         {
            //             alertTriggered = true;
            //             fixVec = -transform.right;
            //         }
            //         Debug.DrawRay(transform.position + Vector3.up * 0.5f, hit.point - (transform.position + Vector3.up * 0.5f), Color.magenta);

            //     }
            //     else if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.Normalize(moveVec - transform.right * 2) * 0.5f, out RaycastHit hit0, 1))
            //     {
            //         if (!alertTriggered)
            //         {
            //             alertTriggered = true;
            //             fixVec = transform.right;
            //         }
            //         Debug.DrawRay(transform.position + Vector3.up * 0.5f, hit.point - (transform.position + Vector3.up * 0.5f), Color.magenta);
            //     }
            // }
            // else
            // {
            //     alertTriggered = false;
            //     fixVec = Vector3.zero;
            // }

            rigid.velocity = moveVec;
            transform.DOLookAt(transform.position + moveVec, 0.25f);



            if (Vector3.Distance(targetPos, transform.position) < 0.1f)
            {
                state = State.Idle;
            }
            if (state != State.Move)
            {
                transform.DOKill();
                rigid.velocity = Vector3.zero;
                StartCoroutine(state.ToString());
                break;
            }
        }
    }
    public Vector3 GetMousePosition_Terrain()
    {
        Ray _ray = CameraController.instance.cam.ScreenPointToRay(MyInput.instance.mousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(_ray, out RaycastHit _hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
        {
            Debug.DrawRay(CameraController.instance.transform.position, _hit.point - CameraController.instance.transform.position, Color.red, 1);
            if (Vector3.Distance(_hit.point, transform.position) < 0.2f)
            {
                return transform.position;
            }
            else
            {
                return _hit.point;
            }
        }
        return transform.position;
    }
    public Vector3 GetMousePosition_BlockRaycast()
    {
        Ray _ray = CameraController.instance.cam.ScreenPointToRay(MyInput.instance.mousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(_ray, out RaycastHit _hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Block Raycast")))
        {
            Debug.DrawRay(CameraController.instance.transform.position, _hit.point - CameraController.instance.transform.position, Color.red, 1);
            if (Vector3.Distance(_hit.point, transform.position) < 0.2f)
            {
                return transform.position;
            }
            else
            {
                return _hit.point;
            }
        }
        return transform.position;
    }
    #endregion

    // #region skill
    // Coroutine _skillCo;
    // private IEnumerator Spaceskill()
    // {
    //     rigid.velocity = Vector3.zero;

    //     blockMove = true;
    //     isSkillPlaying = false;
    //     state = State.Idle;
    //     yield return null;

    //     float _startTime = Time.time;
    //     Vector3 _skillDir = GetMousePosition_BlockRaycast();
    //     _skillDir = _skillDir - transform.position;
    //     _skillDir.y = 0;
    //     _skillDir = Vector3.Normalize(_skillDir);

    //     transform.rotation = Quaternion.LookRotation(_skillDir);

    //     anim.SetTrigger("Space");

    //     while (true)
    //     {
    //         if (Time.time - _startTime > 0.15f)
    //         {
    //             break;
    //         }
    //         // rigid.AddForce(_skillDir * 5, ForceMode.Impulse);
    //         if (Time.time - _startTime < 0.05f)
    //         {
    //             rigid.velocity = _skillDir * 35f;
    //         }
    //         else
    //         {
    //             rigid.velocity = Vector3.zero;

    //         }
    //         yield return null;
    //     }
    //     // DOVirtual.DelayedCall(0.5f, () => blockMove = false);
    //     blockMove = false;

    //     while (true)    // ü���Է� ���
    //     {
    //         float _elapsed = Time.time - _startTime;
    //         // rigid.velocity = _skillDir * Mathf.Pow(1 - _elapsed, 2) * 14;
    //         yield return null;

    //         if (_elapsed > 1f)
    //         {
    //             break;
    //         }
    //     }
    // }
    // private IEnumerator Qskill()
    // {
    //     blockMove = true;
    //     isSkillPlaying = true;
    //     state = State.Idle;
    //     yield return null;      // move state�� transform.DoKill ���

    //     float _startTime = Time.time;
    //     Vector3 _skillDir = GetMousePosition_BlockRaycast();
    //     _skillDir = _skillDir - transform.position;
    //     _skillDir.y = 0;
    //     _skillDir = Vector3.Normalize(_skillDir);

    //     transform.rotation = Quaternion.LookRotation(_skillDir);

    //     bool _attacked2 = false;
    //     anim.SetTrigger("Q");

    //     while (true)
    //     {
    //         float _elapsed = Time.time - _startTime;
    //         rigid.velocity = _skillDir * Mathf.Pow(1 - _elapsed, 2) * 14;
    //         yield return null;
    //         if (!_attacked2)
    //         {
    //             if (_elapsed > 0.65f)
    //             {
    //                 Debug.DrawRay(transform.position + Vector3.up, _skillDir * 2, Color.red, 2f);
    //                 _attacked2 = true;
    //             }
    //         }
    //         if (_elapsed > 1f)
    //         {
    //             break;
    //         }
    //     }

    //     isSkillPlaying = false;
    //     blockMove = false;
    // }
    // #endregion
}
