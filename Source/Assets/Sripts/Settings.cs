using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    public static class Settings
    {
        public const int NumCardsForWar = 3;
        public const int NumPlayers = 2;
        public const int MaxNumRounds = 500;
        public const float RoundTime = 0.25f;

        public const string PlayerWins = "Player {0} Wins!";
        public const string MaxRoundsWin = "Max Rounds Reached, Player {0} Wins!";
        public const string MaxRoundsTie = "Max Rounds Reached, Tie Game!";
        

        public enum CardPlayType { BATTLE = 0, WAR_BATTLE }
    }
}