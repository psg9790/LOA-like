using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public RectTransform rect;
    public DamageDisplayPool pool;

    Sequence mySequence;

    public TextMeshProUGUI text;
    public GameObject isBackAttackgo;

    void Start()
    {
        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //추가
        .OnStart(() =>
        {
            transform.localScale = Vector3.zero;
            GetComponent<CanvasGroup>().alpha = 0;
        })
        .Append(transform.DOScale(1.1f, 0.15f).SetEase(Ease.InFlash).From(0.9f)/*.From(0)*/)
        .Append(transform.DOScale(0.9f, 0.15f).SetEase(Ease.InFlash));

        // .Join(GetComponent<CanvasGroup>().DOFade(1, 1))
        // .SetDelay(0.5f);
    }

    void OnEnable()
    {
        // mySequence.Restart();
    }

    [Button]
    public void DoAction(float _damage, bool _isCrit, Transform _hitTransform, bool _isBackAttack)
    {
        if (DoActionCoroutine != null)
        {
            StopCoroutine(DoActionCoroutine);
        }
        if (DoExpireCoroutine != null)
        {
            StopCoroutine(DoExpireCoroutine);
        }

        text.text = _damage.ToString();
        if (_isCrit)
        {
            text.color = Color.yellow;
        }
        else
        {
            text.color = Color.white;
        }
        transform.position = pool.GetUIPos(_hitTransform);
        rect.anchoredPosition += new Vector2(0, 175);
        if (_isBackAttack)
        {
            isBackAttackgo.SetActive(true);
        }
        else
        {
            isBackAttackgo.SetActive(false);

        }
        DoActionCoroutine = StartCoroutine(DoActionCo(_hitTransform));
        DoExpireCoroutine = StartCoroutine(ExpireCo());
    }
    Coroutine DoActionCoroutine;
    Coroutine DoExpireCoroutine;
    private IEnumerator DoActionCo(Transform _hitTransform)
    {
        mySequence.Restart();

        while (this.gameObject.activeSelf)
        {
            yield return null;
            if (_hitTransform != null)
            {

                transform.position = pool.GetUIPos(_hitTransform);
                rect.anchoredPosition += new Vector2(0, 175);

            }
            else
            {
                pool.Push(this.gameObject);
                StopAllCoroutines();
            }

        }
        DoActionCoroutine = null;
    }
    private IEnumerator ExpireCo()
    {
        yield return new WaitForSeconds(3f);
        // 끝날때
        pool.Push(this.gameObject);
        DoExpireCoroutine = null;
    }
}
