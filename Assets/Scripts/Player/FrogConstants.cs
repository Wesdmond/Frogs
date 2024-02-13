using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FrogConstants
{
    public struct FrogAnimations
    {
        public static readonly string moveRight = "Right";
        public static readonly string moveLeft = "Left";
        public static readonly string moveUp = "Up";
        public static readonly string moveDown = "Down";
        public static readonly string jump = "Jumping";
        public static readonly string shoot = "Shooting";
    }

    public struct FrogMaps
    {
        public static readonly string FrogDefaultMap = "Frog";
        public static readonly string FrogShootingMap = "FrogShootingMode";
    }

    public struct FrogTongueLayers
    {
        public static readonly int TongueDefaultOrderInLayers = 11;
        public static readonly int TongueNotLookingDownOrderInLayers = 9;
    }

}