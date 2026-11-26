using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StyUI
{
    public enum ColorRandomizerTarget
    {
        TextMeshProUGUI,
        SpriteRenderer,
    }
    
    public enum ColorRandomizerType
    {
        LerpBetween,
        ChangeImmediately,
        FromList,
    }
    
    public class ColorRandomizer : MonoBehaviour
    {
        private Coroutine _lerpRoutine, _changeRoutine, _listRoutine;

        private bool _lerping;
        
        
        public TextMeshProUGUI targetTextMesh;
        public SpriteRenderer  targetSpriteRenderer;
        
        
        public ColorRandomizerTarget target      = ColorRandomizerTarget.TextMeshProUGUI;
        public ColorRandomizerType randomizeType = ColorRandomizerType.LerpBetween;
        public Color rangeMin                    = Color.black, rangeMax = Color.white;

        public List<Color> colors;
        
        
        public float lerpTime;
        public float delay = 0.1f; // 100 miliseconds


        private void Update()
        {
            if (randomizeType == ColorRandomizerType.LerpBetween)
                _lerpRoutine   ??= StartCoroutine(Lerp());
            
            else if (_changeRoutine != null)
                StopCoroutine(_lerpRoutine);
            
            
            if (randomizeType == ColorRandomizerType.ChangeImmediately)
                _changeRoutine ??= StartCoroutine(Change());
            
            else if (_changeRoutine != null)
                StopCoroutine(_changeRoutine);
            
            if (randomizeType == ColorRandomizerType.FromList)
                _listRoutine  ??= StartCoroutine(ListRandomizer());
            
            else if (_listRoutine != null)
                StopCoroutine(_listRoutine);
        }
        
        private IEnumerator Lerp()   
        {
            while (randomizeType == ColorRandomizerType.LerpBetween)
            {
                var targetColor = ColorUtility.Instance.RandomRange(rangeMin, rangeMax);
                
                switch (target)
                {
                    case ColorRandomizerTarget.TextMeshProUGUI:
                        ColorUtility.Instance.LerpInSeconds(targetTextMesh      , targetColor, lerpTime);
                        break;
                        
                    case ColorRandomizerTarget.SpriteRenderer:
                        ColorUtility.Instance.LerpInSeconds(targetSpriteRenderer, targetColor, lerpTime);
                        break;
                    
                    default:
                        Debug.LogWarning($"Unknown typeof target: {target}");
                        break;
                }
                
                yield return new WaitForSeconds(lerpTime);
                
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
            }
        }

        private IEnumerator Change() 
        {
            while (randomizeType == ColorRandomizerType.ChangeImmediately)
            {
                var targetColor = ColorUtility.Instance.RandomRange(rangeMin, rangeMax);

                switch (target)
                {
                    case ColorRandomizerTarget.TextMeshProUGUI:
                        targetTextMesh.color = targetColor;
                        break;
                    
                    case ColorRandomizerTarget.SpriteRenderer:
                        targetSpriteRenderer.color = targetColor;
                        break;
                    
                    default:
                        Debug.LogWarning($"Unknown typeof target: {target}");
                        break;
                }
                
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
            }
        }

        private IEnumerator ListRandomizer() 
        {
            if (colors.Count == 0) yield break;
            
            while (randomizeType == ColorRandomizerType.FromList)
            {
                var color = colors[Random.Range(0, colors.Count)];

                switch (target)
                {
                    case ColorRandomizerTarget.TextMeshProUGUI:
                        ColorUtility.Instance.LerpInSeconds(targetTextMesh, color, lerpTime);
                        break;
                    
                    case ColorRandomizerTarget.SpriteRenderer:
                        ColorUtility.Instance.LerpInSeconds(targetSpriteRenderer, color, lerpTime);
                        break;
                    
                    default:
                        print($"unknown typeof target: {target}");
                        break;
                }
                
                yield return new WaitForSeconds(lerpTime);
                
                if (delay > 0)
                    yield return new WaitForSeconds(delay);
            }
        }
    }
}