using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveMetrics
{
    private static string path = Application.persistentDataPath + "/metrics.met";

    public static void SavePlayMetrics(GameSession gameSession) {
        BinaryFormatter bFormatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        GameMetrics data = new GameMetrics(gameSession);

        bFormatter.Serialize(stream, data);

        stream.Close();
    }

    public static GameMetrics LoadMetrics() {
        if (File.Exists(path)) {
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameMetrics data = bFormatter.Deserialize(stream) as GameMetrics;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("NO file");
            return null;
        }
    }
}
