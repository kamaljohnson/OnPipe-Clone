using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class ChatController : MonoBehaviour {


    [FormerlySerializedAs("TMP_ChatInput")] public TMP_InputField tmpChatInput;

    [FormerlySerializedAs("TMP_ChatOutput")] public TMP_Text tmpChatOutput;

    [FormerlySerializedAs("ChatScrollbar")] public Scrollbar chatScrollbar;

    void OnEnable()
    {
        tmpChatInput.onSubmit.AddListener(AddToChatOutput);

    }

    void OnDisable()
    {
        tmpChatInput.onSubmit.RemoveListener(AddToChatOutput);

    }


    void AddToChatOutput(string newText)
    {
        // Clear Input Field
        tmpChatInput.text = string.Empty;

        var timeNow = System.DateTime.Now;

        tmpChatOutput.text += "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText + "\n";

        tmpChatInput.ActivateInputField();

        // Set the scrollbar to the bottom when next text is submitted.
        chatScrollbar.value = 0;

    }

}
