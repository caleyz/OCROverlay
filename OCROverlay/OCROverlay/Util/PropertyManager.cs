using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OCROverlay.Util
{
    public class PropertyManager
    {
        public void SaveProperty(string property, object obj)
        {
            if (!obj.GetType().IsSerializable)
                throw new SerializationException();

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Properties.Settings.Default[property] = Convert.ToBase64String(buffer);
                Properties.Settings.Default.Save();
            }
        }

        public T GetDeserializedProperty<T>(string property)
        {
            T retVal = (T)Activator.CreateInstance(typeof(T));
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(property)))
            {
                if (ms.Length != 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    retVal = (T)bf.Deserialize(ms);
                }
            }
            return retVal;
        }
    }
}
