using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御するクラス
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    public float speed;
    [SerializeField, Header("反転するまでの移動時間")]
    public float changeTime = 3.0f;
    [SerializeField, Header("接触時のダメージ")]
    public int damage = 1;
    [SerializeField, Header("水平移動用フラグ オフなら垂直移動")]
    public bool vertical;
    [SerializeField, Header("煙のエフェクト")]
    public ParticleSystem smokeEffect;

    private float moveRange;
 
    private float timer;
    private int direction = 1;
    private bool isBroken;

    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = changeTime;
    }

    void Update()
    {
        if (isBroken) {            
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0) {
            // 移動時間が経過するたびに、移動方向を反転させる
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rb.position;

        // 移動方向フラグに合わせて移動方向を水平か垂直か決める
        if (vertical) {
            anim.SetFloat("MoveX",0);
            anim.SetFloat("MoveY",direction);
            position.y = rb.position.y + speed * Time.deltaTime * direction;        
        } else {
            anim.SetFloat("MoveX", direction);
            anim.SetFloat("MoveY", 0);            
            position.x = rb.position.x + speed * Time.deltaTime * direction;
        }

        // 移動範囲の制限
        //moveRange = Mathf.Clamp(rb.position.y, rb.position.y + 5.0f, rb.position.y - 5.0f);
        // 移動させる
        rb.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        RubyController ruby = col.gameObject.GetComponent<RubyController>();
        if (ruby != null) {
            ruby.ChangeHealth(-damage);
        }
    }

    public void Fix() {
        isBroken = true;
        rb.simulated = false;
        // Fixedアニメを再生し、煙のエフェクトを止める
        anim.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
}
