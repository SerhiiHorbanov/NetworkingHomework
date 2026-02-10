using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewVisorsList", menuName = "ScriptableObjects/VisorsList")]
	public class VisorsListSO : ScriptableObject
	{
		[SerializeField] private VisorSO[] _Visors;
		
		public VisorSO GetByID(int id)
			=> _Visors[id];
	}
}
