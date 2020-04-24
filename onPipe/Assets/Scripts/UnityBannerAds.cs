using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;

public class UnityBannerAds : MonoBehaviour {

    public string bannerPlacement = "bottomBanner";
    public bool testMode = true;

    public string gameId = "3568970";

    void Start () {
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerPlacement);
        StartCoroutine (ShowBannerWhenReady ());
    }

    IEnumerator ShowBannerWhenReady () {
        while (!Advertisement.IsReady (bannerPlacement)) {
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Banner.Show (bannerPlacement);
    }
}