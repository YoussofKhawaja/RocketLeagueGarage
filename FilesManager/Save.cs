using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace RocketLeagueGarage.FilesManager
{
    public static class Save
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/RocketGarage/";

        public static void WriteToXmlFile<T>(T objectToWrite, string folderName, string fileName, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                CreateDir(path + folderName);
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(path + folderName + "\\" + RemoveSpecialCharacters(fileName) + ".xml", append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string folderName, string fileName) where T : new()
        {
            if (CheckFileExist(path + folderName + "\\" + RemoveSpecialCharacters(fileName) + ".xml"))
            {
                TextReader reader = null;
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    reader = new StreamReader(path + folderName + "\\" + RemoveSpecialCharacters(fileName) + ".xml");
                    return (T)serializer.Deserialize(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            else
            {
                return new T();
            }
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool CheckFileExist(string userID, string folderName, string _fileExt)
        {
            if (File.Exists(path + folderName + "\\" + RemoveSpecialCharacters(userID) + _fileExt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckFileExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            else
            {
                Console.WriteLine(filepath + " File doesn't exist!");
            }
        }

        public static void CreateDir(string path)
        {
            bool exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);
        }
    }
}