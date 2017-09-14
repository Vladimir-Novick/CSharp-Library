using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
namespace SGcombo.Utils
{

////////////////////////////////////////////////////////////////////////////
//	Copyright 2015 - 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Library
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
 
    
   /// <summary>
   /// Compresses/Decompress serializablr objects.
   /// </summary>    
    public class ObjectCompressor
    {

        public static byte[] Compress(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            using (GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(zs, obj);
                return ms.ToArray();
            }
          
        }


        public static T Decompress<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, true))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(zs);
            }
        }


        public static byte[] Compress(byte[] data)
        {
         

            using (MemoryStream output = new MemoryStream())
            {
                using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    dstream.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
            
        }

        public static byte[] Decompress(byte[] data)
        {
          

            using (MemoryStream input = new MemoryStream(data))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        dstream.CopyTo(output);
                    }
                   return output.ToArray();
                }
            }

           
        }


    }
}
