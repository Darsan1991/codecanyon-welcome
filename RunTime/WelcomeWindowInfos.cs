using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGames.Welcome
{
    public partial class WelcomeWindowInfos:IEnumerable<SectionInfo>
    {
        [SerializeField] private string _title = "Welcome to the Project";
        [TextArea]
        [SerializeField] private string _welcomeMessage =
            "Thank you for buying the product from us. Go through the tutorials to setup the project.\n If you got any errors you can contact us. we can guide to fix the problem.";
        [SerializeField] private List<SectionInfo> _sections = new();

        public string WelcomeMessage => _welcomeMessage;

        public string Title => _title;

        public IEnumerator<SectionInfo> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        

        
    }
    
    [Serializable]
    public struct SectionInfo
    {
        public string title;
        public List<TileInfo> tiles;
    }
        
    [Serializable]
    public struct TileInfo
    {
        public string title;
        public string message;
        public Texture icon;
        public ActionType actionType;
        public string url;

    }
        
    public enum ActionType
    {
        Web,File
    }
    public partial class WelcomeWindowInfos : ScriptableObject
    {
        public static WelcomeWindowInfos Default => Resources.Load<WelcomeWindowInfos>(nameof(WelcomeWindowInfos));
#if UNITY_EDITOR
        [UnityEditor.MenuItem("MyGames/System/Welcome Window Infos")]
        public static void Open()
        {
            ScriptableEditorUtils.OpenOrCreateDefault<WelcomeWindowInfos>();
        }
#endif
    }
}