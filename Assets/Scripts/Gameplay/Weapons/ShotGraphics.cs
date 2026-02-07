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
            float deltaTraceAlpha = Time.deltaTime / -_TraceFadeTime;
            _TraceSprite.color += new Color(0,0,0, deltaTraceAlpha);
            if (_TraceSprite.color.a <= 0) 
                _TraceSprite.enabled = false;
            
            float deltaDecalAlpha = Time.deltaTime / -_DecalFadeTime;
            _DecalSprite.color += new Color(0,0,0, deltaDecalAlpha);
            if (_DecalSprite.color.a <= 0) 
                _DecalSprite.enabled = false;

            if (!_TraceSprite.enabled && !_DecalSprite.enabled) 
                Destroy(gameObject);
        }
    }
}
