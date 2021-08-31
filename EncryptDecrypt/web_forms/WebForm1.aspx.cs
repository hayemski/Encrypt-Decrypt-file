using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace EncryptDecrypt
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string kljuc = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //public static byte[] EncryptDES(byte[] plain, String key)
        //{
        //    byte[] keyArray = SoapHexBinary.Parse(key).Value;

        //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //    tdes.Key = keyArray;
        //    tdes.Mode = CipherMode.CBC;
        //    tdes.Padding = PaddingMode.None;
        //    tdes.IV = new byte[8];

        //    ICryptoTransform cTransform = tdes.CreateEncryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(plain, 0, plain.Length);
        //    tdes.Clear();

        //    return resultArray;
        //}


        //public static byte[] DecryptDES(byte[] cipher, String key)
        //{
        //    byte[] keyArray = SoapHexBinary.Parse(key).Value;

        //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //    tdes.Key = keyArray;
        //    tdes.Mode = CipherMode.CBC;
        //    tdes.Padding = PaddingMode.None;
        //    tdes.IV = new byte[8];

        //    ICryptoTransform cTransform = tdes.CreateDecryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(cipher, 0, cipher.Length);
        //    tdes.Clear();

        //    return resultArray;
        //}






        protected void EncryptFile_Click(object sender, EventArgs e)
        {
            //Get the Input File Name and Extension.
            string fileName = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);

            //Build the File Path for the original (input) and the encrypted (output) file.
            string input = Server.MapPath("~/Files/") + fileName + fileExtension;
            string output = Server.MapPath("~/Files/") + fileName + "_Encrypted" + fileExtension;
            
            //Save the Input File, Encrypt it and save the encrypted file in output path.
            FileUpload1.SaveAs(input);
            kljuc = String.Format("{0}", Request.Form["kljuc"]);

            if (DropDownList1.SelectedItem.Value == "1")
            {
                this.EncryptAES(input, output, kljuc);
            }
            else
            {
                Init3DES(kljuc);
                EncryptFile3DES(input, output);
            }

            //Download the Encrypted File.
            Response.ContentType = FileUpload1.PostedFile.ContentType;
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(output));
            Response.WriteFile(output);
            Response.Flush();

            //Delete the original (input) and the encrypted (output) file.
            File.Delete(input);
            File.Delete(output);

            Response.End();
        }

        protected void DecryptFile_Click(object sender, EventArgs e)
        {
            //Get the Input File Name and Extension
            string fileName = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);

            //Build the File Path for the original (input) and the decrypted (output) file
            string input = Server.MapPath("~/Files/") + fileName + fileExtension;
            string output = Server.MapPath("~/Files/") + fileName + "_Decrypted" + fileExtension;

            //Save the Input File, Decrypt it and save the decrypted file in output path.
            FileUpload1.SaveAs(input);
            kljuc = String.Format("{0}", Request.Form["kljuc"]);


            if (DropDownList1.SelectedItem.Value == "1")
            {
                this.DecryptAES(input, output, kljuc);
            }
            else
            {
                Init3DES(kljuc);
                DecryptFile3DES(input, output);
            }

            //Download the Decrypted File.
            Response.Clear();
            Response.ContentType = FileUpload1.PostedFile.ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(output));
            Response.WriteFile(output);
            Response.Flush();

            //Delete the original (input) and the decrypted (output) file.
            File.Delete(input);
            File.Delete(output);

            Response.End();
        }

        private TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        public void Init3DES(string key)
        {
            des.Key = UTF8Encoding.UTF8.GetBytes(key);
            des.Mode = CipherMode.ECB; //Electronic Code Book
            des.Padding = PaddingMode.PKCS7;
        }

        public void EncryptFile3DES(string input, string output)
        {
            byte[] Bytes = File.ReadAllBytes(input);
            byte[] eBytes = des.CreateEncryptor().TransformFinalBlock(Bytes, 0, Bytes.Length);
            File.WriteAllBytes(output, eBytes);
        }

        public void DecryptFile3DES(string filepath, string output)
        {
            byte[] Bytes = File.ReadAllBytes(filepath);
            byte[] dBytes = des.CreateDecryptor().TransformFinalBlock(Bytes, 0, Bytes.Length);
            File.WriteAllBytes(output, dBytes);
        }

        private void EncryptAES(string inputFilePath, string outputfilePath, string kljuc)
        {
            string EncryptionKey = kljuc;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private void DecryptAES(string inputFilePath, string outputfilePath, string kljuc)
        {
            string EncryptionKey = kljuc;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }


        
    }


}