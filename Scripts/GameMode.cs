using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMode
{
    public enum Modes
    {
        Default = 1,
        BirdInverse,
        SpaceInverse,
    }

    public static Modes gameMode = Modes.Default;

    public static string GetGameModeName()
    {
        switch (gameMode)
        {
            case Modes.Default:
                return "Default mode";
                break;
            case Modes.BirdInverse:
                return "Bird inverse mode";
                break;
            case Modes.SpaceInverse:
                return "Space inverse node";
                break;
            default: return "Default mode";
        }
    }
}
