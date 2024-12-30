using UnityEngine;

using DG.Tweening;
public class DOForce : MonoBehaviour
{
    [Header("吹っ飛び速度調整"), Range(0, 100)]
    public float knockbackSpeed;
    [Header("吹っ飛び曲線調整")]
    public AnimationCurve Curve;
    public bool isRigid;
    public LayerMask groundMask;
    public float groundDistance = 1;
    public RaycastHit hit;
    public Vector3 CastOffset;

    public EffectAndParticle HitEffect;
    [Range(0, 1)]
    public float ReflectionForce;
    public bool TweeningExecute;

    public bool Punch;

    public void AddForce(GameObject Attacker, float Power)
    {
        Vector3 m_impact_vector = Vector3.one;
        Power*=WorldInfo.scale;
        if (Attacker)
        {
            ColliderDataInput(Attacker,gameObject, ref m_impact_vector, Power);
            AddForce(m_impact_vector, Power);
        }

    }
    public void ColliderDataInput(GameObject a_collider, GameObject a_object, ref Vector3 a_vector, float Power = 30)
    {
        a_vector.Set(
            a_object.transform.position.x - a_collider.transform.position.x,
            0f,
            a_object.transform.position.z - a_collider.transform.position.z
        );
        a_vector.Normalize();
        a_vector *= Power;
    }
    Tween Tween;
    public void AddForce(Vector3 impactVector, float Power)
    {
        Debug.Log("doforce");
        if (!TweeningExecute && Tween.IsPlaying())
            return;

        if (Physics.Raycast(transform.position + CastOffset, impactVector.normalized, out hit, groundDistance, groundMask))
        {
            return;
        }

        if (isRigid)
        {
            GetComponent<Rigidbody>().AddForce(impactVector, ForceMode.Impulse);
        }
        else
        {
            Tweener tween;
            if (Punch)
            {
                tween = transform.DOPunchPosition(
     new Vector3(Power / 7, 0, Power / 7), // パンチの方向と強さ
     0.2f                    // 演出時間
   ).OnComplete(() => { transform.DOLocalMove(impactVector, knockbackSpeed).SetRelative(true).SetEase(Curve); });
            }
            else
            {
                tween = transform.DOLocalMove(impactVector, knockbackSpeed).SetRelative(true).SetEase(Curve);

            }

            tween.OnUpdate(() =>
            {
                if (Physics.SphereCast(transform.position + CastOffset, 1, impactVector.normalized, out hit, groundDistance, groundMask))
                {
                    tween.Kill();
                    if (HitEffect != null)
                    {
                        HitEffect.Execute(hit.point);
                    }

                    if (ReflectionForce != 0)
                    {
                        AddForce(-1 * impactVector * ReflectionForce, Power);
                    }
                }


            });
        }

    }
}
