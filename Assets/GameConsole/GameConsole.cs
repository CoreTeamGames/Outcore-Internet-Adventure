using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConsole
{
    public delegate void OnCommandSendToConsole(string command);
    public static OnCommandSendToConsole onCommandSendToConsole;
}
