
using UnityEngine;

namespace Managers
{
    public sealed class EventManager : Singleton<EventManager>
    {
        
#region Level Status

        public event System.Action<bool> ONLevelEnd;
        public event System.Action ONLevelStart;
        
        public void OnONLevelStart(){
            ONLevelStart?.Invoke();
        }
        public void OnONLevelEnd(bool isSuccess)
        {
            ONLevelEnd?.Invoke(isSuccess);
        }

#endregion

#region Input

        public event System.Action<Vector2> OnMouseDown;

        public void ONOnMouseDown(Vector2 position)
        {
            OnMouseDown?.Invoke(position);
        }

#endregion

#region Platforms

        public event System.Action<int,float> OnSpawnAnotherPlatform;
        public event System.Action OnPlatformStopMoving;
        public event System.Action<float> OnChangeNextSpawn;
        public event System.Action<float> OnSendPlatformScaleInfo;

        public void ONOnSpawnAnotherPlatform(int sideChoice, float position)
        {
            OnSpawnAnotherPlatform?.Invoke(sideChoice, position);
        }

        public void ONOnPlatformStopMoving()
        {
            OnPlatformStopMoving?.Invoke();
        }

        public void ONOnChangeNextSpawn(float nextSpawn)
        {
            OnChangeNextSpawn?.Invoke(nextSpawn);
        }

        public void ONOnSendPlatformScaleInfo(float platformScale)
        {
            OnSendPlatformScaleInfo?.Invoke(platformScale);
        }
#endregion



        //remove listeners from all of the events here
        public void NextLevelReset()
        {
            ONLevelStart= null;
            ONLevelEnd = null;
            OnMouseDown = null;
        }


        private void OnApplicationQuit() {
            NextLevelReset();
        }
    }
}
