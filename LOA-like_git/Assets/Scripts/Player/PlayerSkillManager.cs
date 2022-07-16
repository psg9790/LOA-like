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
    public enum State       // 다음 enum의 인덱스는 스킬타입의 인덱스임
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

    public void TryAttackFunc(Vector3 _direction, float _range, PlayerSkillType _skillType)
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, _direction, out RaycastHit _tryhit, _range, 1 << LayerMask.NameToLayer("Monster")))
        {
            Monster _hitMonster = _tryhit.transform.GetComponentInParent<Monster>();
        }
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
            && curGreenGage > 0)
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

            if (curGreenGage < greenGage)
            {
                curGreenGage += 5f;
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
            if (!active[x])
            {
                if (coolTime[x] > 0)
                {
                    coolTime[x] -= Time.deltaTime;
                    if (coolTime[x] >= 0)   // 나누기 에러 방지
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
            else
            {
                if (skillCooldownImageList[x].fillAmount != 0)
                {
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
        yield return null;      // move state의 transform.DoKill 대기

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

            if (!_attacked2)    // 공격 한번만 하게 하는 bool
            {
                if (_elapsed > 0.56f)
                {
                    // 공격판정
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
        yield return null;      // move state의 transform.DoKill 대기

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);

        bool attacked = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 0.6f)
            {
                if (!attacked)
                {
                    attacked = true;
                    player.anim.SetTrigger("W1");
                }
            }
            else
            {
                skillCo = StartCoroutine(W2_skillCo());
                break;
            }
        }

        player.blockMove = false;
        state = State.None;
        skillCo = null;
        yield return null;
    }
    private IEnumerator W2_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        yield return null;

        float _startTime = Time.time;
        Vector3 _skillDir;


        bool doAttack = false;
        bool attacked = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;

            if (!doAttack)
            {
                if (_elapsed < 1.5f)
                {
                    if (MyInput.instance.W.IsPressed())
                    {
                        _skillDir = player.GetMousePosition_BlockRaycast();
                        _skillDir = _skillDir - transform.position;
                        _skillDir.y = 0;
                        _skillDir = Vector3.Normalize(_skillDir);
                        transform.rotation = Quaternion.LookRotation(_skillDir);
                        _startTime = Time.time;
                        doAttack = true;
                        player.blockMove = true;
                        player.state = Player.State.Idle;
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                if (!attacked)
                {
                    attacked = true;
                }


                if (_elapsed > 1f)
                {
                    player.blockMove = false;
                    break;
                }
            }
        }

    }
    private IEnumerator E_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.E);
        yield return null;      // move state의 transform.DoKill 대기

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
    }
    private IEnumerator R_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.R);
        yield return null;      // move state의 transform.DoKill 대기

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("R");

        bool attacked = false;

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;

            if (_elapsed < 0.7f)
            {
                if (!attacked)
                {
                    attacked = true;
                    _skillDir = -_skillDir * 2 + Vector3.up;
                    player.rigid.AddForce(_skillDir * 3, ForceMode.Impulse);
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
    private IEnumerator A_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.A);
        yield return null;      // move state의 transform.DoKill 대기

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
            if (_elapsed < 2.4f)
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
        yield return null;      // move state의 transform.DoKill 대기

        float _startTime = Time.time;
        Vector3 _skillDir = player.GetMousePosition_BlockRaycast();
        _skillDir = _skillDir - transform.position;
        _skillDir.y = 0;
        _skillDir = Vector3.Normalize(_skillDir);
        transform.rotation = Quaternion.LookRotation(_skillDir);
        player.anim.SetTrigger("S");

        while (true)
        {
            yield return null;
            float _elapsed = Time.time - _startTime;
            if (_elapsed < 2.1f)
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
    private IEnumerator D_skillCo()
    {
        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.D);
        yield return null;      // move state의 transform.DoKill 대기

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
                    player.rigid.AddForce(_skillDir * 3, ForceMode.Impulse);
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
        yield return null;      // move state의 transform.DoKill 대기

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
        if (skillCo != null)    // 현재 시전중인 스킬 캔슬
        {
            StopCoroutine(skillCo);
            skillCo = null;
        }

        player.state = Player.State.Idle;
        player.blockMove = true;
        UsedSkill(State.Space);
        spaceIconList[0].gameObject.SetActive(true);    // 스페 UI
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
        player.blockMove = false;   // move 제한 해제, 체인 대기

        while (true)    // 체인입력 대기
        {
            float _elapsed = Time.time - _startTime;
            yield return null;
            if (MyInput.instance.space.triggered)
            {
                spaceIconList[2].enabled = false;
                break;
            }
            if (_elapsed > 1.25f)   // 체인 입력 없으면 스페 루틴 탈출
            {
                spaceIconList[2].enabled = false;
                goto SpaceChainExit;
            }
        }

        //  체인 입력을 받으면 break 받아서 이 이후 구문으로 오게 됨 (단순 코드 반복이라 방법을 찾아야함)
        if (skillCo != null)
        {
            // Debug.Log("cancle");
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
