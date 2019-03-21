using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    public static class Settings
    {
        public const int NumCardsForWar = 3;
        public const int NumPlayers = 2;
        public const int MaxNumRounds = 1000;
        public const float RoundTime = 0.25f;

        public enum CardPlayType { BATTLE = 0, WAR_BATTLE }
    }
}