using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class DataManager : MonoBehaviour, IManager
{
    private string _state;
    // File Paths
    private string _dataPath;
    private string _TextFile;
    private string _StreamingTextFile;
    private string _xmlLevelProgress;
    private string _XmlWeapons;
    private string _JSONWeapons;
    //
    private List<Weapon> weaponInventory = new List<Weapon>
    {
        new Weapon("Sword of Doom", 100),
        new Weapon("Butterfly knives", 25),
        new Weapon("Brass Knuckles", 15),
    };
    public string State
    { 
        get { return _state; } 
        set { _state = value; }
    }
    void Start()
    {
        Initialize();
    }
    void Awake()
    {
        _dataPath = Application.persistentDataPath+ "/Player_Data/";
        Debug.Log(_dataPath);
        _TextFile = _dataPath + "Save_Data.txt";
        _StreamingTextFile = _dataPath + "Streaming_Save_Data.txt";
        _xmlLevelProgress = _dataPath + "Progess_Data.xml";
        _XmlWeapons = _dataPath + "WeaponInventory.xml";
        _JSONWeapons = _dataPath + "WeaponJSON.json";
    }
  
    public void Initialize()
    {
        _state = "Dta Manager Initialized..";
        //  Debug.Log(_state);
       // FilesystemInfo();
        NewDirectory();
        WriteToStream(_StreamingTextFile);
        ReadFromStream(_StreamingTextFile);
        WriteToXml(_xmlLevelProgress);
        SerializeXML();
       // DeSerializeXML();
        SerializeJSON();
        DeSerializeJSON();
       // ReadFromFile(_xmlLevelProgress);
    }

    public void SerializeJSON()
    {
        WeaponShop shop = new WeaponShop();
        shop.inventory = weaponInventory;
        string jsonString = JsonUtility.ToJson(shop, true);
        using(StreamWriter stream = File.CreateText(_JSONWeapons))
        {
            stream.WriteLine(jsonString);
        }
    }
    public void DeSerializeJSON()
    {
        if (File.Exists(_JSONWeapons))
        {
            using( StreamReader stream= new StreamReader(_JSONWeapons) )
            {
                var JSONstring = stream.ReadToEnd();
                var weaponData = JsonUtility.FromJson<WeaponShop>(JSONstring);
                foreach(var weapon in weaponData.inventory)
                {
                    Debug.LogFormat("Weapon: {0} - Damage:{1}", weapon.name, weapon.damage);
                }
            }
        }
    }

    public void SerializeXML()
    {
        var XMLSerializer = new XmlSerializer(typeof(List<Weapon>));
        using(FileStream stream = File.Create(_XmlWeapons))
        {
            XMLSerializer.Serialize(stream, weaponInventory);
        }
    }
    public void DeSerializeXML()
    {
        if (File.Exists(_XmlWeapons))
        {
            var xmlSerializer= new XmlSerializer(typeof(List<Weapon>));
            using(FileStream stream = File.OpenRead(_XmlWeapons))
            {
                var weapons = (List<Weapon>)xmlSerializer.Deserialize(stream);
                foreach (var weapon in weapons)
                {
                    Debug.LogFormat("Weapon : {0} - Damage : {1}", weapon.name, weapon.damage);
                }
            }
        }
    }
    public void WriteToXml(string filename)
    {
        if(!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);

            XmlWriter xmlWriter= XmlWriter.Create(xmlStream);
       
            xmlWriter.WriteStartDocument();
            
            xmlWriter.WriteStartElement("level_progess");

            for(int i= 1; i<5; i++)
            {
                xmlWriter.WriteElementString("level", "Level-" + i);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
        }
    }
    public void WriteToStream(string filename)
    {
        if(!File.Exists(filename))
        {
            StreamWriter newStream= File.CreateText(filename);
            newStream.WriteLine("<Save Data> for HERO BORN\n");
            newStream.Close();
            Debug.Log("New file created with StreamWriter");
        }
        StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.WriteLine("Game ended: " + DateTime.Now);
        streamWriter.Close();
        Debug.Log("File contents updated with StreamWriter");
    }
    public void ReadFromStream(string filename)
    {
        if(!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }
        StreamReader streamReader = new StreamReader(filename);
        Debug.Log(streamReader.ReadToEnd());
    }
    public void ReadFromFile(string fileName)
    {
        if(!File.Exists(fileName))
        {
            Debug.Log("File doesn't exist");
            return;
        }
        Debug.Log(File.ReadAllText(fileName));
    }
    public void UpdateTextFile()
    {
        if(!File.Exists(_TextFile))
        {
            Debug.Log("The text file doesn't exist or has already been deleted");
            return;
        }
        File.AppendAllText(_TextFile, $"Game started: {DateTime.Now}\n");
        Debug.Log("File updated successfully");
    }
    public void NewTextFile()
    {
        if (File.Exists(_TextFile))
        {
            Debug.Log("File already exists... ");
            return;
        }
        File.WriteAllText(_TextFile, "<SAVE DATA>\n");
        Debug.Log("New file created!");
    }
    public void FilesystemInfo()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}", Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());

    }
    public void NewDirectory()
    {
        if(Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists");
            return;
        }
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created");
    }
    public void DeleteDirectory()
    {
        if(!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist or has already been deleted ... ");
            return;
        }
        Directory.Delete(_dataPath, true);
        Debug.Log("Directory has been successfully deleted");

    }


}
