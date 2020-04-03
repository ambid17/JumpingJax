using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Consolation
{
    /// <summary>
    /// A console to display Unity's debug logs in-game.
    ///
    /// Version: 1.1.0
    /// </summary>
    class Console : MonoBehaviour
    {
        #region Inspector Settings

        /// <summary>
        /// The hotkey to show and hide the console window.
        /// </summary>
        public KeyCode toggleKey = KeyCode.BackQuote;

        /// <summary>
        /// Whether to open as soon as the game starts.
        /// </summary>
        public bool openOnStart = false;

        /// <summary>
        /// Whether to open the window by shaking the device (mobile-only).
        /// </summary>
        public bool shakeToOpen = true;

        /// <summary>
        /// The (squared) acceleration above which the window should open.
        /// </summary>
        public float shakeAcceleration = 3f;

        /// <summary>
        /// Whether to only keep a certain number of logs, useful if memory usage is a concern.
        /// </summary>
        public bool restrictLogCount = false;

        /// <summary>
        /// Number of logs to keep before removing old ones.
        /// </summary>
        public int maxLogCount = 1000;

        /// <summary>
        /// Font size to display log entries with.
        /// </summary>
        public int logFontSize = 12;

        /// <summary>
        /// Amount to scale UI by.
        /// </summary>
        public float scaleFactor = 1f;

        #endregion Inspector Settings

        private static readonly GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        private static readonly GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
        private const int margin = 20;
        private const string windowTitle = "Console";

        private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        private bool isCollapsed;
        private bool isVisible;
        private readonly List<Log> logs = new List<Log>();
        private readonly ConcurrentQueue<Log> queuedLogs = new ConcurrentQueue<Log>();

        private Vector2 scrollPosition;
        private readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
        private float windowX = margin;
        private float windowY = margin;

        private readonly Dictionary<LogType, bool> logTypeFilters = new Dictionary<LogType, bool>
        {
            { LogType.Assert, true },
            { LogType.Error, true },
            { LogType.Exception, true },
            { LogType.Log, true },
            { LogType.Warning, true },
        };

        #region MonoBehaviour Messages

        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= HandleLogThreaded;
        }

        private void OnEnable()
        {
            Application.logMessageReceivedThreaded += HandleLogThreaded;
        }

        private void OnGUI()
        {
            if (!isVisible)
            {
                return;
            }

            GUI.matrix = Matrix4x4.Scale(Vector3.one * scaleFactor);

            float width = (Screen.width / scaleFactor) - (margin * 2);
            float height = (Screen.height / scaleFactor) - (margin * 2);
            Rect windowRect = new Rect(windowX, windowY, width, height);

            Rect newWindowRect = GUILayout.Window(123456, windowRect, DrawWindow, windowTitle);
            windowX = newWindowRect.x;
            windowY = newWindowRect.y;
        }

        private void Start()
        {
            if (openOnStart)
            {
                isVisible = true;
            }
        }

        private void Update()
        {
            UpdateQueuedLogs();

            if (Input.GetKeyDown(toggleKey))
            {
                isVisible = !isVisible;
            }

            if (shakeToOpen && Input.acceleration.sqrMagnitude > shakeAcceleration)
            {
                isVisible = true;
            }
        }

        #endregion MonoBehaviour Messages

        private void DrawLog(Log log, GUIStyle logStyle, GUIStyle badgeStyle)
        {
            GUI.contentColor = logTypeColors[log.type];

            if (isCollapsed)
            {
                // Draw collapsed log with badge indicating count.
                GUILayout.BeginHorizontal();
                GUILayout.Label(log.GetTruncatedMessage(), logStyle);
                GUILayout.FlexibleSpace();
                GUILayout.Label(log.count.ToString(), GUI.skin.box);
                GUILayout.EndHorizontal();
            }
            else
            {
                // Draw expanded log.
                for (var i = 0; i < log.count; i += 1)
                {
                    GUILayout.Label(log.GetTruncatedMessage(), logStyle);
                }
            }

            GUI.contentColor = Color.white;
        }

        private void DrawLogList()
        {
            GUIStyle badgeStyle = GUI.skin.box;
            badgeStyle.fontSize = logFontSize;

            GUIStyle logStyle = GUI.skin.label;
            logStyle.fontSize = logFontSize;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // Used to determine height of accumulated log labels.
            GUILayout.BeginVertical();

            var visibleLogs = logs.Where(IsLogVisible);

            foreach (Log log in visibleLogs)
            {
                DrawLog(log, logStyle, badgeStyle);
            }

            GUILayout.EndVertical();
            var innerScrollRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndScrollView();
            var outerScrollRect = GUILayoutUtility.GetLastRect();

            // If we're scrolled to bottom now, guarantee that it continues to be in next cycle.
            if (Event.current.type == EventType.Repaint && IsScrolledToBottom(innerScrollRect, outerScrollRect))
            {
                ScrollToBottom();
            }
        }

        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(clearLabel))
            {
                logs.Clear();
            }

            foreach (LogType logType in Enum.GetValues(typeof(LogType)))
            {
                var currentState = logTypeFilters[logType];
                var label = logType.ToString();
                logTypeFilters[logType] = GUILayout.Toggle(currentState, label, GUILayout.ExpandWidth(false));
                GUILayout.Space(20);
            }

            isCollapsed = GUILayout.Toggle(isCollapsed, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();
        }

        private void DrawWindow(int windowID)
        {
            DrawLogList();
            DrawToolbar();

            // Allow the window to be dragged by its title bar.
            GUI.DragWindow(titleBarRect);
        }

        private Log? GetLastLog()
        {
            if (logs.Count == 0)
            {
                return null;
            }

            return logs.Last();
        }

        private void UpdateQueuedLogs()
        {
            Log log;
            while (queuedLogs.TryDequeue(out log))
            {
                ProcessLogItem(log);
            }
        }

        private void HandleLogThreaded(string message, string stackTrace, LogType type)
        {
            var log = new Log
            {
                count = 1,
                message = message,
                stackTrace = stackTrace,
                type = type,
            };

            // Queue the log into a ConcurrentQueue to be processed later in the Unity main thread,
            // so that we don't get GUI-related errors for logs coming from other threads
            queuedLogs.Enqueue(log);
        }

        private void ProcessLogItem(Log log)
        {
            var lastLog = GetLastLog();
            var isDuplicateOfLastLog = lastLog.HasValue && log.Equals(lastLog.Value);

            if (isDuplicateOfLastLog)
            {
                // Replace previous log with incremented count instead of adding a new one.
                log.count = lastLog.Value.count + 1;
                logs[logs.Count - 1] = log;
            }
            else
            {
                logs.Add(log);
                TrimExcessLogs();
            }
        }

        private bool IsLogVisible(Log log)
        {
            return logTypeFilters[log.type];
        }

        private bool IsScrolledToBottom(Rect innerScrollRect, Rect outerScrollRect)
        {
            var innerScrollHeight = innerScrollRect.height;

            // Take into account extra padding added to the scroll container.
            var outerScrollHeight = outerScrollRect.height - GUI.skin.box.padding.vertical;

            // If contents of scroll view haven't exceeded outer container, treat it as scrolled to bottom.
            if (outerScrollHeight > innerScrollHeight)
            {
                return true;
            }

            // Scrolled to bottom (with error margin for float math)
            return Mathf.Approximately(innerScrollHeight, scrollPosition.y + outerScrollHeight);
        }

        private void ScrollToBottom()
        {
            scrollPosition = new Vector2(0, Int32.MaxValue);
        }

        private void TrimExcessLogs()
        {
            if (!restrictLogCount)
            {
                return;
            }

            var amountToRemove = logs.Count - maxLogCount;

            if (amountToRemove <= 0)
            {
                return;
            }

            logs.RemoveRange(0, amountToRemove);
        }
    }

    /// <summary>
    /// A basic container for log details.
    /// </summary>
    struct Log
    {
        public int count;
        public string message;
        public string stackTrace;
        public LogType type;

        /// <summary>
        /// The max string length supported by UnityEngine.GUILayout.Label without triggering this error:
        /// "String too long for TextMeshGenerator. Cutting off characters."
        /// </summary>
        private const int maxMessageLength = 16382;

        public bool Equals(Log log)
        {
            return message == log.message && stackTrace == log.stackTrace && type == log.type;
        }

        /// <summary>
        /// Return a truncated message if it exceeds the max message length.
        /// </summary>
        public string GetTruncatedMessage()
        {
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }

            return message.Length <= maxMessageLength ? message : message.Substring(0, maxMessageLength);
        }
    }

    /// <summary>
    /// Alternative to System.Collections.Concurrent.ConcurrentQueue
    /// (It's only available in .NET 4.0 and greater)
    /// </summary>
    /// <remarks>
    /// It's a bit slow (as it uses locks), and only provides a small subset of the interface
    /// Overall, the implementation is intended to be simple & robust
    /// </remarks>
    class ConcurrentQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly object queueLock = new object();

        public void Enqueue(T item)
        {
            lock (queueLock)
            {
                queue.Enqueue(item);
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (queueLock)
            {
                if (queue.Count == 0)
                {
                    result = default(T);
                    return false;
                }

                result = queue.Dequeue();
                return true;
            }
        }
    }
}