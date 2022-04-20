using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace sync_swagger.Service
{
    public static class CustomAes256
    {
        #region Конвертация из Byte в Hex String

        private static string ByteArrayToString(IReadOnlyCollection<byte> ba)
        {
            StringBuilder hex = new(ba.Count * 2);
            foreach (var b in ba)
            {
                _ = hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        #endregion

        #region Кодирование из String в Hex

        private static byte[] StringToByteArray(string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
        #endregion

        #region  Кодирование данных Формата IV : Данные
        public static string Encrypt(string plainText, string key)
        {
            // Проверяем аргуметы 
            if (plainText is not { Length: > 0 })
            {
                throw new ArgumentNullException("Текст кодировки является пустым");
            }

            if (key is not { Length: > 0 })
            {
                throw new ArgumentNullException("Ключ кодировки является пустым");
            }

            byte[] encrypted;
            byte[] iv;
            // Создайте объект RijndaelManaged
            // с указанным ключом и IV. 
            using (RijndaelManaged rijAlg = new())
            {
                //Записываем ключ в байтах
                rijAlg.Key = Encoding.UTF8.GetBytes(key);
                //Генерируем массив вектора в байтах
                rijAlg.GenerateIV();
                iv = rijAlg.IV;

                // TransformFinalBlock - это специальная функция для преобразования последнего блока или частичного блока в потоке.
                // Возвращает новый массив, содержащий оставшиеся преобразованные байты. Возвращается новый массив, потому что количество
                // информация, возвращаемая в конце, может быть больше одного блока при добавлении заполнения.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Создаём поток для записи шифрования. 
                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    // Записываем все данные в поток
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
            // Возвращаем данные в строке
            return ByteArrayToString(iv) + ":" + ByteArrayToString(encrypted);

        }
        #endregion
    }
}
