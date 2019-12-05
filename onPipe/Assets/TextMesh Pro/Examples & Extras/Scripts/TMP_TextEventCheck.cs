﻿using UnityEngine;
using UnityEngine.Serialization;


namespace TMPro.Examples
{
    public class TmpTextEventCheck : MonoBehaviour
    {

        [FormerlySerializedAs("TextEventHandler")] public TmpTextEventHandler textEventHandler;

        void OnEnable()
        {
            if (textEventHandler != null)
            {
                textEventHandler.OnCharacterSelection.AddListener(OnCharacterSelection);
                textEventHandler.OnSpriteSelection.AddListener(OnSpriteSelection);
                textEventHandler.OnWordSelection.AddListener(OnWordSelection);
                textEventHandler.OnLineSelection.AddListener(OnLineSelection);
                textEventHandler.OnLinkSelection.AddListener(OnLinkSelection);
            }
        }


        void OnDisable()
        {
            if (textEventHandler != null)
            {
                textEventHandler.OnCharacterSelection.RemoveListener(OnCharacterSelection);
                textEventHandler.OnSpriteSelection.RemoveListener(OnSpriteSelection);
                textEventHandler.OnWordSelection.RemoveListener(OnWordSelection);
                textEventHandler.OnLineSelection.RemoveListener(OnLineSelection);
                textEventHandler.OnLinkSelection.RemoveListener(OnLinkSelection);
            }
        }


        void OnCharacterSelection(char c, int index)
        {
            Debug.Log("Character [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnSpriteSelection(char c, int index)
        {
            Debug.Log("Sprite [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnWordSelection(string word, int firstCharacterIndex, int length)
        {
            Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLineSelection(string lineText, int firstCharacterIndex, int length)
        {
            Debug.Log("Line [" + lineText + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLinkSelection(string linkId, string linkText, int linkIndex)
        {
            Debug.Log("Link Index: " + linkIndex + " with ID [" + linkId + "] and Text \"" + linkText + "\" has been selected.");
        }

    }
}