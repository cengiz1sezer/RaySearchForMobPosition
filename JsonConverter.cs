using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class LevelJSONInfo
{

}

public class Level
{

}

public class JsonConverter : MonoBehaviour
{
    public LevelJSONInfo levelData;

    public LevelJSONInfo GetLevelInfo() => levelData;

    Level crntLevel;


    public Level GetCurrentLevel() => crntLevel;


    public static JsonConverter I;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(I.gameObject);
        }

        I = this;
    }


    private void Start()
    {
        FillChapterAndLevel();
    }

    public async void FillChapterAndLevel()
    {
        levelData = await LoadLevels();
        //InfoManager.I.levelData = levelData;
        Debug.Log("init");

    }
    public async Task<LevelJSONInfo> LoadLevels()
    {
        return await LoadLevelsTask();
    }

    Task<LevelJSONInfo> LoadLevelsTask()
    {
        try
        {
            TextAsset _levelData = GetData<TextAsset>("SBF/Levels/EN");
            Debug.Log(_levelData);
            return _levelData.text.DeserializeJson<LevelJSONInfo>();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Level File Can't Found");
            Debug.LogException(e);
            return null;
        }
    }

    //internal Level GetLevelByID(int chapterID, int levelID)
    //{
    //    return levelData.chapters[chapterID].levels[levelID];
    //}


    //public bool IsThereLevel(int idx, int wantedLevel)
    //{
    //    Debug.Log("girdi");

    //    if (levelData.chapters[idx] != null)
    //    {
    //        if (levelData.chapters[idx].levels[wantedLevel] != null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    return false;


    //}

    //public void AssignLevel(int idx, int wantedLevel, Level lv)
    //{
    //    Replace(levelData.chapters[idx].levels, levelData.chapters[idx].levels[wantedLevel], lv);

    //}

    public IEnumerable<T> Replace<T>(IEnumerable<T> list, T find, T replaceWith)
    {

        foreach (T item in list)
        {
            yield return find.Equals(item) ? replaceWith : item;
        }
    }

    public T GetData<T>(string path) where T : Object
    {
        ResourceRequest handle = LoadObject<T>(path);

        return handle.asset as T;
    }

    public ResourceRequest LoadObject<T>(string path)
    {
        ResourceRequest req = Resources.LoadAsync(path, typeof(T));

        return req;
    }
}

public static class JSONReader
{
    public static Task<T> DeserializeJson<T>(this string data, JsonSerializerSettings settings = null)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
        return stream.DeserializeJson<T>(settings);
    }

    public static Task<T> DeserializeJson<T>(this Stream stream, JsonSerializerSettings settings = null)
    {
        return Task.Run(() =>
        {
            using (stream)
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.Create(settings);
                return serializer.Deserialize<T>(jsonReader);
            }
        });
    }
}