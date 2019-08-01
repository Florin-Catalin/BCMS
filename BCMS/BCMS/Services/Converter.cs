using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BCMS.Services
{
    public class Converter
    {
        readonly string filePath = "C:\\Users\\g.daian\\Desktop\\Project\\img.jpg";
        public void convert(string image)
        {
            var bytes = Convert.FromBase64String(image);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
        }
    }
}