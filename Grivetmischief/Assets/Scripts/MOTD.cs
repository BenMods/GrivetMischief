using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class MOTD : MonoBehaviour
{
    [Header("URL of your raw Pastebin or text file")]
    public string motdURL = "https://pastebin.com/raw/YOUR_CODE_HERE";

    [Header("TMP Text to display the MOTD")]
    public TMP_Text motdText;

    void Start()
    {
        StartCoroutine(FetchMOTD());
    }

    IEnumerator FetchMOTD()
    {
        UnityWebRequest www = UnityWebRequest.Get(motdURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch MOTD: " + www.error);
            motdText.text = "Failed to load MOTD.";
        }
        else
        {
            motdText.text = www.downloadHandler.text;
        }
    }
}
