using Ayush;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WTF_GameJam
{
	public class ButtonExtended : Button
	{
		public override void OnPointerClick( PointerEventData eventData )
		{
			base.OnPointerClick( eventData );

			if(GameManager.Instance.TryGetService<AudioService>(out var audioService))
			{
				audioService.PlayButtonSound();
			}
		}
	}
}