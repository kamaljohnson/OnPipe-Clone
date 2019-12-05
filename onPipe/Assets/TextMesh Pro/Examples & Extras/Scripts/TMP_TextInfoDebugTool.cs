using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace TMPro.Examples
{

    public class TmpTextInfoDebugTool : MonoBehaviour
    {
        // Since this script is used for debugging, we exclude it from builds.
        // TODO: Rework this script to make it into an editor utility.
        #if UNITY_EDITOR
        [FormerlySerializedAs("ShowCharacters")] public bool showCharacters;
        [FormerlySerializedAs("ShowWords")] public bool showWords;
        [FormerlySerializedAs("ShowLinks")] public bool showLinks;
        [FormerlySerializedAs("ShowLines")] public bool showLines;
        [FormerlySerializedAs("ShowMeshBounds")] public bool showMeshBounds;
        [FormerlySerializedAs("ShowTextBounds")] public bool showTextBounds;
        [FormerlySerializedAs("ObjectStats")]
        [Space(10)]
        [TextArea(2, 2)]
        public string objectStats;

        [FormerlySerializedAs("m_TextComponent")] [SerializeField]
        private TMP_Text mTextComponent;

        [FormerlySerializedAs("m_Transform")] [SerializeField]
        private Transform mTransform;


        void OnDrawGizmos()
        {
            if (mTextComponent == null)
            {
                mTextComponent = gameObject.GetComponent<TMP_Text>();

                if (mTextComponent == null)
                    return;

                if (mTransform == null)
                    mTransform = gameObject.GetComponent<Transform>();
            }

            // Update Text Statistics
            TMP_TextInfo textInfo = mTextComponent.textInfo;

            objectStats = "Characters: " + textInfo.characterCount + "   Words: " + textInfo.wordCount + "   Spaces: " + textInfo.spaceCount + "   Sprites: " + textInfo.spriteCount + "   Links: " + textInfo.linkCount
                      + "\nLines: " + textInfo.lineCount + "   Pages: " + textInfo.pageCount;


            // Draw Quads around each of the Characters
            #region Draw Characters
            if (showCharacters)
                DrawCharactersBounds();
            #endregion


            // Draw Quads around each of the words
            #region Draw Words
            if (showWords)
                DrawWordBounds();
            #endregion


            // Draw Quads around each of the words
            #region Draw Links
            if (showLinks)
                DrawLinkBounds();
            #endregion


            // Draw Quads around each line
            #region Draw Lines
            if (showLines)
                DrawLineBounds();
            #endregion


            // Draw Quad around the bounds of the text
            #region Draw Bounds
            if (showMeshBounds)
                DrawBounds();
            #endregion

            // Draw Quad around the rendered region of the text.
            #region Draw Text Bounds
            if (showTextBounds)
                DrawTextBounds();
            #endregion
        }


        /// <summary>
        /// Method to draw a rectangle around each character.
        /// </summary>
        /// <param name="text"></param>
        void DrawCharactersBounds()
        {
            TMP_TextInfo textInfo = mTextComponent.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                // Draw visible as well as invisible characters
                TMP_CharacterInfo cInfo = textInfo.characterInfo[i];

                bool isCharacterVisible = i >= mTextComponent.maxVisibleCharacters ||
                                          cInfo.lineNumber >= mTextComponent.maxVisibleLines ||
                                          (mTextComponent.overflowMode == TextOverflowModes.Page && cInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                if (!isCharacterVisible) continue;

                // Get Bottom Left and Top Right position of the current character
                Vector3 bottomLeft = mTransform.TransformPoint(cInfo.bottomLeft);
                Vector3 topLeft = mTransform.TransformPoint(new Vector3(cInfo.topLeft.x, cInfo.topLeft.y, 0));
                Vector3 topRight = mTransform.TransformPoint(cInfo.topRight);
                Vector3 bottomRight = mTransform.TransformPoint(new Vector3(cInfo.bottomRight.x, cInfo.bottomRight.y, 0));

                Color color = cInfo.isVisible ? Color.yellow : Color.grey;
                DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, color);

                // Baseline
                Vector3 baselineStart = new Vector3(topLeft.x, mTransform.TransformPoint(new Vector3(0, cInfo.baseLine, 0)).y, 0);
                Vector3 baselineEnd = new Vector3(topRight.x, mTransform.TransformPoint(new Vector3(0, cInfo.baseLine, 0)).y, 0);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(baselineStart, baselineEnd);


                // Draw Ascender & Descender for each character.
                Vector3 ascenderStart = new Vector3(topLeft.x, mTransform.TransformPoint(new Vector3(0, cInfo.ascender, 0)).y, 0);
                Vector3 ascenderEnd = new Vector3(topRight.x, mTransform.TransformPoint(new Vector3(0, cInfo.ascender, 0)).y, 0);
                Vector3 descenderStart = new Vector3(bottomLeft.x, mTransform.TransformPoint(new Vector3(0, cInfo.descender, 0)).y, 0);
                Vector3 descenderEnd = new Vector3(bottomRight.x, mTransform.TransformPoint(new Vector3(0, cInfo.descender, 0)).y, 0);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(ascenderStart, ascenderEnd);
                Gizmos.DrawLine(descenderStart, descenderEnd);

                // Draw Cap Height
                float capHeight = cInfo.baseLine + cInfo.fontAsset.faceInfo.capLine * cInfo.scale;
                Vector3 capHeightStart = new Vector3(topLeft.x, mTransform.TransformPoint(new Vector3(0, capHeight, 0)).y, 0);
                Vector3 capHeightEnd = new Vector3(topRight.x, mTransform.TransformPoint(new Vector3(0, capHeight, 0)).y, 0);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(capHeightStart, capHeightEnd);

                // Draw Centerline
                float meanline = cInfo.baseLine + cInfo.fontAsset.faceInfo.meanLine * cInfo.scale;
                Vector3 centerlineStart = new Vector3(topLeft.x, mTransform.TransformPoint(new Vector3(0, meanline, 0)).y, 0);
                Vector3 centerlineEnd = new Vector3(topRight.x, mTransform.TransformPoint(new Vector3(0, meanline, 0)).y, 0);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(centerlineStart, centerlineEnd);

                // Draw Origin for each character.
                float gizmoSize = (ascenderEnd.y - descenderEnd.y) * 0.02f;
                Vector3 origin = mTransform.TransformPoint(cInfo.origin, cInfo.baseLine, 0);
                Vector3 originBl = new Vector3(origin.x - gizmoSize, origin.y - gizmoSize, 0);
                Vector3 originTl = new Vector3(originBl.x, origin.y + gizmoSize, 0);
                Vector3 originTr = new Vector3(origin.x + gizmoSize, originTl.y, 0);
                Vector3 originBr = new Vector3(originTr.x, originBl.y, 0);

                Gizmos.color = new Color(1, 0.5f, 0);
                Gizmos.DrawLine(originBl, originTl);
                Gizmos.DrawLine(originTl, originTr);
                Gizmos.DrawLine(originTr, originBr);
                Gizmos.DrawLine(originBr, originBl);

                // Draw xAdvance for each character.
                gizmoSize = (ascenderEnd.y - descenderEnd.y) * 0.04f;
                float xAdvance = mTransform.TransformPoint(cInfo.xAdvance, 0, 0).x;
                Vector3 topAdvance = new Vector3(xAdvance, baselineStart.y + gizmoSize, 0);
                Vector3 bottomAdvance = new Vector3(xAdvance, baselineStart.y - gizmoSize, 0);
                Vector3 leftAdvance = new Vector3(xAdvance - gizmoSize, baselineStart.y, 0);
                Vector3 rightAdvance = new Vector3(xAdvance + gizmoSize, baselineStart.y, 0);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(topAdvance, bottomAdvance);
                Gizmos.DrawLine(leftAdvance, rightAdvance);
            }
        }


        /// <summary>
        /// Method to draw rectangles around each word of the text.
        /// </summary>
        /// <param name="text"></param>
        void DrawWordBounds()
        {
            TMP_TextInfo textInfo = mTextComponent.textInfo;

            for (int i = 0; i < textInfo.wordCount; i++)
            {
                TMP_WordInfo wInfo = textInfo.wordInfo[i];

                bool isBeginRegion = false;

                Vector3 bottomLeft = Vector3.zero;
                Vector3 topLeft = Vector3.zero;
                Vector3 bottomRight = Vector3.zero;
                Vector3 topRight = Vector3.zero;

                float maxAscender = -Mathf.Infinity;
                float minDescender = Mathf.Infinity;

                Color wordColor = Color.green;

                // Iterate through each character of the word
                for (int j = 0; j < wInfo.characterCount; j++)
                {
                    int characterIndex = wInfo.firstCharacterIndex + j;
                    TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[characterIndex];
                    int currentLine = currentCharInfo.lineNumber;

                    bool isCharacterVisible = characterIndex > mTextComponent.maxVisibleCharacters ||
                                              currentCharInfo.lineNumber > mTextComponent.maxVisibleLines ||
                                             (mTextComponent.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                    // Track Max Ascender and Min Descender
                    maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
                    minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

                    if (isBeginRegion == false && isCharacterVisible)
                    {
                        isBeginRegion = true;

                        bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                        topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                        //Debug.Log("Start Word Region at [" + currentCharInfo.character + "]");

                        // If Word is one character
                        if (wInfo.characterCount == 1)
                        {
                            isBeginRegion = false;

                            topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                            bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                            bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                            topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                            // Draw Region
                            DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);

                            //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                        }
                    }

                    // Last Character of Word
                    if (isBeginRegion && j == wInfo.characterCount - 1)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);

                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    }
                    // If Word is split on more than one line.
                    else if (isBeginRegion && currentLine != textInfo.characterInfo[characterIndex + 1].lineNumber)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);
                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                        maxAscender = -Mathf.Infinity;
                        minDescender = Mathf.Infinity;

                    }
                }

                //Debug.Log(wInfo.GetWord(m_TextMeshPro.textInfo.characterInfo));
            }


        }


        /// <summary>
        /// Draw rectangle around each of the links contained in the text.
        /// </summary>
        /// <param name="text"></param>
        void DrawLinkBounds()
        {
            TMP_TextInfo textInfo = mTextComponent.textInfo;

            for (int i = 0; i < textInfo.linkCount; i++)
            {
                TMP_LinkInfo linkInfo = textInfo.linkInfo[i];

                bool isBeginRegion = false;

                Vector3 bottomLeft = Vector3.zero;
                Vector3 topLeft = Vector3.zero;
                Vector3 bottomRight = Vector3.zero;
                Vector3 topRight = Vector3.zero;

                float maxAscender = -Mathf.Infinity;
                float minDescender = Mathf.Infinity;

                Color32 linkColor = Color.cyan;

                // Iterate through each character of the link text
                for (int j = 0; j < linkInfo.linkTextLength; j++)
                {
                    int characterIndex = linkInfo.linkTextfirstCharacterIndex + j;
                    TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[characterIndex];
                    int currentLine = currentCharInfo.lineNumber;

                    bool isCharacterVisible = characterIndex > mTextComponent.maxVisibleCharacters ||
                                              currentCharInfo.lineNumber > mTextComponent.maxVisibleLines ||
                                             (mTextComponent.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                    // Track Max Ascender and Min Descender
                    maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
                    minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

                    if (isBeginRegion == false && isCharacterVisible)
                    {
                        isBeginRegion = true;

                        bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                        topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                        //Debug.Log("Start Word Region at [" + currentCharInfo.character + "]");

                        // If Link is one character
                        if (linkInfo.linkTextLength == 1)
                        {
                            isBeginRegion = false;

                            topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                            bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                            bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                            topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                            // Draw Region
                            DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, linkColor);

                            //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                        }
                    }

                    // Last Character of Link
                    if (isBeginRegion && j == linkInfo.linkTextLength - 1)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, linkColor);

                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    }
                    // If Link is split on more than one line.
                    else if (isBeginRegion && currentLine != textInfo.characterInfo[characterIndex + 1].lineNumber)
                    {
                        isBeginRegion = false;

                        topLeft = mTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = mTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = mTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, linkColor);

                        maxAscender = -Mathf.Infinity;
                        minDescender = Mathf.Infinity;
                        //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    }
                }

                //Debug.Log(wInfo.GetWord(m_TextMeshPro.textInfo.characterInfo));
            }
        }


        /// <summary>
        /// Draw Rectangles around each lines of the text.
        /// </summary>
        /// <param name="text"></param>
        void DrawLineBounds()
        {
            TMP_TextInfo textInfo = mTextComponent.textInfo;

            for (int i = 0; i < textInfo.lineCount; i++)
            {
                TMP_LineInfo lineInfo = textInfo.lineInfo[i];

                bool isLineVisible = (lineInfo.characterCount == 1 && textInfo.characterInfo[lineInfo.firstCharacterIndex].character == 10) ||
                                      i > mTextComponent.maxVisibleLines ||
                                     (mTextComponent.overflowMode == TextOverflowModes.Page && textInfo.characterInfo[lineInfo.firstCharacterIndex].pageNumber + 1 != mTextComponent.pageToDisplay) ? false : true;

                if (!isLineVisible) continue;

                //if (!ShowLinesOnlyVisibleCharacters)
                //{
                // Get Bottom Left and Top Right position of each line
                float ascender = lineInfo.ascender;
                float descender = lineInfo.descender;
                float baseline = lineInfo.baseline;
                float maxAdvance = lineInfo.maxAdvance;
                Vector3 bottomLeft = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].bottomLeft.x, descender, 0));
                Vector3 topLeft = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].bottomLeft.x, ascender, 0));
                Vector3 topRight = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.lastCharacterIndex].topRight.x, ascender, 0));
                Vector3 bottomRight = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.lastCharacterIndex].topRight.x, descender, 0));

                DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, Color.green);

                Vector3 bottomOrigin = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].origin, descender, 0));
                Vector3 topOrigin = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].origin, ascender, 0));
                Vector3 bottomAdvance = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].origin + maxAdvance, descender, 0));
                Vector3 topAdvance = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].origin + maxAdvance, ascender, 0));

                DrawDottedRectangle(bottomOrigin, topOrigin, topAdvance, bottomAdvance, Color.green);

                Vector3 baselineStart = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstCharacterIndex].bottomLeft.x, baseline, 0));
                Vector3 baselineEnd = mTransform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.lastCharacterIndex].topRight.x, baseline, 0));

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(baselineStart, baselineEnd);

                // Draw LineExtents
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(mTransform.TransformPoint(lineInfo.lineExtents.min), mTransform.TransformPoint(lineInfo.lineExtents.max));

                //}
                //else
                //{
                //// Get Bottom Left and Top Right position of each line
                //float ascender = lineInfo.ascender;
                //float descender = lineInfo.descender;
                //Vector3 bottomLeft = m_Transform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstVisibleCharacterIndex].bottomLeft.x, descender, 0));
                //Vector3 topLeft = m_Transform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstVisibleCharacterIndex].bottomLeft.x, ascender, 0));
                //Vector3 topRight = m_Transform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.lastVisibleCharacterIndex].topRight.x, ascender, 0));
                //Vector3 bottomRight = m_Transform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.lastVisibleCharacterIndex].topRight.x, descender, 0));

                //DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, Color.green);

                //Vector3 baselineStart = m_Transform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.firstVisibleCharacterIndex].bottomLeft.x, textInfo.characterInfo[lineInfo.firstVisibleCharacterIndex].baseLine, 0));
                //Vector3 baselineEnd = m_Transform.TransformPoint(new Vector3(textInfo.characterInfo[lineInfo.lastVisibleCharacterIndex].topRight.x, textInfo.characterInfo[lineInfo.lastVisibleCharacterIndex].baseLine, 0));

                //Gizmos.color = Color.cyan;
                //Gizmos.DrawLine(baselineStart, baselineEnd);
                //}
            }
        }

        /// <summary>
        /// Draw Rectangle around the bounds of the text object.
        /// </summary>
        void DrawBounds()
        {
            Bounds meshBounds = mTextComponent.bounds;
            
            // Get Bottom Left and Top Right position of each word
            Vector3 bottomLeft = mTextComponent.transform.position + (meshBounds.center - meshBounds.extents);
            Vector3 topRight = mTextComponent.transform.position + (meshBounds.center + meshBounds.extents);

            DrawRectangle(bottomLeft, topRight, new Color(1, 0.5f, 0));
        }


        void DrawTextBounds()
        {
            Bounds textBounds = mTextComponent.textBounds;

            Vector3 bottomLeft = mTextComponent.transform.position + (textBounds.center - textBounds.extents);
            Vector3 topRight = mTextComponent.transform.position + (textBounds.center + textBounds.extents);

            DrawRectangle(bottomLeft, topRight, new Color(0f, 0.5f, 0.5f));
        }


        // Draw Rectangles
        void DrawRectangle(Vector3 bl, Vector3 tr, Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(new Vector3(bl.x, bl.y, 0), new Vector3(bl.x, tr.y, 0));
            Gizmos.DrawLine(new Vector3(bl.x, tr.y, 0), new Vector3(tr.x, tr.y, 0));
            Gizmos.DrawLine(new Vector3(tr.x, tr.y, 0), new Vector3(tr.x, bl.y, 0));
            Gizmos.DrawLine(new Vector3(tr.x, bl.y, 0), new Vector3(bl.x, bl.y, 0));
        }


        // Draw Rectangles
        void DrawRectangle(Vector3 bl, Vector3 tl, Vector3 tr, Vector3 br, Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(bl, tl);
            Gizmos.DrawLine(tl, tr);
            Gizmos.DrawLine(tr, br);
            Gizmos.DrawLine(br, bl);
        }


        // Draw Rectangles
        void DrawDottedRectangle(Vector3 bl, Vector3 tl, Vector3 tr, Vector3 br, Color color)
        {
            var cam = Camera.current;
            float dotSpacing = (cam.WorldToScreenPoint(br).x - cam.WorldToScreenPoint(bl).x) / 75f;
            UnityEditor.Handles.color = color;

            UnityEditor.Handles.DrawDottedLine(bl, tl, dotSpacing);
            UnityEditor.Handles.DrawDottedLine(tl, tr, dotSpacing);
            UnityEditor.Handles.DrawDottedLine(tr, br, dotSpacing);
            UnityEditor.Handles.DrawDottedLine(br, bl, dotSpacing);
        }

#endif
    }
}

