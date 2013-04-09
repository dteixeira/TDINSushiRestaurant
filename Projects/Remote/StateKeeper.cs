using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

[Serializable]
public class StateKeeper
{
    private static readonly string _configFile = @"config/Config.xml";
    private static string _masterBackupFile;
    private static string _stateDirectory;
    private static string _tempBackupFile;
    private object _fileLock = new object();
    private int _savedDeliveryTeamId;
    private long _savedId;
    private List<Order> _savedList;

    public StateKeeper()
    {
    }

    public int SavedDeliveryTeamId
    {
        get { return _savedDeliveryTeamId; }
        set { _savedDeliveryTeamId = value; }
    }

    public long SavedId
    {
        get { return _savedId; }
        set { _savedId = value; }
    }

    public List<Order> SavedList
    {
        get { return _savedList; }
        set { _savedList = value; }
    }

    public static void ConfigureStateKeeper()
    {
        XDocument doc = XDocument.Load(_configFile);
        XElement elem = doc.Descendants("remotestate").First();
        _masterBackupFile = elem.Attribute("primary").Value;
        _tempBackupFile = elem.Attribute("temporary").Value;
        _stateDirectory = elem.Attribute("directory").Value;
    }

    public void LoadState()
    {
        CreateBackupDirectory();
        if (File.Exists(Path.Combine(_stateDirectory, _masterBackupFile)))
        {
            lock (_fileLock)
            {
                string filename = Path.Combine(_stateDirectory, _masterBackupFile);
                using (var fileStream = new FileStream(filename, FileMode.Open))
                {
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    StateKeeper keeper = (StateKeeper)bFormatter.Deserialize(fileStream);
                    _savedId = keeper.SavedId;
                    _savedList = keeper.SavedList;
                    _savedDeliveryTeamId = keeper.SavedDeliveryTeamId;
                }
            }
        }
        else
        {
            _savedList = new List<Order>();
            _savedId = 1;
            _savedDeliveryTeamId = 1;
        }
    }

    public void SaveState()
    {
        CreateBackupDirectory();
        string masterFilename = Path.Combine(_stateDirectory, _masterBackupFile);
        string tempFilename = Path.Combine(_stateDirectory, _tempBackupFile);
        lock (_fileLock)
        {
            using (var fileStream = new FileStream(tempFilename, FileMode.OpenOrCreate))
            {
                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(fileStream, this);
            }
            File.Copy(tempFilename, masterFilename, true);
        }
    }

    private void CreateBackupDirectory()
    {
        lock (_fileLock)
        {
            // Creates hidden directory for data backups
            if (!Directory.Exists(_stateDirectory))
            {
                DirectoryInfo info = Directory.CreateDirectory(_stateDirectory);
                info.Attributes |= FileAttributes.Directory | FileAttributes.Hidden;
            }
        }
    }
}