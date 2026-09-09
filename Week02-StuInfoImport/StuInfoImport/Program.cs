using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuInfoImport
{
    internal class Program
    {
        static string stuName;
        static int stuId;
        static int stuAge;
        static double litratureScore;
        static double advancedMathScore;
        static double progDesignScore;
        static void Main(string[] args)
        {
            Console.WriteLine("=== 学生信息录入器 v1.0 ===");
            Console.Write("请输入姓名：");
            stuName = Console.ReadLine();
            Console.Write("请输入学号：");
            int.TryParse(Console.ReadLine(), out stuId);
            Console.Write("请输入年龄：");
            int.TryParse(Console.ReadLine(), out stuAge);

            Console.Write("请输入大学语文成绩：");
            double.TryParse(Console.ReadLine(), out litratureScore);
            Console.Write("请输入高等数学成绩：");
            double.TryParse(Console.ReadLine(), out advancedMathScore);
            Console.Write("请输入程序设计成绩：");
            double.TryParse(Console.ReadLine(), out progDesignScore);

            double sum = litratureScore + advancedMathScore + progDesignScore;
            double avg = sum / 3.0;

            Console.WriteLine("\n=== 录入完成 ===");

            Console.WriteLine($"姓名：{stuName}，学号：{stuId}，年龄：{stuAge} 岁");
            Console.WriteLine($"总分：{sum}，平均分：{avg:f2}");
        }
    }
}
