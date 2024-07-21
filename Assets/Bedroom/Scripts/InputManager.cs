using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }

        void Update()
        {
            Move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Run = Input.GetKey(KeyCode.LeftShift);
        }
    }

}