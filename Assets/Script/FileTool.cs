using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

[Serializable]
public class PersistantData
{
    public int highScore = 0;
    public int pushNotify = 0;
    public bool isPlasmaLocked = true;
	public bool isAdsRemoved = false;
    public List<int> plantsNum = new List<int>();
    public List<int> taskNum = new List<int>();
    public List<int> rewardsNum = new List<int>();

    public PersistantData()
    {
        highScore = 0;
        pushNotify = 0;
        plantsNum = new List<int>();
        taskNum = new List<int>();
        rewardsNum = new List<int>();
        isPlasmaLocked = false;
		isAdsRemoved = false;
    }
}

public class FileTool {
	public static string RootPath {
		get {
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
				string tempPath = Application.persistentDataPath, dataPath;
				if (!string.IsNullOrEmpty (tempPath)) {

					dataPath = PlayerPrefs.GetString ("DataPath", "");
					if (string.IsNullOrEmpty (dataPath)) {
						PlayerPrefs.SetString ("DataPath", tempPath);
					}

					return tempPath + "/";
				} else {
					Debug.Log ("Application.persistentDataPath Is Null.");

					dataPath = PlayerPrefs.GetString ("DataPath", "");

					return dataPath + "/";
				}
			} else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
				return Application.dataPath.Replace ("Assets", "");
			} else {
				return Application.dataPath + "/";
			}
		}
	}

	public static void createORwriteFile (string fileName, string info) {
		FileStream fs = new FileStream (RootPath + fileName, FileMode.Create, FileAccess.Write);
		StreamWriter sw = new StreamWriter (fs);
		fs.SetLength (0);
		sw.WriteLine (info);
		sw.Close ();
		sw.Dispose ();
	}

    public static string ReadFile (string fileName, bool onlyreadline = true) {
		string fileContent = ""; 
		StreamReader sr = null;
		try {
			sr = File.OpenText (RootPath + fileName);
		} catch (Exception e) {
			Debug.Log (e.Message);
			return null;
		}
		if (onlyreadline) {
			while ((fileContent = sr.ReadLine ()) != null) {
				break; 
			}
		} else {
			fileContent = sr.ReadToEnd ();
		}
		sr.Close ();
		sr.Dispose ();
		return fileContent;
	}

	public static bool IsFileExists (string fileName) {
		return File.Exists (RootPath + fileName);
	}

	public static void DelectFile (string fileName) {
		File.Delete (RootPath + fileName);
	}

	public static void CopyFolder (string from, string to) {
		if (!Directory.Exists (to))
			Directory.CreateDirectory (to);

		foreach (string sub in Directory.GetDirectories(from))
			CopyFolder (sub, to + Path.GetFileName (sub) + "/");

		foreach (string file in Directory.GetFiles(from))
			File.Copy (file, to + Path.GetFileName (file), true);
	}

	public static void CopyFile (string from, string to, bool overWrite) {
		File.Copy (from, to, overWrite);
	}
}

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        string data = FileTool.ReadFile(file, false);
        var lines = Regex.Split(data, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "")
                continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}

