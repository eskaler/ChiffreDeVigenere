using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiffreDeVigenereApp
{
    public static class Vigenere
    {

        private static Dictionary<char, int> alphabet;          //алфавит в виде словаря
        private static string alphabetFull, alphabetFullLower;  //алфавит в виде строки в обоих регистрах
        private static int n;                                   //количество символов алфавита

        /// <summary>
        /// Установка алфавита для шифрования
        /// </summary>
        /// <param name="language">Порядковый номер языка. 0 - Русский, 1 - Английский</param>
        public static void SetAlphabet(int language)
        {
            if (language == 0)
            {
                alphabet = new Dictionary<char, int>{
                    { 'А', 0}, { 'Б', 1}, { 'В', 2}, { 'Г', 3}, { 'Д', 4}, { 'Е', 5}, { 'Ё', 6}, { 'Ж', 7}, { 'З', 8},
                    { 'И', 9}, { 'Й', 10}, { 'К', 11}, { 'Л', 12}, { 'М', 13}, { 'Н', 14}, { 'О', 15}, { 'П', 16}, { 'Р', 17},
                    { 'С', 18}, { 'Т', 19}, { 'У', 20}, { 'Ф', 21}, { 'Х', 22}, { 'Ц', 23}, { 'Ч', 24}, { 'Ш', 25}, { 'Щ', 26},
                    { 'Ъ', 27}, { 'Ы', 28}, { 'Ь', 29}, { 'Э', 30}, { 'Ю', 31}, { 'Я', 32}
                };
                alphabetFull = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                alphabetFullLower = alphabetFull.ToLower();
                n = 33;
            }
            else if (language == 1)
            {

                alphabet = new Dictionary<char, int>
                {
                    { 'A', 0}, {'B', 1}, {'C', 2}, {'D', 3}, {'E', 4}, {'F', 5}, {'G', 6}, {'H', 7}, {'I', 8},
                    { 'J', 9}, {'K', 10}, {'L', 11}, {'M', 12}, {'N', 13}, {'O', 14}, {'P', 15}, {'Q', 16}, {'R', 17},
                    { 'S', 18}, {'T', 19}, {'U', 20}, {'V', 21}, {'W', 22}, {'X', 23}, {'Y', 24}, {'Z', 25}
                };
                alphabetFull = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                alphabetFullLower = alphabetFull.ToLower();
                n = 26;
            }
        }

        
        /// <summary>
        /// Шифрование
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="key">Ключ</param>
        /// <returns>Зашифрованный текст</returns>
        public static string Encode(string input, string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";

            key = key.Replace(" ", "").ToUpper();

            int currentKeySymbol = 0;   //индекс символа ключа
            StringBuilder output = new StringBuilder();
            
            foreach (var symbol in input)
            {
                //является ли символ буквой выбранного алфавита
                if (alphabetFull.IndexOf(symbol) > -1 || alphabetFullLower.IndexOf(symbol) > -1)
                {
                    //для шифрования применяется формула c = (m + k) mod n

                    //если символ в верхнем регистре
                    if (alphabetFull.IndexOf(symbol) > -1)
                    {
                        output.Append(
                            alphabetFull.ElementAt(SumModuleN(
                                alphabet[symbol], 
                                alphabet[key[currentKeySymbol]])));
                    }
                    else
                    {
                        //если символ в нижнем регистре
                        output.Append(
                            alphabetFullLower.ElementAt(SumModuleN(
                                alphabet[Convert.ToChar(symbol.ToString().ToUpper())],
                                alphabet[key[currentKeySymbol]])));
                    }

                    //переключение символов ключа
                    currentKeySymbol++;
                    if (currentKeySymbol == key.Length)
                        currentKeySymbol = 0;
                }
                else
                {
                    output.Append(symbol);
                }
            }
            return output.ToString();

        }

        /// <summary>
        /// Дешифрование
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="key">Ключ</param>
        /// <returns>Дешифрованный текст</returns>
        public static string Decode(string input, string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            key = key.Replace(" ", "").ToUpper();

            int currentKeySymbol = 0;
            StringBuilder output = new StringBuilder();

            foreach (var symbol in input)
            {
                //является ли символ буквой выбранного алфавита
                if (alphabetFull.IndexOf(symbol) > -1 || alphabetFullLower.IndexOf(symbol) > -1) 
                {

                    //для дешифрования применяется формула m = (c + n - k) mod n

                    //если символ в верхнем регистре
                    if (alphabetFull.IndexOf(symbol) > -1)
                    {
                        output.Append(alphabetFull.ElementAt(DiffModuleN(
                            alphabet[symbol], 
                            alphabet[key[currentKeySymbol]])));
                    }
                    else
                    {
                        //если символ в нижнем регистре
                        output.Append(alphabetFullLower.ElementAt(DiffModuleN(
                            alphabet[Convert.ToChar(symbol.ToString().ToUpper())], 
                            alphabet[key[currentKeySymbol]])));
                    }              

                    //переключение символов ключа
                    currentKeySymbol++;
                    if (currentKeySymbol == key.Length)
                        currentKeySymbol = 0;
                }
                else
                {
                    output.Append(symbol);
                }
            }

            return output.ToString();

        }

        /// <summary>
        /// Сумма по модулю N
        /// </summary>
        /// <param name="m">Символ исходного текста</param>
        /// <param name="k">Символ ключа</param>
        /// <returns>Порядковый номер символа зашифрованного текста</returns>
        private static int SumModuleN(int m, int k) => Mod(m + k, n);

        /// <summary>
        /// Разность по модулю N
        /// </summary>
        /// <param name="c">Символ зашифрованного текста</param>
        /// <param name="k">Символ ключа</param>
        /// <returns>Порядковый номер символа исходного текста</returns>
        private static int DiffModuleN(int c, int k) => Mod(c + n - k, n);

        /// <summary>
        /// Реализация операции деления с остатком
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Результат операции деления с остатком</returns>
        private static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

    }
}
