// /*
//  * Auteur: Lucas Imark
//  * Date: 07.03.2022
//  * 
//  * Module: 4
//  * LABORATOIRE
//  * 
//  * Description:
//  *
//  */
using System;
using EasyModbus;

namespace ConsoleVisualisationPOC
{
    class MainClass
    {
        const int LARGEUR = 40;
        const int HAUTEUR = 40;

        const int X_OFFSET = 300;
        const int Y_OFFSET = 300;
        const int X_RANGE = 680;
        const int Y_RANGE = 420; //NICE

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //drawSquareWithPointAndAngle(20, 20, 69);


            while (true)
            {
                getData();
                convertData(rawX, rawY);
                Console.WriteLine($"DETECTED: {detected}, X: {finalX}, Y: {finalY}, ANGLE: {rawAngle}, RAW: {rawX}, {rawY}");
                Console.ReadKey();
            }
            
            //drawSquareWithPointAndAngle(finalX, finalY, rawAngle);
            
            
            
        }

        public static float rawX = 0f;
        public static float rawY = 0f;
        public static float rawAngle = 0;
        public static bool detected = false;

        public static int finalX = 0;
        public static int finalY = 0;


        public static void getData()
        {
            EasyModbus.ModbusClient modbusClient = new EasyModbus.ModbusClient("172.16.10.176", 502);

            if (!modbusClient.Available(1000))
            {
                Console.WriteLine("Not available");
            } else
            {
                try
                {
                    modbusClient.Connect();
                    Console.WriteLine("CONNECTED");

                    int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(30010, 7);

                    if (readHoldingRegisters[0] == 256)
                    {
                        detected = true;
                    }
                    else
                    {
                        detected = false;
                    }

                    byte[] lowRegisterBytesX = BitConverter.GetBytes(readHoldingRegisters[1]);
                    byte[] highRegisterBytesX = BitConverter.GetBytes(readHoldingRegisters[2]);
                    byte[] floatBytesX = {
                        lowRegisterBytesX[0],
                        lowRegisterBytesX[1],
                        highRegisterBytesX[0],
                        highRegisterBytesX[1]
                    };

                    byte[] lowRegisterBytesY = BitConverter.GetBytes(readHoldingRegisters[3]);
                    byte[] highRegisterBytesY = BitConverter.GetBytes(readHoldingRegisters[4]);
                    byte[] floatBytesY = {
                        lowRegisterBytesY[0],
                        lowRegisterBytesY[1],
                        highRegisterBytesY[0],
                        highRegisterBytesY[1]
                    };


                    byte[] lowRegisterBytesAngle = BitConverter.GetBytes(readHoldingRegisters[5]);
                    byte[] highRegisterBytesAngle = BitConverter.GetBytes(readHoldingRegisters[6]);
                    byte[] floatBytesAngle = {
                        lowRegisterBytesAngle[0],
                        lowRegisterBytesAngle[1],
                        highRegisterBytesAngle[0],
                        highRegisterBytesAngle[1]
                    };


                    rawX = BitConverter.ToSingle(floatBytesX, 0);
                    rawY = BitConverter.ToSingle(floatBytesY, 0);
                    rawAngle = BitConverter.ToSingle(floatBytesAngle, 0);
                } catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e);
                }
            }

            

        }

        public static void convertData(float xData, float yData)
        {
            float offsettedX = xData - X_OFFSET;
            float dividedX = offsettedX / X_RANGE;
            finalX = (int)Math.Round(dividedX * LARGEUR);

            float offsettedY = yData - Y_OFFSET;
            float dividedY = offsettedY / Y_RANGE;
            finalY = (int)Math.Round(dividedY * HAUTEUR);

            if (finalX > LARGEUR) { finalX = LARGEUR; }
            if (finalX < 0) { finalX = 0; }
            if (finalY > HAUTEUR) { finalY = HAUTEUR; }
            if (finalY < 0) { finalY = 0; }
        }

        public static void drawSquare()
        {
            drawHorizontalLine();
            Console.WriteLine();
            for (int Index = 0; Index <= HAUTEUR - 2; Index++)
            {
                Console.Write("*");
                drawHorizontalLine(' ', LARGEUR -2);
                Console.WriteLine("*");
            }

            drawHorizontalLine();
        }

        public static void drawSquareWithPointAndAngle(int X, int Y, float angle = 0f)
        {
            drawHorizontalLine();
            Console.WriteLine();
            for (int Index = 0; Index <= HAUTEUR - 2; Index++)
            {
                if (Index == Y)
                {
                    Console.Write("*");
                    drawHorizontalLine(' ', X - 1);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("*" + angle);
                    Console.ResetColor();
                    drawHorizontalLine(' ', LARGEUR - (X + 4));
                    Console.WriteLine("*");
                }
                else
                {
                    Console.Write("*");
                    drawHorizontalLine(' ', LARGEUR - 2);
                    Console.WriteLine("*");
                }
            }

            drawHorizontalLine();
        }

        public static void drawSquareWithPoint(int X, int Y)
        {
            drawHorizontalLine();
            Console.WriteLine();
            for (int Index = 0; Index <= HAUTEUR - 2; Index++)
            {
                if (Index == Y)
                {
                    Console.Write("*");
                    drawHorizontalLine(' ', X - 1);
                    Console.Write("*");
                    drawHorizontalLine(' ', LARGEUR - (X + 2));
                    Console.WriteLine("*");
                }
                else
                {
                    Console.Write("*");
                    drawHorizontalLine(' ', LARGEUR - 2);
                    Console.WriteLine("*");
                }
            }

            drawHorizontalLine();
        }

        public static void drawHorizontalLine(char Charracter = '*', int Quantity = LARGEUR)
        {
            for(int Index = 0; Index <= Quantity; Index++)
            {
                Console.Write(Charracter + " ");
            }
        }

    }
}
