using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BCMS.Services
{
    public class Converter64ToImg
    {
        private readonly string filePath = "C:\\Users\\g.daian\\Desktop\\Project";
        void convert(string image)
        {
            var bytes = Convert.FromBase64String(image);
            using (var imageFIle = new FileStream(filePath, FileMode.Create))
            {
                imageFIle.Write(bytes, 0, bytes.Length);
                imageFIle.Flush();
            }
        }
    }
}