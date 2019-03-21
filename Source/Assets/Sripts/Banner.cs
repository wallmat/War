using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace War
{
    public class Banner : MonoBehaviour
    {
        //save the backgound and text to toggle
        Text BannerText = null;
        Image Backgound = null;

        // Start is called before the first frame update
        void Start()
        {
            //make sure we can find the parts we need
            var textObj = transform.Find("Text"); 
            if(textObj == null)
            {
                Debug.LogError("Unable to find text on banner", gameObject);
                return;
            }

            BannerText = textObj.GetComponent<Text>();
            if(BannerText == null)
            {
                Debug.LogError("Unable to find text comp on banner obj", gameObject);
                return;
            }

            Backgound = GetComponent<Image>();
            if(Backgound == null)
            {
                Debug.LogError("Unable to find sprite comp on banner obj", gameObject);
                return;
            }

            Backgound.enabled = false;
            BannerText.enabled = false;
        }

        //turn on the banner and show the message
        public void ShowBanner(string text)
        {
            Backgound.enabled = true;
            BannerText.enabled = true;

            BannerText.text = text;
        }

    }
}