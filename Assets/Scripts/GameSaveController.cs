using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameSaveController
{
    private const string FILE = "/savedGamesHex.gd";

    public static List<GameSaveBean> savedGames = new List<GameSaveBean>();
    

    public static void Save(string stageName, int ampli)
    {
        GameSaveController.Load();
        bool encontrou = false;
        FileStream file = null;

        for (int i = 0; i < GameSaveController.savedGames.Count; i++)
        {
            if(GameSaveController.savedGames[i].getStageName() == stageName)
            {
                encontrou = true;
                if(GameSaveController.savedGames[i].getIntAmpli() < ampli)
                {
                    GameSaveController.savedGames[i] = new GameSaveBean(stageName, ampli);
                    file = File.Open(Application.persistentDataPath + FILE, FileMode.Open);
                }
                break;
            }
        }
        if (!encontrou)
        {
            GameSaveController.savedGames.Add(new GameSaveBean(stageName, ampli));
            file = File.Create(Application.persistentDataPath + FILE);
        }
        if(file != null)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, GameSaveController.savedGames);
            file.Close();
        }
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + FILE))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + FILE, FileMode.Open);
            GameSaveController.savedGames = (List<GameSaveBean>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static int getByStageName(string nStageName)
    {
        GameSaveBean ampli = GameSaveController.savedGames.Find(x => x.getStageName() == nStageName);
        if (ampli != null) return ampli.getIntAmpli();
        return -1;
    }

    public static int contAmpli(string name, int tam)
    {
        int cont = 0;
        for (int j = 1; j <= tam; j++)
        {
            GameSaveBean ampli = GameSaveController.savedGames.Find(x => x.getStageName() == name+"-"+j);
            if (ampli != null)
            {
                cont += ampli.getIntAmpli();
            }
        }
        return cont;
    }

    public static bool isStageCompleted(string name, int tam)
    {
        return GameSaveController.savedGames.Exists(x => x.getStageName() == name + "-" + tam);
    }

    [System.Serializable]
    public class GameSaveBean
    {
        private string stageName;
        private int ampli;

        public GameSaveBean(string stageName, int ampli)
        {
            this.stageName = stageName;
            this.ampli = ampli;
        }

        public string getStageName()
        {
            return this.stageName;
        }

        public int getIntAmpli()
        {
            return this.ampli;
        }
    }
}
