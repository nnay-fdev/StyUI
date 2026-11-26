using System;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StyUI
{
    public enum WriteTextMode
    {
        SingleObject,
        BreakToChars,
        BreakToList
    }
    
    public class TypewriterEffect : MonoBehaviour
    {
        public WriteTextMode   mode = WriteTextMode.SingleObject;

        [Multiline]
        public string text;
        public List<string> texts;

        public TMP_FontAsset font;
        public Color         color;
        public float         fontSize = 36;
        
        public float charDelay;
        public float wordDelay;

        public float spaceWidth;

        public  Vector2 position;
        public  Vector2 scale;
        public  Vector2 anchorMin, anchorMax;
        
        public event Action<GameObject> OnCharWrite;


        private void Start()
        {
            StartCoroutine(mode == WriteTextMode.BreakToList ? WriteListRoutine() : WriteTextRoutine());
        }

        // WriteTextMode.SingleObject | WriteTextMode.BreakToChars
        private IEnumerator WriteTextRoutine() 
        {
            switch (mode)
            {
                case WriteTextMode.BreakToList:
                    yield break;
                
                case WriteTextMode.SingleObject:
                {
                    var tObj  = new GameObject("Typewriter-TextObject", typeof(RectTransform), typeof(TextMeshProUGUI));

                    var tRect = tObj.transform as RectTransform;
                    var tMesh = tObj.GetComponent<TextMeshProUGUI>();

                    tMesh.font     = font;
                    tMesh.color    = color;
                    tMesh.fontSize = fontSize;
                    
                    tRect.SetParent(transform, false);
                    
                    tRect.sizeDelta = scale;
                    
                    tRect.anchorMin = anchorMin;
                    tRect.anchorMax = anchorMax;

                    tRect.anchoredPosition = position;


                    foreach (var chr in text)
                    {
                        tMesh.text += chr;
                    
                        OnCharWrite?.Invoke(tObj);
                        if (charDelay > 0)
                            yield return new WaitForSeconds(charDelay);
                    }

                    break;
                }

                case WriteTextMode.BreakToChars:
                {
                    var nextPosition = position;
                    for (var index = 0; index < text.Length; index++)
                    {
                        var chr = text[index];
                        
                        var tObj = new GameObject("Typewriter-TextObject", typeof(RectTransform),
                            typeof(TextMeshProUGUI));

                        var tRect = tObj.transform as RectTransform;
                        var tMesh = tObj.GetComponent<TextMeshProUGUI>();

                        tMesh.font     = font;
                        tMesh.color    = color;
                        tMesh.fontSize = fontSize;
                        
                        if (chr == '\\')
                        {
                            // get next char
                            chr = text[++index];

                            // if it's a newline
                            if (chr is 'n' or 'N')
                            {
                                nextPosition.x = position.x;
                                nextPosition.y -= fontSize;
                                
                                // no need tObj
                                Destroy(tObj);
                                
                                continue;
                            }
                        }
                        
                        else 
                            tMesh.text = chr.ToString();
                        
                        tRect.SetParent(transform, false);

                        tRect.anchorMin = anchorMin;
                        tRect.anchorMax = anchorMax;

                        tRect.anchoredPosition = nextPosition;
                        
                        tMesh.ForceMeshUpdate();

                        if (chr == ' ')
                        {
                            nextPosition.x += spaceWidth;
                            
                            // don't need tObj
                            Destroy(tObj);
                            continue;
                        }
                        
                        nextPosition.x += tMesh.preferredWidth;

                        OnCharWrite?.Invoke(tObj);
                        if (charDelay > 0)
                            yield return new WaitForSeconds(charDelay);
                    }

                    break;
                }
            }
        }
        
        // WriteTextMode.BreakToList
        // Creates GameObject for each List entry
        private IEnumerator WriteListRoutine() 
        {
            if (mode != WriteTextMode.BreakToList) yield break;

            var nextPosition = position;
            foreach (var str in texts)
            {
                var tObj = new GameObject("Typewriter-TextObject", typeof(RectTransform), typeof(TextMeshProUGUI));
                
                var tRect = tObj.transform as RectTransform;
                var tMesh = tObj.GetComponent<TextMeshProUGUI>();
                
                tRect.SetParent(transform, false);
                
                tRect.anchorMin = anchorMin;
                tRect.anchorMax = anchorMax;
                
                tRect.anchoredPosition = nextPosition;
                
                tMesh.font     = font;
                tMesh.color    = color;
                tMesh.fontSize = fontSize;

                foreach (var chr in str)
                {
                    tMesh.text += chr;
                    
                    OnCharWrite?.Invoke(tObj);
                    if (charDelay > 0)
                        yield return new WaitForSeconds(charDelay);
                }
                
                nextPosition.x += tMesh.preferredWidth;
                yield return new WaitForSeconds(wordDelay);
            }
        }
    }
}