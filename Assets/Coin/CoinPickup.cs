using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public float fuelAmount = 20f;

    void OnTriggerEnter(Collider other)
    {
        RocketController rocket = other.GetComponent<RocketController>();

        if (rocket != null)
        {
            rocket.AddFuel(fuelAmount);
            gameObject.SetActive(false); // ซ่อนแทนการลบทิ้ง เพื่อให้กลับมาใหม่ได้ตอนกด R
        }
    }

    public void ResetCoin()
    {
        gameObject.SetActive(true); // เปิดกลับมาให้โชว์อีกครั้ง
    }
}

