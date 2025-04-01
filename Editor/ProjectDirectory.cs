using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;

namespace ProjectDir.Editor
{
    public static class ProjectDirectory
    {
        [MenuItem("Utilities/Create Starter Folders")]
        public static void CreateStarterDirectories()
        {
            var root = $"{Application.dataPath}";
            var project = $"{Application.dataPath}/_Project";
            var graphics = $"{Application.dataPath}/Graphics";
            
            List<(string name, string root)> directories = new List<(string name, string root)>()
            {
                ("_Project", root),
                ("Scripts", root),
                ("Prefabs", project),
                ("Scenes", project),
                ("Graphics", project),
                ("Textures", graphics),
                ("Materials", graphics),
                ("Audio", graphics),
                ("Fonts", graphics),
                ("Shaders", graphics),
                ("Models", graphics),
            };

            foreach (var directory in directories)
            {
                CreateDirectory(directory.name, directory.root);
            }

            AssetDatabase.Refresh();
        }

        private static string CreateDirectory(string name, string root = "")
        {
            if (string.IsNullOrEmpty(root))
            {
                root = Application.dataPath;
            }

            var pathToCreate = Path.Combine(root, name);
            if (Directory.Exists(pathToCreate)) return pathToCreate;
            Directory.CreateDirectory(pathToCreate);

            return pathToCreate;
        }
    }
}