using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMQDataWareHouse
{
    public class LogService
    {
        string path = @"./../DataBase";
        string now = DateTime.Now.ToString("dd-MM-yyyy");
        public void AddLog(string data)
        {
       
            path = $"{path}/{now}.txt";
            File.Create(path).Dispose();
            StreamWriter writer = File.AppendText(path);
            writer.Write(data);
            writer.Close();
        }
        public void UpdateLog(string data) 
        {
            path = $"{path}/{now}.txt";
            StreamWriter writer = File.AppendText(path);
            writer.WriteLine("------------------------------------------------------");
            writer.WriteLine(data);
            writer.Close();
        }

        public bool CheckLog()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = $"{path}/{now}.txt";
            if (!File.Exists(path))
            {
                return false;
            }
            return true;
        }
    }
}
