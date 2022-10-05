using System.Security.Cryptography;
using System.Text;
using SNShop.Models;
namespace SNShop.DAO
{
    public class Encode
    {
        SNOnlineShopDataContext db = new SNOnlineShopDataContext();
        public static string GetMD5(string str)
        {

            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(str));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();

        }
        
    }
}