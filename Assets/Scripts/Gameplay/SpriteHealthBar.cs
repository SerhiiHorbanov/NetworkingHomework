using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteHealthBar : MonoBehaviour
	{
		[SerializeField] private CharacterHealth _Health;

		private SpriteRenderer _sprite;
		
		[SerializeField] private float _FullWidth;

		private void Awake()
		{
			_sprite = GetComponent<SpriteRenderer>();
		}

		private void OnEnable()
		{
			print("Enabled");
			_Health.OnValueChanged += UpdateValue;
			UpdateValue(_Health.Hp);
		}
		
		private void OnDisable()
		{
			_Health.OnValueChanged -= UpdateValue;
		}

		private void UpdateValue(float oldHp, float newHp)
		{
			float x = _FullWidth * newHp / _Health._MaxHp;
			_sprite.size= new(x, _sprite.size.y);
		}
		
		private void UpdateValue(float hp) => UpdateValue(hp, hp);
	}
}
