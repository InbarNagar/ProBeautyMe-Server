using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BeautyMe;

namespace BeautyMe
{
    public class Images
    {
        BeautyMeDBContext DB = new BeautyMeDBContext();
        public string CreateNewNameOrMakeItUniqe(string name,string id)
        {
            try
            {
                  
             return name + id;

            }
            catch (Exception)
            {
                return null;
            }


        }
        public string ImageFileExist(string Name, string rootPath)
        {

            string[] names = Directory.GetFiles(rootPath);

            foreach (var fileName in names)
            {
                if (Path.GetFileNameWithoutExtension(fileName).IndexOf(Path.GetFileNameWithoutExtension(Name)) != -1)
                {

                    return fileName;
                }
            }

            return null;
        }
       
    }
}