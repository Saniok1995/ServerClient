﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Common
{
    public class MessageData : MonoBehaviour
    {
        public string message;

        public MessageData(string message)
        {
            this.message = message;
        }
    }
}
