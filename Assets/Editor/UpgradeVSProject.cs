//#define USE_UPGRADEVS
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

class UpgradeVSProject : AssetPostprocessor
{
#if USE_UPGRADEVS
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        string currentDir = Directory.GetCurrentDirectory();
        string[] slnFile = Directory.GetFiles(currentDir, "*.sln");
        string[] csprojFile = Directory.GetFiles(currentDir, "*.csproj");

        bool hasChanged = false;
        if (slnFile != null)
        {
            for (int i = 0; i < slnFile.Length; i++)
            {
                if (ReplaceInFile(slnFile[i], "Format Version 10.00", "Format Version 11.00"))
                    hasChanged = true;
            }
        }

        if (csprojFile != null)
        {
            for (int i = 0; i < csprojFile.Length; i++)
            {
                if (ReplaceInFile(csprojFile[i], "ToolsVersion=\"3.5\"", "ToolsVersion=\"4.0\""))
                    hasChanged = true;

                if (ReplaceInFile(csprojFile[i], "<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>", "<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>"))
                    hasChanged = true;
            }
        }

        if (hasChanged)
        {
            Debug.LogWarning("Project is now upgraded to Visual Studio 2010 Solution!");
        }
        else
        {
            Debug.Log("Project-version has not changed...");
        }
    }

    static private bool ReplaceInFile(string filePath, string searchText, string replaceText)
    {
        StreamReader reader = new StreamReader(filePath);
        string content = reader.ReadToEnd();
        reader.Close();

        if (content.IndexOf(searchText) != -1)
        {
            content = Regex.Replace(content, searchText, replaceText);
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();

            return true;
        }

        return false;
    }
#endif
}
