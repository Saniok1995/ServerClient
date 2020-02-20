using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Common
{
    public struct MessageData
    {
        public int command;      

        public MessageData(TypeCommand command)
        {
            this.command = (int)command;
        }

        public TypeCommand GetCommand() {
            return (TypeCommand)command;
        }

    }

    public enum TypeCommand
    {
        Non, ToggleLight, Boom
    }
}
