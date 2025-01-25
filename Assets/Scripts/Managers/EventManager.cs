
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

#region Cross Flow

        public event System.Action<Cell> OnCreateCrossOnCell;
        public event System.Action<Cell> OnCheckForMatch;
        public event System.Action<Cross> OnDisableCross;
        public event System.Action<int> OnCreateCrossMatrix;
        public event System.Action<int, int> OnCrossMatched;

        public void ONOnCreateCrossOnCell(Cell cell)
        {
            OnCreateCrossOnCell?.Invoke(cell);
        }

        public void ONOnCheckForMatch(Cell cell)
        {
            OnCheckForMatch?.Invoke(cell);
        }

        public void ONOnDisableCross(Cross cross)
        {
            OnDisableCross?.Invoke(cross);
        }

        public void ONOnCreateCrossMatrix(int size)
        {
            OnCreateCrossMatrix?.Invoke(size);
        }

        public void ONOnCrossMatched(int x, int y)
        {
            OnCrossMatched?.Invoke(x, y);
        }

#endregion

#region UI
        public event System.Action OnSettingsButtonClick;
        public event System.Action OnSettingsClosed;
        public event System.Action<int> OnRebuildButtonClick;

        public void ONOnSettingsButtonClick()
        {
            OnSettingsButtonClick?.Invoke();
        }
        public void ONOnSettingsClosed()
        {
            OnSettingsClosed?.Invoke();
        }

        public void ONOnRebuildButtonClick(int size)
        {
            OnRebuildButtonClick?.Invoke(size);
        }
#endregion



        //remove listeners from all of the events here
        public void NextLevelReset()
        {
            ONLevelStart= null;
            ONLevelEnd = null;
            OnMouseDown = null;
            OnCreateCrossOnCell = null;
            OnCheckForMatch = null;
            OnDisableCross = null;
            OnCreateCrossMatrix = null;
            OnCrossMatched = null;
            OnSettingsButtonClick = null;
            OnSettingsClosed = null;
            OnRebuildButtonClick = null;
        }


        private void OnApplicationQuit() {
            NextLevelReset();
        }
    }
}
