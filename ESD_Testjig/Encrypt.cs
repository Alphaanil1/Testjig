using System;
using System.Security.Cryptography;
using System.Text;

namespace ESD_Testjig
{
    internal class Encrypt : IDisposable
    {
        #region Private/Protected Member Variables
        
        /// <summary>
        /// Decryptor
        /// 
        private readonly ICryptoTransform _decryptor;

        /// <summary>
        /// Encryptor
        /// 
        private readonly ICryptoTransform _encryptor;

        /// <summary>
        /// 16-byte Private Key
        /// 
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("ALPHAICTLLPALPHA");

        /// <summary>
        /// Public Key
        /// 
        private readonly byte[] _password;

        /// <summary>
        /// Rijndael cipher algorithm
        /// 
        private readonly RijndaelManaged _cipher;

        #endregion

        #region Private/Protected Properties

        private ICryptoTransform Decryptor { get { return _decryptor; } }
        private ICryptoTransform Encryptor { get { return _encryptor; } }

        #endregion
        #region Private/Protected Methods
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// 
        /// <param name="password">Public key
        /// 

        private readonly MD5 md5;
        public Encrypt(string password)
        {
            //Encode digest
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
            md5 = new MD5CryptoServiceProvider();
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
            try
            {
                _password = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

                //Initialize objects
                _cipher = new RijndaelManaged();
                _decryptor = _cipher.CreateDecryptor(_password, IV);
                _encryptor = _cipher.CreateEncryptor(_password, IV);
            }
            catch
            {
                throw;
            }
            
        }

        #endregion
       
        #region Public Properties
        #endregion

        #region Public Methods

        /// <summary>
        /// Decryptor
        /// 
        /// <param name="text">Base64 string to be decrypted
        /// <returns>
        public string DecryptString(string text)
        {
            try
            {
                byte[] input = Convert.FromBase64String(text);                
                var newClearData = Decryptor.TransformFinalBlock(input, 0, input.Length);
                return Encoding.ASCII.GetString(newClearData);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("inputCount uses an invalid value or inputBuffer has an invalid offset length. " + ae);
                return null;
            }
            catch (ObjectDisposedException oe)
            {
                Console.WriteLine("The object has already been disposed." + oe);
                return null;
            }
        }

        /// <summary>
        /// Encryptor
        /// 
        /// <param name="text">String to be encrypted
        /// <returns>
        public string EncryptString(string text)
        {
            try
            {
                var buffer = Encoding.ASCII.GetBytes(text);
                return Convert.ToBase64String(Encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("inputCount uses an invalid value or inputBuffer has an invalid offset length. " + ae);
                return null;
            }
            catch (ObjectDisposedException oe)
            {
                Console.WriteLine("The object has already been disposed." + oe);
                return null;
            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    md5.Dispose();
                    _cipher.Dispose();
                    _decryptor.Dispose();
                    _encryptor.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Encrypt()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
             GC.SuppressFinalize(this);
        }
        #endregion






        //public void GenerateNew(string FileName, LicenseData ld)
        //{
        //    StringBuilder logAction = new StringBuilder();
        //    using (StreamWriter streamWriter = new StreamWriter(FileName))
        //    {
        //        string str1 = this.Encrypt(ld.MacAddress);
        //        streamWriter.WriteLine(str1);

        //        //string checkDecrypt = this.Decrypt("rAznA5y0WB3GtfTEvu7SX5v9g6Q8re7CvhiZCqlHeHjbh2C3IPU9u0ximfQfiGrE");

        //        logAction.AppendLine(str1);
        //        string str2 = this.Encrypt(ld.LicenseDate.ToString());
        //        streamWriter.WriteLine(str2);
        //        logAction.AppendLine(str2);
        //        string str3 = this.Encrypt(ld.Validity.ToString());
        //        streamWriter.WriteLine(str3);
        //        logAction.AppendLine(str3);
        //        string str33 = this.Encrypt("User  Username  Password  ManualPanel  Diagnostics ConfigurationDownload");
        //        streamWriter.WriteLine(str33);

        //        byte[] hashBytes = Encoding.UTF8.GetBytes("User1");
        //        SHA1 sha1 = SHA1Managed.Create();
        //        byte[] cryptoPwd = sha1.ComputeHash(hashBytes);
        //        string decrypted = Encoding.Default.GetString(cryptoPwd);

        //        string str4 = this.Encrypt("User1 ") + "*" + this.Encrypt(ld.username1.ToString()) + "*" + this.Encrypt(ld.password1.ToString()) + "*" + this.Encrypt(ld.isManualPanel1.ToString()) + "*" + this.Encrypt(ld.isDiagnostics1.ToString()) + "*" + this.Encrypt(ld.isConfigurationDownload1.ToString());
        //        streamWriter.WriteLine(str4);
        //        logAction.AppendLine(str4);
        //        string str5 = this.Encrypt("User2 ") + "*" + this.Encrypt(ld.username2.ToString()) + "*" + this.Encrypt(ld.password2.ToString()) + "*" + this.Encrypt(ld.isManualPanel2.ToString()) + "*" + this.Encrypt(ld.isDiagnostics2.ToString()) + "*" + this.Encrypt(ld.isConfigurationDownload2.ToString());
        //        streamWriter.WriteLine(str5);
        //        logAction.AppendLine(str5);
        //        string str6 = this.Encrypt("User3 ") + "*" + this.Encrypt(ld.username3.ToString()) + "*" + this.Encrypt(ld.password3.ToString()) + "*" + this.Encrypt(ld.isManualPanel3.ToString()) + "*" + this.Encrypt(ld.isDiagnostics3.ToString()) + "*" + this.Encrypt(ld.isConfigurationDownload3.ToString());
        //        streamWriter.WriteLine(str6);
        //        logAction.AppendLine(str6);
        //        string str7 = this.Encrypt("User4 ") + "*" + this.Encrypt(ld.username4.ToString()) + "*" + this.Encrypt(ld.password4.ToString()) + "*" + this.Encrypt(ld.isManualPanel4.ToString()) + "*" + this.Encrypt(ld.isDiagnostics4.ToString()) + "*" + this.Encrypt(ld.isConfigurationDownload4.ToString());
        //        streamWriter.WriteLine(str7);
        //        logAction.AppendLine(str7);
        //        string str8 = this.Encrypt("User5 ") + "*" + this.Encrypt(ld.username5.ToString()) + "*" + this.Encrypt(ld.password5.ToString()) + "*" + this.Encrypt(ld.isManualPanel5.ToString()) + "*" + this.Encrypt(ld.isDiagnostics5.ToString()) + "*" + this.Encrypt(ld.isConfigurationDownload5.ToString());
        //        streamWriter.WriteLine(str8);
        //        logAction.AppendLine(str8);

        //        string acrualtAction = logAction.ToString();

        //        //insert to log table
        //        frm.UserLog(Convert.ToInt32(GlobaluserID.sessionuserID), acrualtAction, System.DateTime.Now);
        //    }
        //}

        #endregion



        //public static string DecryptString(string cipherText, string passPhrase)
        //{
        //    byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        //    byte[] keyBytes = password.GetBytes(keysize / 8);
        //    RijndaelManaged symmetricKey = new RijndaelManaged();            
        //    symmetricKey.Mode = CipherMode.CBC;
        //    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        //    MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        //    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        //    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        //    memoryStream.Close();
        //    cryptoStream.Close();
        //    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        //}


    }
}
