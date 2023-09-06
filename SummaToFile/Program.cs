using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummaToFile
{

    public class MyFile
    {
        public string filename { get; set; }
     
        public MyFile(string name)
        {
            filename = name;
        }
        public List<(int, int)> GeneratePairs(int count)
        {
            Random random = new Random();
            List<(int, int)> pairs = new List<(int, int)>();

            for (int i = 0; i < count; i++)
            {
                int a = random.Next(1, 100);
                int b = random.Next(1, 100);

                pairs.Add((a, b));
            }

            return pairs;
        }
        public void SumOfPairs()
        {
            lock (this)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        string line;//сюда считываем из файла
                        List<string> strings = new List<string>();

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(' ');
                            
                            strings.Add($" {parts[0]} + {parts[1]} = {int.Parse(parts[0]) + int.Parse(parts[1])}");

                        }
                        StreamWriter writer = new StreamWriter("Result.txt", true);//дозапись разрешена
                        foreach (string s in strings)
                        { writer.WriteLine(s); }
                        writer.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка чтения файла: {e.Message}");
                }
            }
        }
    
    public void MultOfPairs()
    {
            lock (this)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        string line;//сюда считываем из файла
                        List<string> strings = new List<string>();

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(' ');
                            strings.Add($" {parts[0]} x {parts[1]} = {int.Parse(parts[0]) * int.Parse(parts[1])}");

                        }
                        StreamWriter writer = new StreamWriter("Result.txt", true);//дозапись разрешена
                        foreach (string s in strings)
                        { writer.WriteLine(s); }
                        writer.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка чтения файла: {e.Message}");
                }
            }
       
    }

    public void WriteNumbers()
    {
        lock (this)
        {

            FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(file);
                var temp = GeneratePairs(10);
                foreach (var i in temp)
                { writer.WriteLine($"{i.Item1} {i.Item2}");}
            writer.Close();
        }
    }

    
}

    internal class Program
    {
        static void Main(string[] args)
        {
            MyFile myFile = new MyFile("Numbers.txt");

            Task task1 = new Task(myFile.WriteNumbers);
            
            Task task2 = new Task(myFile.SumOfPairs);
            Task task3 = new Task(myFile.MultOfPairs);
            task1.Start();
            Task.WaitAll(task1);
            task2.Start(); task3.Start();
            Task.WaitAll(task2, task3);
        }
    }

}
    

