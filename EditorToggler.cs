//exclude
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


class EditorToggler : EditorWindow
{
    private const string EXCLUDED_ARROW_TEXTURE = "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MjYxMTU1MjEyNjQ4MTFFOEFGODRCMDBEOEQ2NkY2NTAiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MjYxMTU1MjIyNjQ4MTFFOEFGODRCMDBEOEQ2NkY2NTAiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDoyNjExNTUxRjI2NDgxMUU4QUY4NEIwMEQ4RDY2RjY1MCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDoyNjExNTUyMDI2NDgxMUU4QUY4NEIwMEQ4RDY2RjY1MCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PmREYyYAAADmSURBVHjaYvz//z8DVcCLmzcxTAIZzoRLw+t79xhOLFmCoYnFWVPztDgfHx+y4N9//xi+vX/PwMTMzHDr0KH/anZ2jDA5xnBT07slJSVKv3/8gFgLVAwC//7+hShgYmLgl5Rk0PX0ZAQ5iQUkCFL8+/t3nP75+OwZzHmMLCDrPzx9yvDn50+cGpjZ2Bj4xMXBbKZ/IJ8D3crMygrGTCxASxkZGf7+/g3GILagjAyDbUoK2B+M1ioq+7GZWhIW5gByPysn5yPv6mp5WLDiBBvr6v4fX7z4P3o84ATY4gCkgZHUpAEQYABkoWN6iXaRvQAAAABJRU5ErkJggg==";
    private const string INCLUDED_ARROW_TEXTURE = "iVBORw0KGgoAAAANSUhEUgAAAAwAAAAMCAYAAABWdVznAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QUQ0QzkzRDUyNjQ4MTFFODg0OUY5OEU5MEY0NzE5RjAiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QUQ0QzkzRDYyNjQ4MTFFODg0OUY5OEU5MEY0NzE5RjAiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpBRDRDOTNEMzI2NDgxMUU4ODQ5Rjk4RTkwRjQ3MTlGMCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpBRDRDOTNENDI2NDgxMUU4ODQ5Rjk4RTkwRjQ3MTlGMCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PuhwlgYAAADeSURBVHjaYvz//z8DSQCbhpsvbuI0hQldYMmJJf/vvb6H0wIWZM6hW4f+P/3wlOH9t/cMpuGmN5mYUc379PLTJ0aQkxgZGRm2X97+//nH5wz//v8DSzIzMUOcwAjRxMHKwdDT03OPBeaMZx+fMfz++xtu2p9/f3A76cfvHwwvP71k+PX3F063s7OwM/z7+w/i6RTbFEYZQRkGRiAE2QLCIDYLEwsDKzMrGIOc+P8f0PkwP4BA69bWh99/f5cD+WNVz6oDRMXD4uOL/9dtrPtPUsSBAgGXekZSkwZAgAEACdFuCHRQqMwAAAAASUVORK5CYII=";
    private const string DISABLED_FILE = ".csdisabled";
    private const string DISABLED_FILE_SEARCH = "*" + DISABLED_FILE;
    private const int MAX_ITEM_SHOWN = 20;

    private static Texture2D m_activeTexture;
    private static Texture2D m_disableTexture;

    private static Texture2D m_includedArrowTexture;
    private static Texture2D m_excludedArrowTexture;

    private readonly Stack<string> m_filesToReimport = new Stack<string>();

    private readonly List<string> m_editorScripts = new List<string>();
    private readonly List<string> m_disabledEditorScripts = new List<string>();
    private readonly List<int> m_editorTobeConverted = new List<int>();
    private readonly List<int> m_disabledEditorTobeConverted = new List<int>();

    private int m_editorCurrentPage;
    private int m_editorPageMax;

    private int m_disabledEditorCurrentPage;
    private int m_disabledEditorPageMax;

    [MenuItem("Window/Jackisgames/EditorToggler")]
    public static void Init()
    {
        GetWindow<EditorToggler>().Show(true);
    }

    private void OnEnable()
    {
        
        titleContent = new GUIContent("Editor Toggler");
        ReloadTextures();
        Refresh();

    }

    private void OnGUI()
    {
        ReloadTextures();

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("Editor Script Active {0}/{1}", m_editorScripts.Count,
            m_editorScripts.Count + m_disabledEditorScripts.Count));

        if (GUILayout.Button("Disable All"))
        {
            m_disabledEditorTobeConverted.Clear();
            m_editorTobeConverted.AddRange(Enumerable.Range(0, m_editorScripts.Count));
        }


