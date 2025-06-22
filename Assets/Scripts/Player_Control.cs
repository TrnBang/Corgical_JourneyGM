using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Control : MonoBehaviour
{
    public float moveDistance = 1f;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private LevelManager levelManager;

    private bool isRotating = false;
    private Quaternion targetRotation;
    public float rotationSpeed = 180f;

    private Quaternion originalRotation;
    private Stick stickCollision;
    void Start()
    {
        targetPosition = transform.position;
        stickCollision = GetComponentInChildren<Stick>();
    }

    void Update()
    {
        if (!isMoving && !isRotating)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                StartRotation(-90f); // quay trai
            }
            else if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartRotation(90f); // quay phai
            }

            Vector3 direction = Vector3.zero;

            if (Keyboard.current.wKey.wasPressedThisFrame)
                direction = Vector3.forward;
            else if (Keyboard.current.sKey.wasPressedThisFrame)
                direction = Vector3.back;
            else if (Keyboard.current.aKey.wasPressedThisFrame)
                direction = Vector3.left;
            else if (Keyboard.current.dKey.wasPressedThisFrame)
                direction = Vector3.right;

            if (direction != Vector3.zero)
            {
                Vector3 nextPosition = targetPosition + direction * moveDistance;

                if (IsGroundBelow(nextPosition))
                {
                    targetPosition = nextPosition;
                    StartCoroutine(MoveToPosition(targetPosition));
                }
            }
        }

        if (isRotating)
        {
            if (stickCollision != null && stickCollision.isColliding)
            {
            
                stickCollision.isColliding = false;


                transform.rotation = originalRotation;
                isRotating = false;
                Debug.Log("Bị vật cản - quay lại góc ban đầu");
                return;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public void SetLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    void StartRotation(float angle)
    {
        originalRotation = transform.rotation;
        isRotating = true;
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGrassStep();
    }
    bool IsGroundBelow(Vector3 position)
    {
        // Raycast từ trên xuống tại vị trí muốn đến, kiểm tra layer Ground
        Ray ray = new Ray(position + Vector3.up * 2f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        // Gọi âm thanh bước đi
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGrassStep();

        float elapsedTime = 0f;
        float moveTime = 0.2f;
        Vector3 startingPos = transform.position;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPos, target, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Victory"))
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayBark();

            Debug.Log("Victory!");
            if (levelManager != null)
            {
                StartCoroutine(DelayNextLevel());
            }
        }
    }

    System.Collections.IEnumerator DelayNextLevel()
    {
        yield return new WaitForSeconds(5f); // đợi 5 giây

        levelManager.LoadNextLevel();
    }

}
