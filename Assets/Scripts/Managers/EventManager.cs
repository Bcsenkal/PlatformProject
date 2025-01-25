
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public sealed class EventManager : Singleton<EventManager>
    {
        
#region Level Status

        public event System.Action<bool> OnLevelEnd;
        public event System.Action OnLevelStart;
        
        public void ONOnLevelStart(){
            OnLevelStart?.Invoke();
        }
        public void ONOnLevelEnd(bool isSuccess)
        {
            OnLevelEnd?.Invoke(isSuccess);
        }

#endregion

#region Input

        public event System.Action<Vector2> OnMouseDown;

        public void ONOnMouseDown(Vector2 position)
        {
            OnMouseDown?.Invoke(position);
        }

#endregion

#region Player

        public event System.Action OnPlayerStartMoving;
        public event System.Action<List<MovingPlatform>,bool> OnSetPlayerPath;

        public void ONOnSetPlayerPath(List<MovingPlatform> platforms,bool isWinCondition)
        {
            OnSetPlayerPath?.Invoke(platforms,isWinCondition);
        }
        
        public void ONOnPlayerStartMoving()
        {
            OnPlayerStartMoving?.Invoke();
        }


#endregion

#region Camera

        public event System.Action OnSwitchToOrbitalCamera;
        public event System.Action OnSwitchToFollowCamera;
        public event System.Action OnSwitchToFreeCamera;
        
        public void ONOnSwitchToOrbitalCamera()
        {
            OnSwitchToOrbitalCamera?.Invoke();
        }
        public void ONOnSwitchToFollowCamera()
        {
            OnSwitchToFollowCamera?.Invoke();
        }
        public void ONOnSwitchToFreeCamera()
        {
            OnSwitchToFreeCamera?.Invoke();
        }
        
#endregion

#region Platforms

        public event System.Action<float,float,int> OnSpawnMovingPlatform;
        public event System.Action<int> OnSpawnStaticPlatforms;
        public event System.Action<float> OnCallNextPlatform;
        public event System.Action<float> OnChangeNextSpawn;
        public event System.Action<Vector3> OnSendPlatformScaleInfo;
        public event System.Action<bool> OnPerfectPlacement;
        public event System.Action OnFailedPlacement;
        public event System.Action<MovingPlatform> OnAddPlatformToSpawnedList;

        public void ONOnSpawnMovingPlatform(float scaleX, float position,int spawnedPlatforms)
        {
            OnSpawnMovingPlatform?.Invoke(scaleX, position, spawnedPlatforms);
        }

        public void ONOnSpawnStaticPlatforms(int parkourLength)
        {
            OnSpawnStaticPlatforms?.Invoke(parkourLength);
        }

        public void ONOnCallNextPlatform(float platformScale)
        {
            OnCallNextPlatform?.Invoke(platformScale);
        }

        public void ONOnChangeNextSpawn(float nextSpawn)
        {
            OnChangeNextSpawn?.Invoke(nextSpawn);
        }

        public void ONOnSendPlatformScaleInfo(Vector3 platformScale)
        {
            OnSendPlatformScaleInfo?.Invoke(platformScale);
        }

        public void ONOnPerfectPlacement(bool isPerfect)
        {
            OnPerfectPlacement?.Invoke(isPerfect);
        }

        public void ONOnFailedPlacement()
        {
            OnFailedPlacement?.Invoke();
        }

        public void ONOnAddPlatformToSpawnedList(MovingPlatform platform)
        {
            OnAddPlatformToSpawnedList?.Invoke(platform);
        }


#endregion



        //remove listeners from all of the events here
        public void NextLevelReset()
        {
            OnLevelStart= null;
            OnLevelEnd = null;
            OnMouseDown = null;
            OnSpawnMovingPlatform = null;
            OnCallNextPlatform = null;
            OnChangeNextSpawn = null;
            OnSendPlatformScaleInfo = null;
            OnPerfectPlacement = null;
        }


        private void OnApplicationQuit() {
            NextLevelReset();
        }
    }
}
