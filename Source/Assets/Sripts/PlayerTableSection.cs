using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    public class PlayerTableSection
    {
        public Transform BattleHolder { get; set; }
        public Transform WarBattleHolder {get; set; }
        public List<Transform> WarHolders { get; set; }
    }
}