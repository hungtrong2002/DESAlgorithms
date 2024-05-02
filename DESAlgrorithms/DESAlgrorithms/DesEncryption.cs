using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace DESAlgrorithms
{
    public class DesEncryption
    {
        // Initial Permutation Table
        private static int[] IP = new int[]
        {
        58, 50, 42, 34, 26, 18, 10, 2,
        60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6,
        64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17, 9, 1,
        59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5,
        63, 55, 47, 39, 31, 23, 15, 7
        };

        // Final Permutation Table
        private static int[] FP = new int[]
        {
        40, 8, 48, 16, 56, 24, 64, 32,
        39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30,
        37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28,
        35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26,
        33, 1, 41, 9, 49, 17, 57, 25
        };

        // Expansion Table
        private static int[] E = new int[]
        {
        32, 1, 2, 3, 4, 5,
        4, 5, 6, 7, 8, 9,
        8, 9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17,
        16, 17, 18, 19, 20, 21,
        20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29,
        28, 29, 30, 31, 32, 1
        };

        // Permutation Table
        private static int[] P = new int[]
        {
        16, 7, 20, 21,
        29, 12, 28, 17,
        1, 15, 23, 26,
        5, 18, 31, 10,
        2, 8, 24, 14,
        32, 27, 3, 9,
        19, 13, 30, 6,
        22, 11, 4, 25
        };

        // Permuted Choice 1 (PC1) Table
        private static int[] PC1 = new int[]
        {
        57, 49, 41, 33, 25, 17, 9,
        1, 58, 50, 42, 34, 26, 18,
        10, 2, 59, 51, 43, 35, 27,
        19, 11, 3, 60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15,
        7, 62, 54, 46, 38, 30, 22,
        14, 6, 61, 53, 45, 37, 29,
        21, 13, 5, 28, 20, 12, 4
        };

        // Permuted Choice 2 (PC2) Table
        private static int[] PC2 = new int[]
        {
        14, 17, 11, 24, 1, 5,
        3, 28, 15, 6, 21, 10,
        23, 19, 12, 4, 26, 8,
        16, 7, 27, 20, 13, 2,
        41, 52, 31, 37, 47, 55,
        30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53,
        46, 42, 50, 36, 29, 32
        };

        // Shifts for Key Schedule
        private static int[] shifts = new int[]
        {
        1, 1, 2, 2,
        2, 2, 2, 2,
        1, 2, 2, 2,
        2, 2, 2, 1
        };

        // S-boxes (Substitution boxes)
        private static int[][][] S_BOX = new int[][][]
        {
        // S1
        new int[][]
        {
            new int[] { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
            new int[] { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
            new int[] { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
            new int[] { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
        },
        // S2
        new int[][]
        {
            new int[] { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
            new int[] { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
            new int[] { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
            new int[] { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
        },
        // S3
        new int[][]
        {
            new int[] { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
            new int[] { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
            new int[] { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
            new int[] { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
        },
        // S4
        new int[][]
        {
            new int[] { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
            new int[] { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
            new int[] { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
            new int[] { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
        },
        // S5
        new int[][]
        {
            new int[] { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
            new int[] { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
            new int[] { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
            new int[] { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
        },
        // S6
        new int[][]
        {
            new int[] { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
            new int[] { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
            new int[] { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
            new int[] { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
        },
        // S7
        new int[][]
        {
            new int[] { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
            new int[] { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
            new int[] { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
            new int[] { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
        },
        // S8
        new int[][]
        {
            new int[] { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
            new int[] { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
            new int[] { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
            new int[] { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
        }
        };

        public static string HexToBinary(string hexString)
        {
            string binaryString = "";

            foreach (char c in hexString)
            {
                // Chuyển mỗi ký tự hex thành một chuỗi nhị phân 4-bit
                string binary = Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0');
                binaryString += binary;
            }

            return binaryString;
        }
        public static string ConvertBitsToHex(string bitStr)
        {
            StringBuilder finString = new StringBuilder();

            // Lặp qua từng nhóm 4 bit trong chuỗi
            for (int i = 0; i < bitStr.Length / 4; i++)
            {
                // Lấy một nhóm 4 bit
                string bitVal = bitStr.Substring(i * 4, 4);

                switch (bitVal)
                {
                    case "0000":
                        finString.Append("0");
                        break;
                    case "0001":
                        finString.Append("1");
                        break;
                    case "0010":
                        finString.Append("2");
                        break;
                    case "0011":
                        finString.Append("3");
                        break;
                    case "0100":
                        finString.Append("4");
                        break;
                    case "0101":
                        finString.Append("5");
                        break;
                    case "0110":
                        finString.Append("6");
                        break;
                    case "0111":
                        finString.Append("7");
                        break;
                    case "1000":
                        finString.Append("8");
                        break;
                    case "1001":
                        finString.Append("9");
                        break;
                    case "1010":
                        finString.Append("A");
                        break;
                    case "1011":
                        finString.Append("B");
                        break;
                    case "1100":
                        finString.Append("C");
                        break;
                    case "1101":
                        finString.Append("D");
                        break;
                    case "1110":
                        finString.Append("E");
                        break;
                    case "1111":
                        finString.Append("F");
                        break;
                    default:
                        throw new ArgumentException($"Nhóm bit '{bitVal}' không hợp lệ.");
                }
            }

            return finString.ToString();
        }

        // Permutation Function
        private static string Permute(string block, int[] table)
        {
            StringBuilder result = new StringBuilder();
            foreach (int i in table)
            {
                result.Append(block[i - 1]);
            }
            return result.ToString();
        }
        // Initial Permutation (IP)
        private static string InitialPermutation(string plain_text)
        {
            return Permute(plain_text, IP);
        }

        // Final Permutation (FP)
        private static string FinalPermutation(string cipher_text)
        {
            return Permute(cipher_text, FP);
        }

        // Expansion Function (E)
        private static string Expansion(string bits)
        {
            return Permute(bits, E);
        }

        // XOR Function
        private static string XOR(string bits1, string bits2)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bits1.Length; i++)
            {
                result.Append((int.Parse(bits1[i].ToString()) ^ int.Parse(bits2[i].ToString())).ToString());
            }
            return result.ToString();
        }

        // S-box Function
        private static string SBox(string bits)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 48; i += 6)
            {
                string block = bits.Substring(i, 6);
                int row = Convert.ToInt32(block[0].ToString() + block[5].ToString(), 2);
                int col = Convert.ToInt32(block.Substring(1, 4), 2);
                int val = S_BOX[i / 6][row][col];
                result.Append(Convert.ToString(val, 2).PadLeft(4, '0'));
            }
            return result.ToString();
        }
        // DES Round Function
        private static string DESRound(string right, string subkey)
        {
            string expanded_right = Expansion(right);
            string xored = XOR(expanded_right, subkey);
            string sboxed = SBox(xored);
            return Permute(sboxed, P);
        }

        // Generate Subkeys
        private static List<string> GenerateSubkeys(string key)
        {
            // PC-1 Permutation
            key = Permute(key, PC1);

            //chia thành 2 nửa 32 bit
            string left = key.Substring(0, 28);
            string right = key.Substring(28);

            List<string> subkeys = new List<string>();
            for (int i = 0; i < 16; i++)
            {
                // dịch trái 
                left = left.Substring(shifts[i]) + left.Substring(0, shifts[i]);
                right = right.Substring(shifts[i]) + right.Substring(0, shifts[i]);

                // PC-2 Permutation
                string subkey = Permute(left + right, PC2);
                subkeys.Add(subkey);
            }
            return subkeys;
        }

        // DES Encryption
        public static string Encrypt(string plain_text, string key)
        {
            string binary_plaintext=HexToBinary(plain_text.Trim());
            string binary_key=HexToBinary(key.Trim());
            // Initial Permutation
            string permuted_text = InitialPermutation(binary_plaintext);

            // Generate Subkeys
            List<string> subkeys = GenerateSubkeys(binary_key);

            // Split into two halves
            string left = permuted_text.Substring(0, 32);
            string right = permuted_text.Substring(32);

            for (int i = 0; i < 16; i++)
            {
                // DES Round
                string temp = left;
                left = right;
                right = XOR(temp, DESRound(right, subkeys[i]));
            }

            // Final Permutation
            string cipher_text = FinalPermutation(right + left);// right+left chính là hoán vị 32 bit

            return ConvertBitsToHex(cipher_text);
        }

        // DES Decryption
        public static string Decrypt(string cipher_text, string key)
        {
            string binary_cipher_text=HexToBinary(cipher_text.Trim());
            string binary_key=HexToBinary(key.Trim());
            // Generate Subkeys
            List<string> subkeys = GenerateSubkeys(binary_key);

            // Initial Permutation
            string permuted_text = InitialPermutation(binary_cipher_text);

            // Split into two halves
            string left = permuted_text.Substring(0, 32);
            string right = permuted_text.Substring(32);

            for (int i = 15; i >= 0; i--)
            {
                // DES Round
                string temp = left;
                left = right;
                right = XOR(temp, DESRound(right, subkeys[i]));
            }

            // Final Permutation
            string plain_text = FinalPermutation(right + left);
            return ConvertBitsToHex(plain_text);
        }
        public static List<PlainText> PrintLeftRight(string plain_text, string key)
        {
            List<PlainText> result = new List<PlainText>();
            string binary_plaintext = HexToBinary(plain_text.Trim());
            string binary_key = HexToBinary(key.Trim());
            // Initial Permutation
            string permuted_text = InitialPermutation(binary_plaintext);

            // Generate Subkeys
            List<string> subkeys = GenerateSubkeys(binary_key);

            // Split into two halves
            string left = permuted_text.Substring(0, 32);
            string right = permuted_text.Substring(32);
            for (int i = 0; i < 16; i++)
            {
                PlainText plainText = new PlainText(i, ConvertBitsToHex(left), ConvertBitsToHex(right));
                result.Add(plainText);
                // DES Round
                string temp = left;
                left = right;
                right = XOR(temp, DESRound(right, subkeys[i]));
            }
            return result;
        }


        public static List<Key> printSubKey(string key)
        {   
            key= HexToBinary(key.Trim());
            key = Permute(key, PC1);
            //chia thành 2 nửa 32 bit
            string left = key.Substring(0, 28);
            string right = key.Substring(28);

            List<Key> subkeys = new List<Key>();
            for (int i = 0; i < 16; i++)
            {
                // dịch trái 
                left = left.Substring(shifts[i]) + left.Substring(0, shifts[i]);
                right = right.Substring(shifts[i]) + right.Substring(0, shifts[i]);

                // PC-2 Permutation
                string subkey = Permute(left + right, PC2);
                Key key_round=new Key(i, ConvertBitsToHex(subkey));
                subkeys.Add(key_round);
            }
            return subkeys;

        }
    }
}


