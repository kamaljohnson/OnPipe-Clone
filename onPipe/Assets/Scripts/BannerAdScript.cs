using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdScript : MonoBehaviour {

    public string gameId = "3568970";
    public string placementId = "bottomBanner";
    public bool testMode = true;

    void Start () {
        Advertisement.Initialize (gameId, testMode);
        StartCoroutine (ShowBannerWhenReady ());
    }

    IEnumerator ShowBannerWhenReady () {
        while (!Advertisement.IsReady (placementId)) {
            Debug.Log("not ready");
            yield return new WaitForSeconds (0.5f);
        }
        Debug.Log("ready");
        Advertisement.Show();
    }
}