        if (GUILayout.Button("Enable All"))
        {
            m_editorTobeConverted.Clear();
            m_disabledEditorTobeConverted.AddRange(Enumerable.Range(0, m_disabledEditorScripts.Count));
        }
            
        
        if (GUILayout.Button("Refresh"))
            Refresh();
        
        GUILayout.EndHorizontal();
        Rect activeArea = position;
        activeArea.x = 0;
        activeArea.y = 0;
        activeArea.y = EditorGUIUtility.singleLineHeight * 2;
        activeArea.width *= .5f;
        activeArea.height = MAX_ITEM_SHOWN * EditorGUIUtility.singleLineHeight;

        GUI.DrawTexture(activeArea, m_activeTexture, ScaleMode.StretchToFill);
        GUILayout.BeginArea(activeArea);

        int editorStartIndex = m_editorCurrentPage * MAX_ITEM_SHOWN;
        for (int i = editorStartIndex; i < Mathf.Min(editorStartIndex + MAX_ITEM_SHOWN, m_editorScripts.Count); i++)
            DisplayOption(m_editorScripts[i], i, true);
        
        GUILayout.EndArea();

        activeArea.x += activeArea.width;

        GUI.DrawTexture(activeArea, m_disableTexture, ScaleMode.StretchToFill);
        GUILayout.BeginArea(activeArea);

        int disabledEditorStartIndex = m_disabledEditorCurrentPage * MAX_ITEM_SHOWN;
        for (int i = disabledEditorStartIndex; i < Mathf.Min(disabledEditorStartIndex + MAX_ITEM_SHOWN, m_disabledEditorScripts.Count); i++)
            DisplayOption(m_disabledEditorScripts[i], i, false);
        
        GUILayout.EndArea();
        GUILayout.Space(activeArea.height + EditorGUIUtility.singleLineHeight);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Prev"))
            m_editorCurrentPage = Mathf.Max(0, m_editorCurrentPage - 1);
        
        if (GUILayout.Button("Next"))
            m_editorCurrentPage = Mathf.Min(m_editorPageMax, m_editorCurrentPage + 1);
        
        if (GUILayout.Button("Prev"))
            m_disabledEditorCurrentPage = Mathf.Max(0, m_disabledEditorCurrentPage - 1);
        
        if (GUILayout.Button("Next"))
            m_disabledEditorCurrentPage = Mathf.Min(m_disabledEditorPageMax, m_disabledEditorCurrentPage + 1);
        
