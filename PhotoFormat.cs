using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProgram
{
    public enum PhotoFormat
    {
        Все,
        JPG,
        PNG,
        JPEG,
        BMP,
        GIF,
        TIFF
    }

    public static class PhotoFormatHelper
    {
        public static string GetExtension(PhotoFormat format)
        {
            switch (format)
            {
                case PhotoFormat.JPG:
                    return ".jpg";
                case PhotoFormat.PNG:
                    return ".png";
                case PhotoFormat.JPEG:
                    return ".jpeg";
                case PhotoFormat.BMP:
                    return ".bmp";
                case PhotoFormat.GIF:
                    return ".gif";
                case PhotoFormat.TIFF:
                    return ".tiff";
                default:
                    return "";
            }
        }

        public static string GetFilter(PhotoFormat format)
        {
            switch (format)
            {
                case PhotoFormat.JPG:
                    return "JPG файлы (*.jpg)|*.jpg";
                case PhotoFormat.PNG:
                    return "PNG файлы (*.png)|*.png";
                case PhotoFormat.JPEG:
                    return "JPEG файлы (*.jpeg)|*.jpeg";
                case PhotoFormat.BMP:
                    return "BMP файлы (*.bmp)|*.bmp";
                case PhotoFormat.GIF:
                    return "GIF файлы (*.gif)|*.gif";
                case PhotoFormat.TIFF:
                    return "TIFF файлы (*.tiff)|*.tiff";
                default:
                    return "Все изображения (*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
            }
        }

        public static List<string> GetAllExtensions()
        {
            return new List<string> { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };
        }

        public static PhotoFormat GetFormatFromExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return PhotoFormat.JPG;
                case ".jpeg":
                    return PhotoFormat.JPEG;
                case ".png":
                    return PhotoFormat.PNG;
                case ".bmp":
                    return PhotoFormat.BMP;
                case ".gif":
                    return PhotoFormat.GIF;
                case ".tiff":
                    return PhotoFormat.TIFF;
                default:
                    return PhotoFormat.Все;
            }
        }
    }
}