using System;
using UnityEngine;
using System.IO;

public class GameConfig
{
	public const string CONFIG_SUB_FOLDER = "data/";
	public const String XML = ".xml";
	public static String CONFIG_FILE_EXTENSION = string.Empty;

	public enum BattleResult
	{
		SUCCESS = 0,
		FAILED = 1,
		ABSTAIN = 2
	}

	public static bool IsUseOutterConfig
	{
		get
		{
			if (Application.platform == RuntimePlatform.WindowsPlayer)
			{
				if (Directory.Exists(String.Concat(OutterPath, CONFIG_SUB_FOLDER)))
					return true;
			}
			return false;
		}
	}

	public static String OutterPath
	{
		get
		{
			var path = Application.dataPath + "/../HtcResources/";
			return path;
		}
	}
}