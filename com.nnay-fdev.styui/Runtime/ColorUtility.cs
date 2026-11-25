using TMPro;
using System;
using UnityEngine;
using System.Collections;


namespace StyUI
{
    public class ColorUtility : MonoBehaviour
    {
        public static ColorUtility Instance { get; private set; }

        public bool dontDestroyOnLoad = true;

        public Color minimumRandomRange = Color.black, maximumRandomRange = Color.white;

        
        private void Awake()                                                                
        {
            if (!dontDestroyOnLoad) return;

            if (!Instance)
            {
                Instance = this;
                
                DontDestroyOnLoad(this);
            }
            
            else 
                Destroy(gameObject);
        }

        public Color Random()                                                               
        {
            return RandomRange(minimumRandomRange, maximumRandomRange);
        }

        public Color RandomRange(Color rangeMin, Color rangeMax)                            
        {
            float 
                r = UnityEngine.Random.Range(rangeMin.r, rangeMax.r),
                g = UnityEngine.Random.Range(rangeMin.g, rangeMax.g),
                b = UnityEngine.Random.Range(rangeMin.b, rangeMax.b),
                a = UnityEngine.Random.Range(rangeMin.a, rangeMax.a);
            
            return new Color(r, g, b, a);
        }

        public Color Lerp(Color from             , Color to    , float time)                
        {
            return Color.Lerp(from, to, time);
        }
        public void  Lerp(TextMeshProUGUI text   , Color target, float time)                
        {
            text.color = Lerp(text.color, target, time);
        }
        public void  Lerp(SpriteRenderer renderer, Color target, float time)                
        {
            renderer.color = Lerp(renderer.color, target, time);
        }
        
        public void LerpInSeconds(Color from             , Color to    , float time, Action<Color> callback) 
        {
            StartCoroutine(_LerpInSeconds(from, to, time, callback));
        }
        public void LerpInSeconds(TextMeshProUGUI text   , Color target, float time)                         
        {
            StartCoroutine(_LerpInSeconds(text.color, target, time, col => text.color = col));
        }
        public void LerpInSeconds(SpriteRenderer renderer, Color target, float time)                         
        {
            StartCoroutine(_LerpInSeconds(renderer.color, target, time, col => renderer.color = col));
        }
        
        
        private IEnumerator _LerpInSeconds(Color from, Color to, float time, Action<Color> callback)         
            {
                var timer = 0.0f;
                var res   = from;
                
                while ((timer += Time.deltaTime) <= time)
                {
                    var prog = Mathf.Clamp01(timer / time);
                    
                    res = Lerp(res, to, prog);
                    callback?.Invoke(res);
    
                    yield return new WaitForEndOfFrame();
                }
            }
    }
}