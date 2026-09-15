using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RocketController : MonoBehaviour
{
    public float thrustForce = 15f;
    public float moveForce = 8f;

    public float maxFuel = 100f;
    public float fuelConsumptionRate = 10f;
    private float currentFuel;

    public Slider fuelBar;

    public float gameOverDelay = 5f;
    private float noFuelTimer = 0f;
    private bool isGameOver = false;
    private bool isGameWon = false;

    public Transform savePoint;

    public TMP_Text countdownText;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentFuel = maxFuel;

        if (rb == null)
        {
            Debug.LogError("ไม่พบ Rigidbody บน " + gameObject.name);
        }

        UpdateFuelBar();

        if (countdownText != null)
        {
            countdownText.text = "";
        }
    }

    void Update()
    {
        if (Keyboard.current == null || rb == null) return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetRocket();
            return;
        }

        if (isGameOver || isGameWon) return;

        if (Keyboard.current.wKey.isPressed && currentFuel > 0f)
        {
            rb.AddForce(Vector3.up * thrustForce, ForceMode.Acceleration);

            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(currentFuel, 0f);

            UpdateFuelBar();
        }

        if (Keyboard.current.aKey.isPressed)
        {
            rb.AddForce(Vector3.left * moveForce, ForceMode.Acceleration);
        }

        if (Keyboard.current.dKey.isPressed)
        {
            rb.AddForce(Vector3.right * moveForce, ForceMode.Acceleration);
        }

        if (currentFuel <= 0f)
        {
            noFuelTimer += Time.deltaTime;

            float timeLeft = Mathf.Max(gameOverDelay - noFuelTimer, 0f);

            if (countdownText != null)
            {
                countdownText.text = timeLeft.ToString("F1");
            }

            if (noFuelTimer >= gameOverDelay)
            {
                TriggerGameOver();
            }
        }
        else
        {
            noFuelTimer = 0f;

            if (countdownText != null)
            {
                countdownText.text = "";
            }
        }
    }

    void UpdateFuelBar()
    {
        if (fuelBar != null)
        {
            fuelBar.value = currentFuel / maxFuel;
        }
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Min(currentFuel, maxFuel);

        UpdateFuelBar();

        Debug.Log("เก็บเหรียญ! น้ำมันเพิ่มเป็น: " + currentFuel.ToString("F1"));
    }

    public void TriggerWin()
    {
        if (isGameOver || isGameWon) return;

        isGameWon = true;
        Debug.Log("YOU WIN! กด R เพื่อเล่นใหม่");

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (countdownText != null)
        {
            countdownText.text = "You Win!\nPlay Again \"R\"";
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER - press R to restart");

        if (countdownText != null)
        {
            countdownText.text = "Game Over\nPlay Again \"R\"";
        }
    }

    void ResetRocket()
    {
        rb.isKinematic = false;

        if (savePoint != null)
        {
            transform.position = savePoint.position;
            transform.rotation = savePoint.rotation;
        }
        else
        {
            Debug.LogWarning("ยังไม่ได้กำหนด Save Point ใน Inspector");
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        currentFuel = maxFuel;
        noFuelTimer = 0f;
        isGameOver = false;
        isGameWon = false;

        UpdateFuelBar();

        if (countdownText != null)
        {
            countdownText.text = "";
        }

        // เรียกเหรียญทุกเหรียญในฉากกลับมาโชว์ใหม่ (รวมตัวที่ถูกซ่อนอยู่)
        CoinPickup[] allCoins = FindObjectsByType<CoinPickup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CoinPickup coin in allCoins)
        {
            coin.ResetCoin();
        }

        Debug.Log("รีเซ็ตจรวดและเหรียญแล้ว");
    }
}