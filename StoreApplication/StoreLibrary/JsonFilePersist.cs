using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace StoreLibrary
{
    /// <summary>
    /// persistent data handling
    /// </summary>
    public class JsonFilePersist
    {
        private readonly string path;
        /// <summary>
        /// default constructor
        /// </summary>
        public JsonFilePersist() { }

        public JsonFilePersist(string cpath)
        {
            path = cpath;
        }

        /// <summary>
        /// original behavior to serialize and write data
        /// </summary>
        public void WriteStoreData(CStore data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data, typeof(CStore));
            }

            /*
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(path,json);
            */


        }

        /// <summary>
        /// original behavior to read data and deserialize
        /// have jsonignore on model order class to avoid object cycle
        /// </summary>
        public CStore ReadStoreData()
        {
            CStore data;
            try
            {
                string json = File.ReadAllText(path);
                /*
                // object cycle
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
                */
                data = JsonConvert.DeserializeObject<CStore>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                return new CStore();
            }
            return data;
        }

        //
        public string WriteProductsTempData(List<CProduct> data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            // 
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data, typeof(List<CProduct>));
            }

            // should at least contain one product, so try catch is not needed, but just for safety
            string json = "";
            try
            {
                json = File.ReadAllText(path);
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
            return json;

        }

        public List<CProduct> ReadProductsTempData(string json)
        {
            List<CProduct> data;
            try
            {
                data = JsonConvert.DeserializeObject<List<CProduct>>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                return new List<CProduct>();
            }
            return data;
        }



    }
}
