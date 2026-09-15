using UnityEngine;

public class LandingZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        RocketController rocket = other.GetComponent<RocketController>();

        if (rocket != null)
        {
            rocket.TriggerWin();
        }
    }
}
