using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{

    public static void SavePlayer(Combat player, Movement playerMovement)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, playerMovement);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveOptions()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        OptionsData data = new OptionsData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static OptionsData LoadOptions()
    {
        string path = Application.persistentDataPath + "/options.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionsData data = formatter.Deserialize(stream) as OptionsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveVault()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/vault.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        VaultData data = new VaultData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static VaultData LoadVault()
    {
        string path = Application.persistentDataPath + "/vault.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            VaultData data = formatter.Deserialize(stream) as VaultData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveHeroes()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/heroes.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        HeroesData data = new HeroesData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static HeroesData LoadHeroes()
    {
        string path = Application.persistentDataPath + "/heroes.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HeroesData data = formatter.Deserialize(stream) as HeroesData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveCP(CapturePointManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/cp.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        CapturePointData data = new CapturePointData(manager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CapturePointData LoadCP()
    {
        string path = Application.persistentDataPath + "/cp.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CapturePointData data = formatter.Deserialize(stream) as CapturePointData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveGameTracking( )
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game_tracking.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameTrackingData data = new GameTrackingData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameTrackingData LoadGameTracking()
    {
        string path = Application.persistentDataPath + "/game_tracking.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameTrackingData data = formatter.Deserialize(stream) as GameTrackingData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveEnemies(CapturePointManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/enemies.nt";
        FileStream stream = new FileStream(path, FileMode.Create);

        EnemyData data = new EnemyData(manager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static EnemyData LoadEnemies()
    {
        string path = Application.persistentDataPath + "/enemies.nt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnemyData data = formatter.Deserialize(stream) as EnemyData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
