using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRInteraction
{
	public delegate void OculusClickedEventHandler(object sender);

	public class VRInputOculus : MonoBehaviour, IVRInput
	{
		public OVRInput.Controller controllerHand;
		public string[] VRActions;
		public bool debugMode;

		public int triggerKey;
		public int stickTop;
		public int stickLeft;
		public int stickRight;
		public int stickBottom;
		public int stickCentre;
		public int stickTouch;
		public int gripKey;
		public int BYKey;
		public int AXKey;

		public bool triggerPressed = false;
		public bool padPressed = false;
		public bool padTouched = false;
		public bool gripped = false;
		public bool BY_Pressed = false;
		public bool AX_Pressed = false;

		public event OculusClickedEventHandler d_TriggerClicked;
		public event OculusClickedEventHandler d_TriggerUnclicked;
		public event OculusClickedEventHandler d_ThumbStickClicked;
		public event OculusClickedEventHandler d_ThumbStickUnclicked;
		public event OculusClickedEventHandler d_ThumbStickTouched;
		public event OculusClickedEventHandler d_ThumbStickUntouched;
		public event OculusClickedEventHandler d_Gripped;
		public event OculusClickedEventHandler d_Ungripped;
		public event OculusClickedEventHandler d_BY_ButtonClicked;
		public event OculusClickedEventHandler d_BY_ButtonUnclicked;
		public event OculusClickedEventHandler d_AX_ButtonClicked;
		public event OculusClickedEventHandler d_AX_ButtonUnclicked;

		public GameObject getGameObject()
		{
			return gameObject;
		}
		public bool isSteamVR()
		{
			return false;
		}
		public string[] getVRActions
		{
			get { return VRActions; }
			set { VRActions = value; }
		}
		public int g_triggerKey{set{}get{return 0;}}
		public int g_padTop{set{}get{return 0;}}
		public int g_padLeft{set{}get{return 0;}}
		public int g_padRight{set{}get{return 0;}}
		public int g_padBottom{set{}get{return 0;}}
		public int g_padCentre{set{}get{return 0;}}
		public int g_padTouch{set{}get{return 0;}}
		public int g_gripKey{set{}get{return 0;}}
		public int g_menuKey{set{}get{return 0;}}
		public int g_aButtonKey{set{}get{return 0;}}
		public int g_triggerKeyOculus{set{triggerKey = value;}get{return triggerKey;}}
		public int g_padTopOculus{set{stickTop = value;}get{return stickTop;}}
		public int g_padLeftOculus{set{stickLeft = value;}get{return stickLeft;}}
		public int g_padRightOculus{set{stickRight = value;}get{return stickRight;}}
		public int g_padBottomOculus{set{stickBottom = value;}get{return stickBottom;}}
		public int g_padCentreOculus{set{ stickCentre = value;}get{return stickCentre;}}
		public int g_padTouchOculus{set{ stickTouch = value;}get{return stickTouch;}}
		public int g_gripKeyOculus{set{ gripKey = value;}get{return gripKey;}}
		public int g_menuKeyOculus{set{ BYKey = value; }get{return BYKey;}}
		public int g_aButtonKeyOculus{set{ AXKey = value;}get{return AXKey;}}

		public bool ActionPressed(string action)
		{
			for(int i=0; i<VRActions.Length; i++)
			{
				if (action == VRActions[i])
				{
					return ActionPressed(i);
				}
			}
			if (debugMode)
			{
				string debugString = "No action with the name " + action + ". Current options are ";
				foreach(string availableOption in VRActions)
				{
					debugString += availableOption + " ";
				}
				Debug.LogWarning(debugString);
			}
			return false;
		}

		public bool ActionPressed(int action)
		{
			if (triggerKey == action && TriggerPressure > 0.9f)
				return true;
			if (stickTop == action && PadUpPressed)
				return true;
			if (stickLeft == action && PadLeftPressed)
				return true;
			if (stickRight == action && PadRightPressed)
				return true;
			if (stickBottom == action && PadDownPressed)
				return true;
			if (stickCentre == action && PadCentrePressed)
				return true;
			if (stickTouch == action && PadTouched)
				return true;
			if (gripKey == action && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controllerHand) > 0.9f)
				return true;
			if (BYKey == action && OVRInput.Get(OVRInput.Button.Two, controllerHand))
				return true;
			if (AXKey == action && OVRInput.Get(OVRInput.Button.Three, controllerHand))
				return true;
			return false;
		}

		public float TriggerPressure
		{
			get
			{
				return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerHand);
			}
		}
		public bool PadUpPressed
		{
			get
			{
				
				if (OVRInput.Get(OVRInput.Button.DpadDown, controllerHand))
				{
					Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerHand);
					if (axis.y > 0.4f &&
						axis.x < axis.y &&
						axis.x > -axis.y)
						return true;
				}
				return false;
			}
		}
		public bool PadLeftPressed
		{
			get
			{
				if (OVRInput.Get(OVRInput.Button.DpadDown, controllerHand))
				{
					Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerHand);
					if (axis.x < -0.4f &&
						axis.y > axis.x &&
						axis.y < -axis.x)
						return true;
				}
				return false;
			}
		}
		public bool PadRightPressed
		{
			get
			{
				if (OVRInput.Get(OVRInput.Button.DpadDown, controllerHand))
				{
					Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerHand);
					if (axis.x > 0.4f &&
						axis.y < axis.x &&
						axis.y > -axis.x)
						return true;
				}
				return false;
			}
		}
		public bool PadDownPressed
		{
			get
			{
				if (OVRInput.Get(OVRInput.Button.DpadDown, controllerHand))
				{
					Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerHand);
					if ((axis.y < -0.4f &&
						axis.x > axis.y &&
						axis.x < -axis.y) ||
						axis == Vector2.zero)
						return true;
				}
				return false;
			}
		}
		public bool PadCentrePressed
		{
			get
			{
				if (OVRInput.Get(OVRInput.Button.DpadDown, controllerHand))
				{
					Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerHand);
					if (axis.y >= -0.4f && axis.y <= 0.4f && axis.x >= -0.4f && axis.x <= 0.4f)
						return true;
				}
				return false;
			}
		}
		public bool PadTouched
		{
			get { return OVRInput.Get(OVRInput.Button.DpadDown, controllerHand); }
		}
		public Vector2 PadPosition
		{
			get
			{
				return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerHand);
			}
		}

		protected virtual void OnEnable()
		{
			d_TriggerClicked += TriggerClicked;
			d_TriggerUnclicked += TriggerReleased;
			d_ThumbStickClicked += TrackpadDown;
			d_ThumbStickUnclicked += TrackpadUp;
			d_ThumbStickTouched += TrackpadTouch;
			d_ThumbStickUntouched += TrackpadUnTouch;
			d_Gripped += Gripped;
			d_Ungripped += UnGripped;
			d_BY_ButtonClicked += BYClicked;
			d_BY_ButtonUnclicked += BYReleased;
			d_AX_ButtonClicked += AXPressed;
			d_AX_ButtonUnclicked += AXReleased;
		}

		protected virtual void OnDisable()
		{
			d_TriggerClicked -= TriggerClicked;
			d_TriggerUnclicked -= TriggerReleased;
			d_ThumbStickClicked -= TrackpadDown;
			d_ThumbStickUnclicked -= TrackpadUp;
			d_ThumbStickTouched -= TrackpadTouch;
			d_ThumbStickUntouched -= TrackpadUnTouch;
			d_Gripped -= Gripped;
			d_Ungripped -= UnGripped;
			d_BY_ButtonClicked -= BYClicked;
			d_BY_ButtonUnclicked -= BYReleased;
			d_AX_ButtonClicked -= AXPressed;
			d_AX_ButtonUnclicked -= AXReleased;
		}

		protected virtual void Update()
		{
			float trigger = TriggerPressure;
			if (trigger > 0.9f && !triggerPressed)
			{
				triggerPressed = true;
				OnTriggerClicked();
			} else if (trigger < 0.9f && triggerPressed)
			{
				triggerPressed = false;
				OnTriggerUnclicked();
			}

			bool thumbstick = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controllerHand);
			if (thumbstick && !padPressed)
			{
				padPressed = true;
				OnThumbStickClicked();
			} else if (!thumbstick && padPressed)
			{
				padPressed = false;
				OnThumbStickUnclicked();
			}

			bool thumbstickTouch = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, controllerHand);
			if (thumbstickTouch && !padTouched)
			{
				padTouched = true;
				OnThumbStickTouched();
			} else if (!thumbstickTouch && padTouched)
			{
				padTouched = false;
				OnThumbStickUntouched();
			}
				
			float grip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controllerHand);
			if (grip > 0.9f && !gripped)
			{
				gripped = true;
				OnGripped();
			} else if (grip < 0.9f && gripped)
			{
				gripped = false;
				OnUngripped();
			}

			bool AX = OVRInput.Get(OVRInput.Button.One, controllerHand);
			if (AX && !AX_Pressed)
			{
				AX_Pressed = true;
				OnAX_Clicked();
			} else if (!AX && AX_Pressed)
			{
				AX_Pressed = false;
				OnAX_Unclicked();
			}

			bool BY = OVRInput.Get(OVRInput.Button.Two, controllerHand);
			if (BY && !BY_Pressed)
			{
				BY_Pressed = true;
				OnBY_Clicked();
			} else if (!BY && BY_Pressed)
			{
				BY_Pressed = false;
				OnBY_Unclicked();
			}
		}

		public virtual void OnTriggerClicked()
		{
			if (d_TriggerClicked != null)
				d_TriggerClicked(this);
		}

		public virtual void OnTriggerUnclicked()
		{
			if (d_TriggerUnclicked != null)
				d_TriggerUnclicked(this);
		}

		public virtual void OnThumbStickClicked()
		{
			if (d_ThumbStickClicked != null)
				d_ThumbStickClicked(this);
		}

		public virtual void OnThumbStickUnclicked()
		{
			if (d_ThumbStickUnclicked != null)
				d_ThumbStickUnclicked(this);
		}

		public virtual void OnThumbStickTouched()
		{
			if (d_ThumbStickTouched != null)
				d_ThumbStickTouched(this);
		}

		public virtual void OnThumbStickUntouched()
		{
			if (d_ThumbStickUntouched != null)
				d_ThumbStickUntouched(this);
		}

		public virtual void OnGripped()
		{
			if (d_Gripped != null)
				d_Gripped(this);
		}

		public virtual void OnUngripped()
		{
			if (d_Ungripped != null)
				d_Ungripped(this);
		}

		public virtual void OnBY_Clicked()
		{
			if (d_BY_ButtonClicked != null)
				d_BY_ButtonClicked(this);
		}

		public virtual void OnBY_Unclicked()
		{
			if (d_BY_ButtonUnclicked != null)
				d_BY_ButtonUnclicked(this);
		}

		public virtual void OnAX_Clicked()
		{
			if (d_AX_ButtonClicked != null)
				d_AX_ButtonClicked(this);
		}

		public virtual void OnAX_Unclicked()
		{
			if (d_AX_ButtonUnclicked != null)
				d_AX_ButtonUnclicked(this);
		}

		void TriggerClicked(object sender)
		{
			int triggerKey = this.triggerKey;
			if (triggerKey >= VRActions.Length)
			{
				Debug.LogWarning("Trigger key index (" + triggerKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[triggerKey], SendMessageOptions.DontRequireReceiver);
		}

		void TriggerReleased(object sender)
		{
			int triggerKey = this.triggerKey;
			if (triggerKey >= VRActions.Length)
			{
				Debug.LogWarning("Trigger key index (" + triggerKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[triggerKey]+"Released", SendMessageOptions.DontRequireReceiver);
		}

		void TrackpadTouch(object sender)
		{
			int touchKey = this.stickTouch;
			if (touchKey >= VRActions.Length)
			{
				Debug.LogWarning("Touch key index (" + touchKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[touchKey], SendMessageOptions.DontRequireReceiver);
		}

		void TrackpadUnTouch(object sender)
		{
			int touchKey = this.stickTouch;
			if (touchKey >= VRActions.Length)
			{
				Debug.LogWarning("Touch key index (" + touchKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[touchKey]+"Released", SendMessageOptions.DontRequireReceiver);
		}

		void TrackpadDown(object sender)
		{
			int action = 0;
			if (PadUpPressed) action = stickTop;
			if (PadLeftPressed) action = stickLeft;
			if (PadRightPressed) action = stickRight;
			if (PadDownPressed) action = stickBottom;
			if (PadCentrePressed) action = stickCentre;

			if (action >= VRActions.Length)
			{
				Debug.LogWarning("Pad key index (" + action + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[action], SendMessageOptions.DontRequireReceiver);
		}

		void TrackpadUp(object sender)
		{
			for(int i=0; i<VRActions.Length; i++)
			{
				if (stickLeft == i || stickTop == i || stickRight == i || stickBottom == i || stickCentre == i)
					SendMessage(VRActions[i]+"Released", SendMessageOptions.DontRequireReceiver);
			}
		}

		void Gripped(object sender)
		{
			int gripKey = this.gripKey;
			if (gripKey >= VRActions.Length)
			{
				Debug.LogWarning("Gripped key index (" + gripKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[gripKey], SendMessageOptions.DontRequireReceiver);
		}

		void UnGripped(object sender)
		{
			int gripKey = this.gripKey;
			if (gripKey >= VRActions.Length)
			{
				Debug.LogWarning("Gripped key index (" + gripKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[gripKey]+"Released", SendMessageOptions.DontRequireReceiver);
		}

		void BYClicked(object sender)
		{
			int menuKey = this.BYKey;
			if (menuKey >= VRActions.Length)
			{
				Debug.LogWarning("B/Y key index (" + menuKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[menuKey], SendMessageOptions.DontRequireReceiver);
		}

		void BYReleased(object sender)
		{
			int menuKey = this.BYKey;
			if (menuKey >= VRActions.Length)
			{
				Debug.LogWarning("B/Y key index (" + menuKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[menuKey]+"Released", SendMessageOptions.DontRequireReceiver);
		}

		void AXPressed(object sender)
		{
			int aButtonKey = this.AXKey;
			if (aButtonKey >= VRActions.Length)
			{
				Debug.LogWarning("A/X Button key index (" + aButtonKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[aButtonKey], SendMessageOptions.DontRequireReceiver);
		}

		void AXReleased(object sender)
		{
			int aButtonKey = this.AXKey;
			if (aButtonKey >= VRActions.Length)
			{
				Debug.LogWarning("A/X Button key index (" + aButtonKey + ") out of range (" + VRActions.Length+")");
				return;
			}
			SendMessage(VRActions[aButtonKey]+"Released", SendMessageOptions.DontRequireReceiver);
		}
	}
}
