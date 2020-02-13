using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Menu
{
    public class SceneController : MonoBehaviour
    {
        public void OnClickServer()
        {
            SceneManager.LoadScene("RoomServer");
        }

        public void OnClickClient()
        {
            SceneManager.LoadScene("RoomClient");
        }
    }
}
