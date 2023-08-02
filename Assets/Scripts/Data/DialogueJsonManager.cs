using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


public class DialogueJsonManager
{
    public void SaveJson(SaveDialogueClass saveDialogue) // 데이터를 저장하는 함수
    {
        string jsonText;
        string savePath = Application.dataPath + "/Data/DialogueData.json";

#if UNITY_EDITOR_WIN

#endif
#if UNITY_ANDROID
        savePath = Application.persistentDataPath + "/Data/DialogueData.json";
#endif
        jsonText = JsonUtility.ToJson(saveDialogue, true);
        FileStream fileStream = new FileStream(savePath, FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    public SaveDialogueClass LoadsaveDialogue()
    {
        SaveDialogueClass loadDialogue;
        string loadPath = Application.dataPath + "/Data/DialogueData.json";

#if UNITY_EDITOR_WIN

#endif
#if UNITY_ANDROID
        loadPath = Application.persistentDataPath + "/Data/DialogueData.json";
#endif
        if (File.Exists(loadPath))
        {
            FileStream stream = new FileStream(loadPath, FileMode.Open);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            string jsonData = Encoding.UTF8.GetString(bytes);
            loadDialogue = JsonUtility.FromJson<SaveDialogueClass>(jsonData);
        }
        else
        {
            loadDialogue = new SaveDialogueClass();
        }

        return loadDialogue;
    }
}
