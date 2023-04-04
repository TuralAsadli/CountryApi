namespace CountryInfoApi.Utilites.FiIeExtentions
{
    public static class ImgExtention
    {
        public static string RenameImg(this IFormFile file)
        {
            return Guid.NewGuid().ToString() + file.FileName;
        }

        public static void CreateImgFile(this IFormFile file, string path)
        {
            if (file == null)
            {
                return;
            }


            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }


        public static void DeleteImgFile(string path)
        {
            if (path == null)
            {
                return;
            }
            if (!File.Exists(path))
            {
                return;
            }


            File.Delete(path);
        }

        public static bool CheckImgFileType(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }
    }
}
