using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using Characters;
using GameManagers;
using System.Collections;
using Utility;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using System.Reflection;
using UnityEditor;
using SimpleJSONFixed;

namespace UI
{
    class CreditsMenu: BaseMenu
    {
        private Transform _creditsLabelTransform;

        public override void Setup()
        {
            base.Setup();
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: "DefaultPanel");
            var button = ElementFactory.CreateDefaultButton(transform, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.SetAnchor(button, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
            var label = ElementFactory.CreateDefaultLabel(transform, style, GetCreditsString());
            label.GetComponent<Text>().color = Color.white;
            _creditsLabelTransform = label.transform;
            ElementFactory.SetAnchor(label, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, -500f));
        }

        private string GetCreditsString()
        {
            string str = "";
            if (MiscInfo.Credits != null)
            {
                foreach (JSONNode node in MiscInfo.Credits)
                {
                    // ElementFactory.CreateDefaultLabel(SinglePanel, style, node["Category"].Value + ":", FontStyle.Bold, TextAnchor.MiddleLeft);
                    List<string> names = new List<string>();
                    foreach (JSONNode name in node["Names"])
                    {
                        if (!names.Contains(name.Value))
                            names.Add(name.Value);
                    }
                    str += "\n<size=28><b>" + node["Category"].Value + "</b></size>\n";
                    names.Sort();
                    foreach (string name in names)
                        str += name + "\n";
                }
                str += "\n\n\nBased on the original game created by Feng Li and Jiang Li.";
                return str;
            }
            else
                return "Error loading data.";
        }

        private void Update()
        {
            _creditsLabelTransform.localPosition += Vector3.up * Time.deltaTime * 60f;
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                SceneLoader.LoadScene(SceneName.MainMenu);
        }

    }
}
