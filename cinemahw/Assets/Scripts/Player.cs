using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("クリア設定")]
    [SerializeField] private int targetItemCount = 5; 
    private int currentItemCount = 0;                  // 現在集めた個数

    [SerializeField] private GameObject clearUIPanel; // Step 1で作ったクリア画面UI

    private bool isCleared = false;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float turnSpeed = 720f;
    public Animator animator;

    Rigidbody rb;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isCleared && Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            Debug.Log("Kキーが押されました！");
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || ArduinoSerialPOC.GetButtonDown("JUMP")))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v);
        Vector3 moveDirection = inputDirection.normalized;

        if (Camera.main != null)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;
        }

        Vector3 move = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (moveDirection.sqrMagnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }

        if (move.magnitude > 0f)
        {
            animator.SetBool("IsWalking",true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

 
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Laser"))
        {
            Debug.Log("レーザーに触れた！ゲームオーバー！");
            GameOver();
        }

        if (other.CompareTag("Item"))
        {
            currentItemCount++; // カウントを1増やす
            Debug.Log("アイテム獲得！ 現在の個数: " + currentItemCount);

            // 触れた丸いオブジェクトを消す

            // 5個集まったかチェック
            if (currentItemCount >= targetItemCount)
            {
                GameClear();
            }
        }
    }

  
    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameClear()
    {
        Debug.Log("ゲームクリア！");

        isCleared = true;

        // 非表示にしていたクリア画面を表示する
        if (clearUIPanel != null)
        {
            clearUIPanel.SetActive(true);
        }

        // ゲームの時間を止める（プレイヤーやレーザーの動きがピタッと止まります）
        Time.timeScale = 0f;
    }




}
