using UnityEngine;

namespace ArtillerySimulator
{
    public class CursorHandler : MonoBehaviour
    {
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
