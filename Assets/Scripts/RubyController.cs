using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー操作やライフ管理のクラス
/// </summary>
public class RubyController : MonoBehaviour {
    [SerializeField, Header("移動速度")]
    public float speed = 3.0f;
    [SerializeField, Header("ライフの最大値")]
    public int maxHealth = 5;
    [SerializeField, Header("無敵時間")]
    public float timeInvincible = 2.0f;
    [SerializeField, Header("弾のプレファブ")]
    public Projectile projectilePrefab;
    [SerializeField, Header("弾の速度")]
    public float force = 300.0f;
    [SerializeField, Header("発射エフェクト")]
    public ParticleSystem launchEffectPrefab;
    [SerializeField, Header("発射SE")]
    public AudioClip launchSE;
    [SerializeField, Header("被ダメージSE")]
    public AudioClip hitSE;
    [SerializeField, Header("移動時のSE")]
    public AudioClip runSE;

    public int Health { get {return currentHealth;}}  // プロパティ getのみする
    
    private int currentHealth;
    private bool isInvincible;
    private float invincibleTimer;

    Rigidbody2D rb;
    Animator anim;
    AudioSource audioSource;     // 外部から引数でSEをもらって再生するため

    Vector2 lookDirection = new Vector2(1, 0);   // キャラの向き

    private void Start() {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    /// <summary>
    /// アイテム取得時などに呼ばれる
    /// SEは引数で受け取る
    /// アイテム側で鳴らすとアイテム破壊時に再生が止まるため、プレイヤー側で鳴らす
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// 弾を生成する処理
    /// </summary>
    private void Launch() {
        Projectile projectile = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        // 発射する処理
        projectile.Launch(lookDirection, force);
        // アニメとSE、エフェクト再生
        anim.SetTrigger("Launch");
        PlaySound(launchSE);
        ParticleSystem launchEffect = Instantiate(launchEffectPrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        launchEffect.Play();
    }

    void Update() {        
        if (Input.GetButtonDown("Fire")) {
            // 弾の生成
            Launch();
        }

        if (Input.GetButtonDown("Action")) {
            // キャラの足元を始点にし、前方にNPCレイヤーのオブジェクトがあるか判定する
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.0f, LayerMask.GetMask("NPC"));
            if (hit.collider != null) {
                // NPCであるなら変数に格納しメソッドを呼び出す
                NonPlayerCharacter npc = hit.collider.gameObject.GetComponent<NonPlayerCharacter>();
                if (npc != null) {
                    npc.DisplayDialog();
                }
            }
        }

        if (isInvincible) {
            // 無敵フラグがオンなら無敵時間の処理を行う
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0) {
                isInvincible = false;
            }
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // キー入力を１つの変数に格納する
        Vector2 move = new Vector2(horizontal, vertical);

        // move.xとmove.yが0dではないか確認＝キー入力があるなら
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) {
            // どちらかが0ではない場合、移動しているとする
            // 視点の方向であるlookDirectionを更新し、正規化(0か1)する
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        // 移動方向に合わせてアニメを再生する
        // 再生時間をSpeedに送るmove.magnitudeで調整する
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", move.magnitude);

        Vector2 position = rb.position;
        position += move * speed * Time.deltaTime;
        rb.MovePosition(position);
    }

    /// <summary>
    /// ライフの増減処理
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount) {
        if (amount < 0) {
            // 引数がマイナスならダメージ処理
            if (isInvincible) {
                // すでに無敵状態なら何もしない
                return;
            }
            // SEとアニメ再生し、無敵状態にする
            PlaySound(hitSE);
            anim.SetTrigger("Hit");
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        // currentHralthの最低値と最大値を設定し、それ以上にもそれ以下にもならないように制限する
        currentHealth = Mathf.Clamp(currentHealth + amount,0,maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
}
