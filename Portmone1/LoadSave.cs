using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Portmone1
{
    public class LoadSave
    {
        #pragma warning disable SYSLIB0011
        public object Data;
        public int X;
        public int y;
        public bool Delkey;
        public bool NoSave = true;
        public LoadSave() { }
        public LoadSave(object data, int x, bool delkey) { Data = data; X = x; Delkey = delkey; }        
        string path = @".\save.bin";
        BinaryFormatter format = new BinaryFormatter();
        
        public void Save() 
        { if (!Delkey)
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                { format.Serialize(fs, X); format.Serialize(fs, Data); }
            }
            else  { File.Delete(path); }
        }
        public object Load() 
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                NoSave = false;
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    y = (int)format.Deserialize(fs);

                    object UserLoaded = format.Deserialize(fs);
                    object[][] User2 = new object[y][];
                    User2 = (object[][])UserLoaded;

                    return User2;                  
                }                
            }
            else   { y = 0; return y; }              
           }
        }        
    }


