using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    [Required] public Player player;
    [Required] public PlayerSkillList skillList;
    public enum State       // ���� enum�� �ε����� ��ųŸ���� �ε�����
    {
        None = 999,
        Q = 0,
        W = 1,
        E = 2,
        R = 3,
        A = 4,
        S = 5,
        D = 6,
        F = 7,
        Space = 8
    }
    public State state;
    public const float yellowGage = 100f;
    public const float greenGage = 100f;
    public float curYellowGage = 100f;
    public float curGreenGage = 0;

    public Coroutine skillCo;
    public List<bool> active = new List<bool>();
    public List<float> coolTime = new List<float>();
    public List<Image> skillCooldownImageList = new List<Image>();
    public List<Image> skillIconList = new List<Image>();
    public List<Image> spaceIconList = new List<Image>();
    public List<Slider> idenSliderList = new List<Slider>();

    private void Start()
    {
        StartCoroutine(GageEarnCo());
    }

    private void Update()
    {
        IdenUpdate();
        CoolDownUpdate();

        if (state == State.None || state == State.Space)
        {
            if (MyInput.instance.Q.triggered
            && active[(int)State.Q]
            && skillCo == null
            && skillList.list[(int)State.Q].yellowCost <= curYellowGage
            && skillList.list[(int)State.Q].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(Q_skillCo());
            }
            else if (MyInput.instance.W.triggered
            && active[(int)State.W]
            && skillCo == null
            && skillList.list[(int)State.W].yellowCost <= curYellowGage
            && skillList.list[(int)State.W].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(W_skillCo());
            }
            else if (MyInput.instance.E.triggered
            && active[(int)State.E]
            && skillCo == null
            && skillList.list[(int)State.E].yellowCost <= curYellowGage
            && skillList.list[(int)State.E].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(E_skillCo());
            }
            else if (MyInput.instance.R.triggered
            && active[(int)State.R]
            && skillCo == null
            && skillList.list[(int)State.R].yellowCost <= curYellowGage
            && skillList.list[(int)State.R].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(R_skillCo());
            }
            else if (MyInput.instance.A.triggered
            && active[(int)State.A]
            && skillCo == null
            && skillList.list[(int)State.A].yellowCost <= curYellowGage
            && skillList.list[(int)State.A].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(A_skillCo());
            }
            else if (MyInput.instance.S.triggered
            && active[(int)State.S]
            && skillCo == null
            && skillList.list[(int)State.S].yellowCost <= curYellowGage
            && skillList.list[(int)State.S].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(S_skillCo());
            }
            else if (MyInput.instance.D.triggered
            && active[(int)State.D]
            && skillCo == null
            && skillList.list[(int)State.D].yellowCost <= curYellowGage
            && skillList.list[(int)State.D].greenCost <= curGreenGage)
            {
                skillCo = StartCoroutine(D_skillCo());
            }
            else if (MyInput.instance.F.triggered
            && active[(int)State.F]
            && skillCo == null
            && curGreenGage > 0
            /*&& skillList.list[(int)State.F].yellowCost <= curYellowGage*/
            /*&& skillList.list[(int)State.F].greenCost <= curGreenGage*/)
            {
                skillCo = StartCoroutine(F_skillCo());
            }
        }
        if (MyInput.instance.space.triggered && active[(int)State.Space])
        {
            StartCoroutine(Space_skillCo());
        }
    }

    private IEnumerator GageEarnCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            // if (curYellowGage < yellowGage)
            // {
            //     curYellowGage += 18f;
            //     if (curYellowGage > yellowGage)
            //     {
            //         curYellowGage = yellowGage;
            //     }
            // }
            if (curGreenGage < greenGage)
            {
                curGreenGage += 18f;
                if (curGreenGage > greenGage)
                {
                    curGreenGage = greenGage;
                }
            }
        }
    }
    private void IdenUpdate()
    {
        idenSliderList[0].value = curYellowGage / yellowGage;
        idenSliderList[1].value = curGreenGage / greenGage;
    }
    private void CoolDownUpdate()
    {
        for (int x = 0; x < 8; x++)
        {
            if (coolTime[x] > 0)
            {
                coolTime[x] -= Time.deltaTime;
                if (coolTime[x] >= 0)   // fillAmount ��� ��� - ��ٿ� ���÷���
                {
                    skillCooldownImageList[x].fillAmount = coolTime[x] / skillList.list[x].coolDown;
                }
                if (coolTime[x] <= 0)
                {
                    active[x] = true;
                    skillCooldownImageList[x].fillAmount = 0;
                    skillIconList[x].DOColor(Color.white * 1.5f, 0.25f).From();
                }
            }
        }
        if (coolTime[8] >= 0)
        {
            spaceIconList[1].fillAmount = coolTime[8] / skillList.list[8].coolDown;
        }
        if (coolTime[8] > 0)
        {
            coolTime[8] -= Time.deltaTime;

            if (coolTime[8] <= 0)
            {
                active[8] = true;
                spaceIconList[2].enabled = true;
                spaceIconList[0].gameObject.SetActive(false);
            }
        }
    }
    private void UsedSkill(State _skillState)
    {
        state = _skillState;
        active[(int)_skillState] = false;
        coolTime[(int)_skillState] = skillList.list[(int)_skillState].coolDown;

        curYellowGage -= skillList.list[(int)_skillState].yellowCost;
        curYellowGage += skillList.list[(int)_skillState].yellowEarn;
        curGreenGage -= skillList.list[(int)_skillState].greenCost;
        curGreenGage += skillList.list[(int)_skillState].greenEarn;

        curYellowGage = Mathf.Clamp(curYellowGage, 0, yellowGage);
        curGreenGage = Mathf.Clamp(curGreenGage, 0, greenGage);
    }

    private IEnumerator Q_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.Q);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);

        bool _attacked2 = false;
        player.anim.SetTrigger("Q");

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.1f)
            {
                player.rigid.velocity = _skillDir * Mathf.Pow(1 - _elapsed, 2) * 25;
            }
            else if (_elapsed < 0.8f)
            {
                player.rigid.velocity = player.rigid.velocity * 0.95f;
            }
            else if (_elapsed > 0.85f)
            {
                player.rigid.velocity = Vector3.zero;
                break;
            }

            if (!_attacked2)    // ���� �ѹ��� �ϰ� �ϴ� bool
            {
                if (_elapsed > 0.56f)
                {
                    // ��������
                    Debug.DrawRay(transform.position + Vector3.up, _skillDir * 2, Color.red, 2f);
                    _attacked2 = true;
                }
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator W_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.W);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("Q");

        bool atk2gate = false;
        bool atk3gate = false;
        bool atk1 = false;
        bool atk2 = false;
        bool atk3 = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.4f)
            {
                // atk1 �ִϸ��̼�
                if (!atk1)
                {
                    atk1 = true;
                    // player.rigid.AddForce(_skillDir * 6, ForceMode.Impulse);
                    Debug.Log("w1");
                }
            }
            else if (_elapsed < 0.8f)
            {
                if (MyInput.instance.S.IsPressed() || atk2gate)
                {
                    atk2gate = true;
                    if (!atk2)
                    {
                        atk2 = true;
                        // player.rigid.AddForce(_skillDir * 4, ForceMode.Impulse);
                        Debug.Log("w2");
                    }
                }
                else
                {
                    break;
                }
            }
            else if (_elapsed < 1.2f)
            {
                if (MyInput.instance.S.IsPressed() || atk3gate)
                {
                    atk3gate = true;
                    if (!atk3)
                    {
                        atk3 = true;
                        // player.rigid.AddForce(_skillDir * 6, ForceMode.Impulse);
                        Debug.Log("w3");
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
        // player.state = Player.State.Idle;
        // player.blockMove = true;
        // UsedSkill(State.W);
        // yield return null;      // move state�� transform.DoKill ���

        // float _startTime = Time.time;
        // Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        // _skillDir = _skillDir - transform.position;
        // _skillDir.y = 0;
        // _skillDir = Vector3.Normalize(_skillDir);
        // transform.rotation = Quaternion.LookRotation(_skillDir);
        // player.anim.SetTrigger("Q");

        // bool dash = false;

        // while (true)
        // {
        //     yield return null;
        //     float _elapsed = Time.time - _startTime;
        //     if (_elapsed < 0.1f)
        //     {
        //     }
        //     else if (_elapsed < 0.35f)
        //     {
        //         if (!dash)
        //         {
        //             dash = true;
        //             player.rigid.AddForce(_skillDir * 7, ForceMode.Impulse);
        //         }
        //     }
        //     else if (_elapsed < 0.75f)
        //     {
        //         player.rigid.velocity = Vector3.zero;
        //     }
        //     else
        //     {
        //         break;
        //     }
        // }

        // player.blockMove = false;
        // state = State.None;
        // skillCo = null;
        // yield return null;
    }
    private IEnumerator E_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.E);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("E");

        bool dash = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.1f)
            {
            }
            else if (_elapsed < 0.3f)
            {
                if (!dash)
                {
                    dash = true;
                    player.rigid.AddForce(_skillDir * 7, ForceMode.Impulse);
                }
            }
            else if (_elapsed < 0.5f)
            {
                player.rigid.velocity = Vector3.zero;
            }
            else
            {
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
        // player.state = Player.State.Idle;
        // player.blockMove = true;
        // UsedSkill(State.E);
        // yield return null;      // move state�� transform.DoKill ���

        // float _startTime = Time.time;
        // Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        // _skillDir = _skillDir - transform.position;
        // _skillDir.y = 0;
        // if (_skillDir.magnitude > 3)
        // {
        //     _skillDir = Vector3.Normalize(_skillDir) * 3;
        // }

        // transform.rotation = Quaternion.LookRotation(_skillDir);
        // player.anim.SetTrigger("Q");
        // Vector3 startPos = transform.position;

        // while (true)
        // {
        //     yield return null;
        //     float _elapsed = Time.time - _startTime;
        //     if (_elapsed < 0.5f)
        //     {
        //         transform.position = new Vector3(Mathf.Lerp(startPos.x, startPos.x + _skillDir.x, _elapsed),
        //          Mathf.Lerp(0, 1f, (_elapsed * 2)),
        //         Mathf.Lerp(startPos.z, startPos.z + _skillDir.z, _elapsed));
        //     }
        //     else if (_elapsed < 1f)
        //     {
        //         transform.position = new Vector3(Mathf.Lerp(startPos.x, startPos.x + _skillDir.x, _elapsed),
        //         Mathf.Lerp(1f, 0, (_elapsed - 0.5f) * 2),
        //         Mathf.Lerp(startPos.z, startPos.z + _skillDir.z, _elapsed));
        //     }
        //     else
        //     {
        //         break;
        //     }
        // }

        // player.blockMove = false;
        // state = State.None;
        // skillCo = null;
        // yield return null;
    }
    private IEnumerator R_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.R);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("Q");

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;

            if (MyInput.instance.R.IsPressed())
            {
                if (_elapsed < 1.2f)
                {
                    Debug.Log("r");
                }
                else
                {
                    break;
                }
            }
            else
            {
                Debug.Log("ĵ��");

                // ��Ÿ ���
                break;
            }
        }

        // �ļ�Ÿ
        Debug.Log("r final");

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator A_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.A);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("A");

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 1f)
            {

            }
            else
            {
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator S_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.S);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("S");

        bool atk2gate = false;
        bool atk3gate = false;
        bool atk1 = false;
        bool atk2 = false;
        bool atk3 = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.4f)
            {
                // atk1 �ִϸ��̼�
                if (!atk1)
                {
                    atk1 = true;
                    // player.rigid.AddForce(_skillDir * 6, ForceMode.Impulse);
                    Debug.Log("s1");
                }
            }
            else if (_elapsed < 0.8f)
            {
                if (MyInput.instance.S.IsPressed() || atk2gate)
                {
                    atk2gate = true;
                    if (!atk2)
                    {
                        atk2 = true;
                        // player.rigid.AddForce(_skillDir * 4, ForceMode.Impulse);
                        Debug.Log("s2");
                    }
                }
                else
                {
                    break;
                }
            }
            else if (_elapsed < 1.2f)
            {
                if (MyInput.instance.S.IsPressed() || atk3gate)
                {
                    atk3gate = true;
                    if (!atk3)
                    {
                        atk3 = true;
                        // player.rigid.AddForce(_skillDir * 6, ForceMode.Impulse);
                        Debug.Log("s3");
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator D_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.D);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("D");

        bool atk = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.1f)
            {

            }
            else if (_elapsed < 0.8f)
            {
                if (!atk)
                {
                    atk = true;
                    player.rigid.AddForce(_skillDir * 2, ForceMode.Impulse);
                }
            }
            else
            {
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator F_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.F);
        yield return null;      // move state�� transform.DoKill ���

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("F");

        bool knockBack = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed > 0.8f)
            {
                // player.rigid.velocity = player.rigid.velocity * 0.95f;
                if (!knockBack)
                {
                    knockBack = true;
                    player.rigid.AddForce(-transform.forward * 10, ForceMode.Impulse);
                }
            }
            if (_elapsed > 1.25f)
            {
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator Space_skillCo()
    {
        if (skillCo != null)    // ���� �������� ��ų ĵ��
        {
            StopCoroutine(skillCo);
            skillCo = null;
        }

        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.Space);
        spaceIconList[0].gameObject.SetActive(true);    // ���� UI
        yield return null;

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("Space");

        while (true)
        {
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.06f)
            {
                player.rigid.velocity = _skillDir * 35f;
            }
            else
            {
                player.rigid.velocity = Vector3.zero;
            }

            if (_elapsed > 0.1f)
            {
                break;
            }
            yield return null;
        }
        player.blockMove = false;   // move ���� ����, ü�� ���

        while (true)    // ü���Է� ���
        {
            float _elapsed = Time.time - _startTime;
            yield return null;
            if (MyInput.instance.space.triggered)
            {
                spaceIconList[2].enabled = false;
                break;
            }
            if (_elapsed > 1.25f)   // ü�� �Է� ������ ���� ��ƾ Ż��
            {
                spaceIconList[2].enabled = false;
                goto SpaceChainExit;
            }
        }

        //  ü�� �Է��� ������ break �޾Ƽ� �� ���� �������� ���� �� (�ܼ� �ڵ� �ݺ��̶� ����� ã�ƾ���)
        if (skillCo != null)
        {
            Debug.Log("cancle");
            StopCoroutine(skillCo);
            skillCo = null;
        }

        player.state = Player.State.Idle;
        player.blockMove = true;
        state = State.Space;
        yield return null;

        _startTime = Time.time;
        _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("Space");

        while (true)
        {
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.06f)
            {
                player.rigid.velocity = _skillDir * 35f;
            }
            else
            {
                player.rigid.velocity = Vector3.zero;
            }

            if (_elapsed > 0.1f)
            {
                break;
            }
            yield return null;
        }
        player.blockMove = false;

    SpaceChainExit:

        state = State.None;
        yield return null;
    }

}