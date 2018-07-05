using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRInteraction
{
	public interface IVRInput 
	{
		bool isSteamVR();
		GameObject getGameObject();
		string[] getVRActions{get; set;}

		int g_triggerKey {set;get;}
		int g_padTop{set;get;}
		int g_padLeft{set;get;}
		int g_padRight{set;get;}
		int g_padBottom{set;get;}
		int g_padCentre{set;get;}
		int g_padTouch{set;get;}
		int g_gripKey{set;get;}
		int g_menuKey{set;get;}
		int g_aButtonKey{set;get;}
		int g_triggerKeyOculus{set;get;}
		int g_padTopOculus{set;get;}
		int g_padLeftOculus{set;get;}
		int g_padRightOculus{set;get;}
		int g_padBottomOculus{set;get;}
		int g_padCentreOculus{set;get;}
		int g_padTouchOculus{set;get;}
		int g_gripKeyOculus{set;get;}
		int g_menuKeyOculus{set;get;}
		int g_aButtonKeyOculus{set;get;}
		Vector2 PadPosition{get;}
	}
}
