﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Downloader.Shared.Operations
{
    public class FileEdit
    {
        public FileEdit() { }
        public string Read(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return null;
        }

        public string[] ReadAllLines(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            try
            {
                return File.ReadAllLines(path);
            }
            catch (Exception ex) { Console.WriteLine(ex); return null; }
        }
        public byte[] ReadAsBytes(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex) { Console.WriteLine(ex); return null; }
        }
        public string ReadAsJsonObject(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex) { Console.WriteLine(ex); return null; }
        }
        public bool Write(string path, string textToSave, bool overwrite)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (string.IsNullOrEmpty(textToSave)) return false;
            if (!File.Exists(path)) CreateFile(path);
            if (overwrite)
            {
                try
                {
                    File.WriteAllText(path, textToSave);
                    return true;
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            else
            {
                try
                {
                    //MessageBox.Show(path);
                    //MessageBox.Show(textToSave);
                    if (File.Exists(path))
                    {
                        File.AppendAllText(path, textToSave);
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex) { Console.WriteLine(ex + ""); }
            }
            return false;
        }
        public bool Write(string path, byte[] textToSave)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (textToSave == null) return false;
            if (!File.Exists(path)) CreateFile(path);
            try
            {
                //MessageBox.Show(path);
                //MessageBox.Show(textToSave);
                File.WriteAllBytes(path, textToSave);
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex + ""); }
            return false;
        }
        public bool Write(string path, string[] textToSave, bool overwrite)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (textToSave == null) return false;
            if (overwrite)
            {
                try
                {
                    foreach (string str in textToSave)
                    {
                        File.AppendAllText(path, str);
                    }
                    return true;
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            else
            {
                try
                {
                    File.WriteAllText(path, "");
                    foreach (string str in textToSave)
                    {
                        File.AppendAllText(path, str);
                    }
                    return true;
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            return false;
        }

        public bool WriteAsJson(string path, JsonObject jsonObject, bool overwrite)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (!File.Exists(path)) CreateFile(path);
            if (overwrite)
            {
                try
                {
                    File.WriteAllText(path, jsonObject + "");
                    return true;
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            else
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.AppendAllText(path, jsonObject + "");
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex) { Console.WriteLine(ex + ""); }
            }
            return false;
        }
        public void RenameFile(string path, string newName)
        {
            try
            {
                string direction = Directory.GetParent(path).FullName;
                string newPath = Path.Combine(direction, newName);
                File.Move(path, newPath);
            }
            catch (Exception ex) { Console.Write(ex); }
        }
        public string ChangePath(string oldPath, string newPath)
        {
            try
            {
                Directory.Move(oldPath, newPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return newPath;
        }
        public string CreateFile(string path, string name, bool overwrite)
        {
            if (overwrite)
            {
                path = FindAvailableFileName(path, name);
            }
            else
            {
                path = Path.Combine(path, name);
            }
            try
            {
                File.Create(path).Dispose();

            }
            catch (Exception ex) { Console.Write(ex); }
            return path;
        }
        public string CreateFile(string path)
        {
            try
            {
                File.Create(path).Dispose();

            }
            catch (Exception ex) { Console.Write(ex); }
            return path;
        }
        public string CreateFolder(string folderPath, string name)
        {
            string path = "";
            path = FindAvailableFileName(folderPath, name);
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return path;
        }
        public bool IsFileExist(string path)
        {
            return File.Exists(path);
        }
        public string FindAvailableFileName(string folderPath, string name)
        {
            int i = 0;
            string path = Path.Combine(folderPath, name + i);
            while (Directory.Exists(path))
            {
                i++;
                path = Path.Combine(folderPath, name + i);
            }
            return path;
        }
        public string FindAvailableFolderName(string folderPath, string name)
        {
            int i = 0;
            string path = Path.Combine(folderPath, name + i);
            while (Directory.Exists(path))
            {
                i++;
                path = Path.Combine(folderPath, name + i);
            }
            return path;
        }
        public FileInfo[] GetFilesInfoFromPath(string path)
        {
            if (!Directory.Exists(path)) return null;
            try
            {
                DirectoryInfo directoryInfo = new(path);
                FileInfo[] files = directoryInfo.GetFiles();
                return files;
            }
            catch (Exception ex) { Console.WriteLine(ex + ""); }
            return null;

        }
        public DirectoryInfo[] GetFoldersInfoFromPath(string path)
        {
            if (!Directory.Exists(path)) return null;
            try
            {
                DirectoryInfo directoryInfo = new(path);
                DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
                return directoryInfos;
            }
            catch (Exception ex) { Console.WriteLine(ex + ""); }
            return null;
        }
        public string[] GetAllFilesAndFolders(string path)
        {
            if (!Directory.Exists(path)) return null;
            try
            {
                string[] allFilesAndFolders = Directory.GetFileSystemEntries(path);
                return allFilesAndFolders;
            }
            catch (Exception ex) { Console.WriteLine(ex + ""); }
            return null;
        }
        public string[] GetDirectorys(string path)
        {
            if (!Directory.Exists(path)) return null;
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (Exception ex) { Console.WriteLine(ex + ""); }
            return null;
        }
    }
}
