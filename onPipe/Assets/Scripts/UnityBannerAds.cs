using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityBannerAds : MonoBehaviour {

    public string bannerPlacement = "bottomBanner";
    public bool testMode = true;

#if UNITY_IOS
    public const string gameID = "1234567";
#elif UNITY_ANDROID
    public const string gameID = "3568970";
#elif UNITY_EDITOR
    public const string gameID = "1111111";
#endif

    void Start () {
        Advertisement.Initialize (gameID, testMode);
        StartCoroutine (ShowBannerWhenReady ());
    }

    IEnumerator ShowBannerWhenReady () {
        while (!Advertisement.IsReady (bannerPlacement)) {
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show (bannerPlacement);
    }
}