        GUILayout.EndHorizontal();
        if (EditorApplication.isCompiling)
        {
            GUILayout.Button("Editor is compiling...");
        }
        else
        {
            if (GUILayout.Button("Apply"))
            {
                m_filesToReimport.Clear();
                Flush(true);
                Flush(false);

                while (m_filesToReimport.Count > 0)
                {
                    AssetDatabase.ImportAsset(m_filesToReimport.Pop().Substring(Application.dataPath.Length - 6), ImportAssetOptions.ForceUpdate);
                }
                AssetDatabase.Refresh();

                Refresh();

            }
        }
    }

    private void ReloadTextures()
    {
        if (m_activeTexture == null)
        {
            m_activeTexture = new Texture2D(1, 1);
            m_disableTexture = new Texture2D(1, 1);
            
            m_activeTexture.SetPixel(0, 0, new Color(.1f, .2f, .1f));
            m_disableTexture.SetPixel(0, 0, new Color(.2f, .1f, .1f));

            m_includedArrowTexture = new Texture2D(1, 1);
            m_excludedArrowTexture = new Texture2D(1, 1);

            m_includedArrowTexture.LoadImage(System.Convert.FromBase64String(INCLUDED_ARROW_TEXTURE));
            m_excludedArrowTexture.LoadImage(System.Convert.FromBase64String(EXCLUDED_ARROW_TEXTURE));

            m_activeTexture.Apply();
            m_disableTexture.Apply();
        }
    }
    private void Refresh()
    {
        m_editorTobeConverted.Clear();
        m_disabledEditorTobeConverted.Clear();
        m_editorScripts.Clear();
        m_disabledEditorScripts.Clear();

        SearchDirectory(Application.dataPath, m_editorScripts, m_disabledEditorScripts);

        //exclude scripts
        for (int i = 0; i < m_editorScripts.Count; i++)
        {
            string url = m_editorScripts[i];
            string assetDataBasePath = url.Substring(Application.dataPath.Length - 6);

            MonoScript scriptAsset = AssetDatabase.LoadAssetAtPath<MonoScript>(assetDataBasePath);

            if (scriptAsset.text.StartsWith("//exclude"))
            {
                m_editorScripts.RemoveAt(i);
                i--;
            }

        }

        m_editorCurrentPage = m_disabledEditorCurrentPage = 0;
        m_editorPageMax = Mathf.CeilToInt((float)m_editorScripts.Count / MAX_ITEM_SHOWN) - 1;
        m_disabledEditorPageMax = Mathf.CeilToInt((float)m_disabledEditorScripts.Count / MAX_ITEM_SHOWN) - 1;
    }

    private void Flush(bool isEnabled)
    {
        List<int> markedList = isEnabled ? m_editorTobeConverted : m_disabledEditorTobeConverted;
        List<string> sourcePath = isEnabled ? m_editorScripts : m_disabledEditorScripts;
        List<string> targetPath = isEnabled ? m_disabledEditorScripts : m_editorScripts;

        markedList.Sort(DescendingSort);
        //rename files
        for (int i = 0; i < markedList.Count; i++)
        {
            int index = markedList[i];
            string url = sourcePath[index];
            string targetUrl = Path.ChangeExtension(url, isEnabled ? DISABLED_FILE : ".cs");

            sourcePath.RemoveAt(index);
            if (url != targetUrl)
            {
                File.Delete(targetUrl);
                File.Move(url, targetUrl);
                //move meta too
                string targetMetaUrl = targetUrl + ".meta";
                File.Delete(targetMetaUrl);
                File.Move(url + ".meta", targetMetaUrl);

                //include renamed assets to target path
                targetPath.Add(targetUrl);
                m_filesToReimport.Push(targetUrl);
            }
        }
        markedList.Clear();
    }

    private int DescendingSort(int a, int b)
    {
        return b.CompareTo(a);
    }
    private void DisplayOption(string url, int index, bool isEnabled)
    {
        string assetDataBasePath = url.Substring(Application.dataPath.Length - 6);

        MonoScript scriptAsset = null;
        Object textAsset = null;
        if (isEnabled)
        {
            scriptAsset = AssetDatabase.LoadAssetAtPath<MonoScript>(assetDataBasePath);
            if (scriptAsset == null)
                return;
        }
        else
        {
            textAsset = AssetDatabase.LoadAssetAtPath<Object>(assetDataBasePath);
            if (textAsset == null)
                return;
        }

        List<int> markedList = isEnabled ? m_editorTobeConverted : m_disabledEditorTobeConverted;
        bool isMarked = markedList.IndexOf(index) >= 0;

        GUILayout.BeginHorizontal();
        if (isEnabled)
        {
            EditorGUILayout.ObjectField(scriptAsset, typeof(MonoScript), false);
        }
        else
        {
            Rect iconRect = EditorGUILayout.GetControlRect(GUILayout.Width(EditorGUIUtility.singleLineHeight), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (isMarked)
                GUI.DrawTexture(iconRect, m_includedArrowTexture);

            EditorGUILayout.ObjectField(textAsset, typeof(Object), false);
        }
        if (isMarked)
        {

            if (GUILayout.Button("Cancel", GUILayout.Width(80)))
                markedList.Remove(index);
        }
        else
        {
            if (GUILayout.Button(isEnabled ? "Disable" : "Enable",GUILayout.Width(80)))
                markedList.Add(index);

        }

        if (isEnabled)
        {
            Rect iconRect = EditorGUILayout.GetControlRect(GUILayout.Width(EditorGUIUtility.singleLineHeight), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (isMarked)
                GUI.DrawTexture(iconRect, m_excludedArrowTexture);
        }

        GUILayout.EndHorizontal();
    }

    private void SearchDirectory(string targetFolder, List<string> editorScript, List<string> disabledScript)
    {
        string[] subfolders = Directory.GetDirectories(targetFolder);

        string folderName = Path.GetFileNameWithoutExtension(targetFolder);
        if (folderName.Equals("editor", System.StringComparison.OrdinalIgnoreCase))
        {
            editorScript.AddRange(Directory.GetFiles(targetFolder, "*.cs", SearchOption.AllDirectories));
            disabledScript.AddRange(Directory.GetFiles(targetFolder, DISABLED_FILE_SEARCH, SearchOption.AllDirectories));
        }
        else
        {
            for (int i = 0; i < subfolders.Length; i++)
                SearchDirectory(subfolders[i], editorScript, disabledScript);

        }
    }
}

