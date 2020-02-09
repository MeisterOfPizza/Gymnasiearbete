using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ArenaShooter.Data
{

    static class DataManager
    {

        #region Private constants

        private const string SAVE_FOLDER_PATH = "/ArenaShooter/savedata/";
        private const string SAVE_FILE_PATH   = "profile.dat";

        #endregion

        public static bool SaveProfile(ProfileData profileData)
        {
            try
            {
                string path = GetMyDocumentsPath();

                if (!string.IsNullOrWhiteSpace(path))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(path + SAVE_FOLDER_PATH);

                    if (directoryInfo.Exists)
                    {
                        using (FileStream stream = new FileStream(path + SAVE_FOLDER_PATH + SAVE_FILE_PATH, FileMode.OpenOrCreate))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, profileData);

                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("Could not find MyDocuments path on OS. Cannot save profile.");

                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("An error occured when trying to save the player profile: " + e.Message);

                throw new Exception("An error occured when trying to save the player profile.", e);
            }
        }

        public static ProfileData LoadProfile()
        {
            try
            {
                string path = GetMyDocumentsPath();

                if (!string.IsNullOrWhiteSpace(path))
                {
                    // Check so the file exists.
                    if (File.Exists(path + SAVE_FOLDER_PATH + SAVE_FILE_PATH) && Directory.Exists(path + SAVE_FOLDER_PATH))
                    {
                        // The file did exist, open and read it.
                        using (FileStream stream = new FileStream(path + SAVE_FOLDER_PATH + SAVE_FILE_PATH, FileMode.Open))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            return formatter.Deserialize(stream) as ProfileData;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Could not find MyDocuments path on OS. Cannot load profile.");
                }

            }
            catch (Exception e)
            {
                Debug.LogError("An error occured when trying to load the player profile: " + e.Message);

                throw new Exception("An error occured when trying to load the player profile.", e);
            }

            return null;
        }

        private static string GetMyDocumentsPath()
        {
            // TODO: Check if this works on OSX, Android and iOS as well.
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

    }

}
