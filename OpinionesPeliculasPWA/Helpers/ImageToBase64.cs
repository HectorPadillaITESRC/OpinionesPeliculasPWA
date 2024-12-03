namespace OpinionesPeliculasPWA.Helpers
{
    public class ImageToBase64
    {
        public static string ConvertBase64(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                byte[] imageArray = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageArray);
            }
            return "";
        }
    }
}
