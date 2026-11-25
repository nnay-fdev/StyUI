using UnityEngine;
using System.Collections;


namespace StyUI
{
    public enum ShakeAxis
    {
        [InspectorName("X Axis")]    X,
        [InspectorName("Y Axis")]    Y,
        [InspectorName("Both Axes")] Both
    }
    
    public class ShakeEffect : MonoBehaviour
    {
        public Transform     targetTransform;
        public RectTransform targetRectTransform;
        
        
        public ShakeAxis axis = ShakeAxis.Both;
        
        public float shakeBoundsX;
        public float shakeBoundsY;

        public float shakeDelay;

        
        private void Start()
        {
            targetTransform     ??= transform;
            targetRectTransform ??= targetTransform.GetComponent<RectTransform>();

            StartCoroutine(Shake());
        }

        private IEnumerator Shake()
        {
            var startPosition = Vector2.zero;
            
            if      (targetTransform)     startPosition = targetTransform.localPosition;
            else if (targetRectTransform) startPosition = targetRectTransform.anchoredPosition;
            
            while (true)
            {
                switch (axis)
                {
                    case ShakeAxis.X:
                    {
                        var randX = Random.Range(-shakeBoundsX, shakeBoundsX);

                        if (targetTransform)
                            targetTransform.localPosition = new Vector3(startPosition.x + randX, startPosition.y);
                        
                        else if (targetRectTransform)
                            targetRectTransform.anchoredPosition = new Vector2(startPosition.x +  randX, startPosition.y);
                        
                        break;
                    }
                    
                    case ShakeAxis.Y:
                    {
                        var randY = Random.Range(-shakeBoundsY, shakeBoundsY);

                        if (targetTransform)
                            targetTransform.localPosition = new Vector3(startPosition.x, startPosition.y + randY);
                        
                        else if (targetRectTransform)
                            targetRectTransform.anchoredPosition = new Vector2(startPosition.x, startPosition.y + randY);
                        
                        break;
                    }
                    
                    case ShakeAxis.Both:
                    {
                        var randX = Random.Range(-shakeBoundsX, shakeBoundsX);
                        var randY = Random.Range(-shakeBoundsY, shakeBoundsY);

                        if (targetTransform)
                            targetTransform.localPosition = new Vector3(startPosition.x + randX, startPosition.y + randY);
                        
                        else if (targetRectTransform)
                            targetRectTransform.anchoredPosition = new Vector2(startPosition.x + randX,  startPosition.y + randY);
                        
                        break;
                    }
                }

                if (shakeDelay > 0)
                    yield return new WaitForSeconds(shakeDelay);
            }    
        }
    }
}