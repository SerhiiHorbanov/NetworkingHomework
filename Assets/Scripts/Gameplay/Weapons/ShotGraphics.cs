using UnityEngine;

namespace Gameplay.Weapons
{ 
    class ShotGraphics : MonoBehaviour
    {
        [SerializeField] private Transform _Trace;
        [SerializeField] private SpriteRenderer _TraceSprite;
        
        [SerializeField] private Transform _Decal;
        [SerializeField] private SpriteRenderer _DecalSprite;

        [SerializeField] private float _TraceFadeTime;
        [SerializeField] private float _DecalFadeTime;
        
        private const float DecalDistanceFromHit = 0.005f;
        
        public void Initialize(ShotGraphicsInfo shot)
        {
            _Trace.position = (shot.Origin + shot.Hit) * 0.5f;
            _Trace.LookAt(shot.Hit);
            
            float traceLength = Vector3.Distance(shot.Hit, shot.Origin);
            _Trace.localScale = new(1, 1, traceLength);

            if (shot.DidHit)
            {
                _Decal.position = shot.Hit + (shot.HitNormal * DecalDistanceFromHit);
                _Decal.forward = shot.HitNormal;
            }
        }

        public void Update()
        {
            FadeAndTryDisableSprite(_TraceSprite);
            FadeAndTryDisableSprite(_DecalSprite);

            if (!_TraceSprite.enabled && !_DecalSprite.enabled) 
                Destroy(gameObject);
            
            return;

            void FadeAndTryDisableSprite(SpriteRenderer sprite)
            {
                float deltaAlpha = Time.deltaTime / -_DecalFadeTime;
                sprite.color += new Color(0,0,0, deltaAlpha);
                if (sprite.color.a <= 0) 
                    sprite.enabled = false;
            }
        }
    }
}
