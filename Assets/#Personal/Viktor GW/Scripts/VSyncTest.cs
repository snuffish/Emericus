using UnityEngine;

namespace FPS.Scripts.Gameplay.SP1.Viktor
{
    public class VSyncTest : MonoBehaviour
    {
        public int vSyncTest = 1;
        private void Start()
        {
            QualitySettings.vSyncCount = vSyncTest;
        }
    }
}