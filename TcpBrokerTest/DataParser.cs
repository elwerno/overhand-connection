using System;

namespace TcpBrokerTest
{
    public class DataParser
    {
        /*
         * Function: 
         * Converts the String to a Byte-Array, which then will past on to ParseByte
         * 
         * Input:
         * stringData = 1 data pack (10 measurements) from the Overhand-trackers in a String
         * 
         * Output:
         * A 2D Array, ParseString[Measurement, Sensor], values are all Int32
         * => 0 = gyroX ; 1 = gyroY; 2 = gyroZ ; 3 = lowAccelX ; 4 = lowAccelY ; 5 = lowAccelZ ; 6 = highAccelX ; 7 = highAccelY ; 8 = highAccelZ
         */
        public static Int32[,] ParseString(string stringData)
        {
            byte[] byteData = new byte[stringData.Length / 2];

            // convert string to byte Array
            for (int index = 0; index < stringData.Length; index += 2)
            {
                byteData[index / 2] = Convert.ToByte(stringData.Substring(index, 2), 16);
            }

            // get parsed value
            Int32[,] result = new Int32[10, 9];
            result = ParseByte(byteData);

            return result;
        }

        /*
         * Function: 
         * Formats the data from the Overhand-trackers correctly
         * 
         * Input:
         * byteData = 1 data pack (10 measurements) from the Overhand-trackers in a (byte-)Array
         * 
         * Output:
         * A 2D Array, ParseByte[Measurement, Sensor], values are all Int32
         * => 0 = gyroX ; 1 = gyroY; 2 = gyroZ ; 3 = lowAccelX ; 4 = lowAccelY ; 5 = lowAccelZ ; 6 = highAccelX ; 7 = highAccelY ; 8 = highAccelZ
         */
        public static Int32[,] ParseByte(byte[] byteData)
        {
            Int32[,] formatted = new Int32[10, 9];

            // count through all the measurements
            for (int measurement = 0; measurement < 10; measurement++)
            {
                // count through all the sensors
                for (int sensor = 0; sensor < 9; sensor++)
                {
                    // select the bytes
                    byte byteA = byteData[(sensor + measurement * 9) * 2];
                    byte byteB = byteData[(sensor + measurement * 9) * 2 + 1];

                    // shift bytes together
                    int sign = Convert.ToByte(byteB) & Convert.ToByte(0x80);
                    Int32 provisional = Convert.ToInt32(byteB << 8 | byteA);

                    // check if +/-
                    // 128 == 0x80
                    if (sign == 128)
                    {
                        provisional = unchecked((int) 0xFFFF0000) | provisional;
                    }

                    // save everything in a array
                    formatted[measurement, sensor] = provisional;
                }
            }

            return formatted;
        }
    }
}