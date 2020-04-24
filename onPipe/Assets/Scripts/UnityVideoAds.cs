using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Monetization;

public class UnityVideoAds : MonoBehaviour {

    public string placementId = "video";

    public string gameId = "3568970";
    public bool testMode = true;

    public void Start()
    {
        Monetization.Initialize(gameId, testMode);
        ShowAd();
    }

    public void ShowAd () {
        StartCoroutine (ShowAdWhenReady ());
    }

    private IEnumerator ShowAdWhenReady () {
        while (!Monetization.IsReady (placementId)) {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent (placementId) as ShowAdPlacementContent;

        if(ad != null) {
            ad.Show ();
        }
    }
}