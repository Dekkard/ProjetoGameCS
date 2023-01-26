using System;
using System.Threading;
#pragma warning disable CA1416
public class Sounds
{
    public static void LevelUpSound()
    {
        Console.Beep(1200, 100);
        Console.Beep(1200, 100);
        Console.Beep(1500, 200);
        Console.Beep(1200, 100);
        Console.Beep(1500, 500);
    }
    public static void Cave()
    {
        Console.Beep(200, 500);
        Console.Beep(100, 1000);
    }
    public static void Play()
    {
        Console.WriteLine("Bem-vindo ao tocador de beeps"
            + "\nPrecione de '-q' a '-P' no teclado para tocar as notas."
            + "\nAs teclas 7 e 4 alteram o tempo de toque da nota."
            + "\nAs teclas 9 e 6 ajustam a frequência de toque das notas."
            + "\nA tecla S fecha o tocador."
        );
        char inp = ' ';
        int time = 100;
        int frequency = 100;
        while (inp != 's')
        {
            inp = Console.ReadKey(true).KeyChar;
            switch (inp)
            {
                case 'q':
                    Console.Beep(frequency * 1, time);
                    break;
                case 'w':
                    Console.Beep(frequency * 3, time);
                    break;
                case 'e':
                    Console.Beep(frequency * 5, time);
                    break;
                case 'r':
                    Console.Beep(frequency * 7, time);
                    break;
                case 't':
                    Console.Beep(frequency * 9, time);
                    break;
                case 'y':
                    Console.Beep(frequency * 10, time);
                    break;
                case 'u':
                    Console.Beep(frequency * 11, time);
                    break;
                case 'i':
                    Console.Beep(frequency * 13, time);
                    break;
                case 'o':
                    Console.Beep(frequency * 15, time);
                    break;
                case 'p':
                    Console.Beep(frequency * 19, time);
                    break;
                case '7':
                    time += 100;
                    time = time > 1000 ? 1000 : time;
                    Console.WriteLine("Aumetando o tempo: " + time);
                    break;
                case '4':
                    time -= 100;
                    time = time < 100 ? 100 : time;
                    Console.WriteLine("Diminuindo o tempo: " + time);
                    break;
                case '9':
                    frequency += 100;
                    frequency = frequency > 500 ? 500 : frequency;
                    Console.WriteLine("Aumetando o tom: " + frequency);
                    break;
                case '6':
                    frequency -= 100;
                    frequency = frequency < 100 ? 100 : frequency;
                    Console.WriteLine("Diminuindo o tom: " + frequency);
                    break;
            }
        }
    }
}