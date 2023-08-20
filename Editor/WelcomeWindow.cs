using System;
using System.IO;
using System.Linq;
using DGames.Essentials.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DGames.Welcome
{
    public partial class WelcomeWindow : EditorWindow
    {
        private VisualElement _container;

        public static bool ShowAtStart
        {
            get => EditorPrefs.GetBool(nameof(ShowAtStart), true);
            set => EditorPrefs.SetBool(nameof(ShowAtStart), value);
        }

        [MenuItem("MyGames/Welcome &2",priority = -1)]
        public static void Open()
        {
            var window = GetWindow<WelcomeWindow>();
            window.titleContent = new GUIContent("Welcome",
                EditorGUIUtility.IconContent("d_BuildSettings.Standalone.Small").image);
            window.minSize = new Vector2(800, 550);
            window.Show();
        }


        private void CreateGUI()
        {
            rootVisualElement.Clear();
            // rootVisualElement.Add(new IMGUIContainer(OnGUI));

            CreateTitleBar();

            rootVisualElement.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);

            _container = new VisualElement()
            {
                style =
                {
                    display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column),
                    // justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween),
                    // flexWrap = new StyleEnum<Wrap>(Wrap.Wrap),

                    flexGrow = 1,
                    marginLeft = 5,
                    marginRight = 5,
                }
            };

            rootVisualElement.Add(_container);
            CreateFooter();

            foreach (var info in WelcomeWindowInfos.Default)
            {
                AddSection(info);
            }
        }

        private void CreateTitleBar()
        {
            var box = new Box()
            {
                style = { borderBottomColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f, 0.1f)) }
            };
            box.Add(new Label(WelcomeWindowInfos.Default.Title)
            {
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    fontSize = 35,
                    color = new StyleColor(new Color(0.4f, 0.4f, 0.4f)),
                    marginBottom = 8,
                    marginTop = 8
                }
            });
            rootVisualElement.Add(box);
            rootVisualElement.Add(new Label(WelcomeWindowInfos.Default.WelcomeMessage)
            {
                style =
                {
                    marginTop = 10,
                    marginBottom = 10,
                    marginLeft = 30,
                    marginRight = 30,
                    color = new StyleColor(new Color(0.4f, 0.4f, 0.4f)),
                    flexWrap = new StyleEnum<Wrap>(Wrap.Wrap),
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                }
            });
        }

        private void CreateFooter()
        {
            var footer = new Box()
            {
                style =
                {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column),
                    alignItems = new StyleEnum<Align>(Align.Center),
                }
            };
            rootVisualElement.Add(footer);

            footer.Add(new UnityEngine.UIElements.Button(DashBoardWindow.Open)
            {
                text = "Open The Dashboard", style =
                {
                    width = 300, height = 35, marginTop = 12,
                    borderBottomLeftRadius = 10,
                    borderBottomRightRadius = 10,
                    borderTopLeftRadius = 10,
                    borderTopRightRadius = 10,
                }
            });
            var toggleGroup = new VisualElement()
            {
                style =
                {
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    alignSelf = new StyleEnum<Align>(Align.FlexStart),
                    marginLeft = 10,
                    marginBottom = 10
                }
            };
            var toggle = new Toggle()
            {
                value = ShowAtStart,
            };
            toggle.RegisterValueChangedCallback(evt => ShowAtStart = evt.newValue);
            toggleGroup.Add(toggle);
            toggleGroup.Add(new Label("Show Window at Start")
            {
                style = { marginLeft = 5 }
            });
            footer.Add(toggleGroup);
        }

        private void AddSection(SectionInfo sectionInfo)
        {
            _container.Add(new Label(GetTitleWithDashes(sectionInfo.title, 200))
            {
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter), marginBottom = 15,
                    color = new StyleColor(Color.gray)
                }
            });
            var sectionContainer = new VisualElement()
            {
                style =
                {
                    display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween),
                    flexWrap = new StyleEnum<Wrap>(Wrap.Wrap),

                    flexGrow = 1,
                    marginLeft = 5,
                    marginRight = 5,
                }
            };
            foreach (var info in sectionInfo.tiles)
            {
                sectionContainer.Add(CreateTileItem(info.title, info.message, info.icon,
                    () => { OpenUrl(info.url, info.actionType); }));
            }

            _container.Add(sectionContainer);
        }

        private void OpenUrl(string url, ActionType type)
        {
            switch (type)
            {
                case ActionType.Web:
                    Application.OpenURL(url);
                    break;
                case ActionType.File:
                    var baseUrl =
                        $"{Application.dataPath.Substring(0, Application.dataPath.LastIndexOf(Path.DirectorySeparatorChar))}";
                    EditorUtility.OpenWithDefaultApp($"{baseUrl}{Path.DirectorySeparatorChar}{url}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string GetTitleWithDashes(string title, int countPerSide = 70)
        {
            var dashes = string.Join("", Enumerable.Repeat("-", countPerSide));

            return $"{dashes}{title}{dashes}";
        }

        // ReSharper disable once TooManyArguments
        private static UnityEngine.UIElements.Button CreateTileItem(string title, string message, Texture icon,
            Action onClick = null)
        {
            var element = new UnityEngine.UIElements.Button(() => onClick?.Invoke())
            {
                style =
                {
                    display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween),
                    width = new StyleLength(Length.Percent(49f)),
                    height = 70,
                    // marginLeft = 10,
                    // marginRight = 10,
                    marginBottom = 15,
                    borderBottomLeftRadius = 15,
                    borderBottomRightRadius = 15,
                    borderTopLeftRadius = 15,
                    borderTopRightRadius = 15,
                    paddingBottom = 10,
                    paddingLeft = 10,
                    paddingRight = 10,
                    paddingTop = 10,
                    backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f)),
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.UpperLeft)
                }
            };

            element.Add(new Image()
            {
                image = icon,
                style = { width = 50, height = 50 }
            });
            var rightContainer = new VisualElement()
            {
                style = { flexGrow = 1, marginLeft = 5, marginRight = 15, }
            };
            rightContainer.Add(new Label(title)
            {
                style =
                {
                    fontSize = 12
                }
            });
            rightContainer.Add(new Label(message)
            {
                style =
                {
                    color = Color.gray,
                    whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal)
                }
            });
            element.Add(rightContainer);
            return element;
        }
    }

    public partial class WelcomeWindow
    {
        [InitializeOnLoadMethod]
        public static void OnOpenProject()
        {
            Debug.Log(nameof(OnOpenProject) + ":" + ShowAtStart);


            EditorApplication.update += OnEditorApplicationUpdate;
        }

        private static void OnEditorApplicationUpdate()
        {
            if (EditorApplication.timeSinceStartup < 2f)
                return;

            if (ShowAtStart)
                Open();
            EditorApplication.update -= OnEditorApplicationUpdate;
        }
    }
}