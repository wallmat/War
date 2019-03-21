using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace War
{
    public class RoundCounter : MonoBehaviour
    {
        Text RoundText = null;

        // Start is called before the first frame update
        void Start()
        {
            //make sure we can find the parts we need
            var textObj = transform.Find("Text"); 
            if(textObj == null)
            {
                Debug.LogError("Unable to find text on round counter", gameObject);
                return;
            }

            RoundText = textObj.GetComponent<Text>();
            if(RoundText == null)
            {
                Debug.LogError("Unable to find text comp on round counter obj", gameObject);
                return;
            }
        }

        //turn on the banner and show the message
        public void UpdateRound(int round)
        {
            RoundText.text = string.Format("{0} / {1}", round, Settings.MaxNumRounds);
        }
    }
}