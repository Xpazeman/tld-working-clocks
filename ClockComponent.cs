using UnityEngine;
using Il2CppInterop.Runtime.Attributes;

namespace WorkingClocks
{
    class ClockComponent : MonoBehaviour
    {
        public GameObject? hourHand;
        public GameObject? minuteHand;

        public ClockComponent() { }

        public ClockComponent(System.IntPtr intPtr) : base(intPtr) { }

        [HideFromIl2Cpp]
        public void UpdateHands(float hourDeg, float minutesDeg)
        {
            //Move hands
            if (hourHand != null)
            {
                hourHand.transform.localEulerAngles = new Vector3(0, hourHand.transform.localEulerAngles.y, hourDeg);
            }

            if (minuteHand != null)
            {
                minuteHand.transform.localEulerAngles = new Vector3(0, hourHand.transform.localEulerAngles.y, minutesDeg);
            }
            
        }
    }
}
