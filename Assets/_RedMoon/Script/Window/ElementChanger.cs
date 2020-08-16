using RenderHeads.Media.AVProVideo;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ElementChanger : MonoBehaviour
{
    public string[] videoFiles;
    public MediaPlayer mediaPlayer;
    public GameObject photo;
    public GameObject breakingNews;

    [SerializeField] private GameObject m_StopWhisperButton;

    private int index = 0;

    private bool isFirstPlay = true;

    void Start()
    {
        breakingNews.SetActive(false);
        int mission_type = MissionProgress.GetCurMissionType();
        int mission_index = MissionProgress.GetMissionChallengeIndex(mission_type);

        if (mission_index == 0 || mission_index == 4 || mission_index == 9 || mission_index == 14)
        {
            m_StopWhisperButton.SetActive(true);
            GameObject.Find("WhisperAudio").GetComponent<AudioSource>().Play();
        }
        else
        {
            GameObject.Find("WhisperAudio").GetComponent<AudioSource>().Stop();
        }

        if (mission_index == 0)
        {
            if (isFirstPlay == true)
            {
                index = 9;
                mediaPlayer.m_Loop = false;
            }
            else
            {
                index = 0;
                mediaPlayer.m_Loop = true;
            }
        }

        if (mission_index > 0 && mission_index < 4)
        {
            index = 0; mediaPlayer.m_Loop = true;
        }
        else if (mission_index >= 4 && mission_index < 6)
        {
            index = 1;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index == 6)
        {
            index = 2;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index >= 7 && mission_index < 9)
        {
            index = 3;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index == 9)
        {
            index = 4;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index == 10)
        {
            index = 5;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index >= 11 && mission_index < 20)
        {
            index = 6;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index >= 20 && mission_index < 25)
        {
            index = 7;
            mediaPlayer.m_Loop = true;
        }
        else if (mission_index >= 25)
        {
            index = 8;
            mediaPlayer.m_Loop = true;
        }

        if (index > videoFiles.Length - 1)
        {
            index = videoFiles.Length - 1;
            mediaPlayer.m_Loop = true;
        }

        if (index == 8)
        {
            LoadPhoto();
        }

        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, videoFiles[index], true);

        if (mission_index > MissionProgress.GetMissionCurIndex(mission_type))
            if (mission_index == 4
                || mission_index == 6
                || mission_index == 7
                || mission_index == 9
                || mission_index == 10
                || mission_index == 11
                || mission_index == 20
                || mission_index == 25)
                breakingNews.SetActive(true);
            else
                breakingNews.SetActive(false);
    }

    private void ShowHeroImage()
    {
        if (index == 8)
        {
            if (mediaPlayer.Control.GetCurrentTimeMs() >= 7300f && mediaPlayer.Control.GetCurrentTimeMs() <= 21000)
                photo.SetActive(true);
            else
                photo.SetActive(false);
        }
    }

    private void Update()
    {
        ShowHeroImage();

        if (isFirstPlay == true)
        {
            if (mediaPlayer.Control.IsFinished() == true)
            {
                isFirstPlay = false;

                int mission_type = MissionProgress.GetCurMissionType();
                int mission_index = MissionProgress.GetMissionChallengeIndex(mission_type);

                if (mission_index == 0)
                {
                    index = 0;
                }

                if (mission_index > 0 && mission_index < 4)
                {
                    index = 0;
                }
                else if (mission_index >= 4 && mission_index < 6)
                {
                    index = 1;
                }
                else if (mission_index == 6)
                {
                    index = 2;
                }
                else if (mission_index >= 7 && mission_index < 9)
                {
                    index = 3;
                }
                else if (mission_index == 9)
                {
                    index = 4;
                }
                else if (mission_index == 10)
                {
                    index = 5;
                }
                else if (mission_index >= 11 && mission_index < 20)
                {
                    index = 6;
                }
                else if (mission_index >= 20 && mission_index < 25)
                {
                    index = 7;
                }
                else if (mission_index >= 25)
                {
                    index = 8;
                }

                if (index > videoFiles.Length - 1)
                {
                    index = videoFiles.Length - 1;
                }

                mediaPlayer.m_Loop = true;

                mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, videoFiles[index], true);
            }
        }
    }

    public void PlayCommercialVideo()
    {
        isFirstPlay = true;
        mediaPlayer.m_Loop = false;
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, videoFiles[9], true);

        m_StopWhisperButton.SetActive(false);
        GameObject.Find("WhisperAudio").GetComponent<AudioSource>().Stop();
    }

    void LoadPhoto()
    {
        if (!File.Exists(Application.persistentDataPath + "/nimei.png"))
            return;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        byte[] imageByteData = File.ReadAllBytes(Application.persistentDataPath + "/nimei.png");
        Debug.Log(Application.persistentDataPath + "/nimei.png" + imageByteData.Length);
        tex.LoadImage(imageByteData);
        photo.GetComponent<RawImage>().texture = tex;
    }
}