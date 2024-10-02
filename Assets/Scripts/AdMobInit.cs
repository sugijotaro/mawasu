using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobInit : MonoBehaviour
{
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob is initialized successfully.");
        });
    }
}