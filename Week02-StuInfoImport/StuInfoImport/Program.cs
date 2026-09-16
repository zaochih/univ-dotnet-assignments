using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StuInfoImport
{
    internal class Program
    {
        class Student
        {
            public string name;
            public Int64 id = 0;
            public int age = 0;
            public double litratureScore = 0.0;
            public double advancedMathScore = 0.0;
            public double progDesignScore = 0.0;
            public double sum => litratureScore + advancedMathScore + progDesignScore;
            public double avg => sum / 3.0;
        }
        static Student currentStudent = null;

        #region 导入工具方法
        /// <summary>
        /// 导入工具方法 - Written w/ Help from Gemini
        /// </summary>
        /// <typeparam name="T">目标变量的类型</typeparam>
        /// <param name="prompt">在控制台提供给用户的输入提示</param>
        /// <param name="currentValue">默认值</param>
        /// <param name="isEdit">标记本次调用是否为编辑用途（编辑用途下空输入或空格输入不修改原始值）</param>
        /// <returns>返回一个值，将该值赋给对应参数。在编辑模式下，如果输入为空或全为空格，则返回值等于原始值。</returns>
        static T ImportUtil<T>(string prompt, T currentValue, bool isEdit)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (isEdit && string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            try
            {
                return (T)Convert.ChangeType(input, typeof(T));
            }
            catch
            {
                return currentValue;
            }
        }
        #endregion

        #region 学生信息导入
        /// <summary>
        /// 学生信息导入方法
        /// </summary>
        /// <param name="isEdit">标记本次调用是否为编辑。</param>
        static void StudentInfoImport(bool isEdit)
        {
            currentStudent.name = ImportUtil("请输入姓名：", currentStudent.name, isEdit);
            currentStudent.id = ImportUtil("请输入学号：", currentStudent.id, isEdit);
            currentStudent.age = ImportUtil("请输入年龄：", currentStudent.age, isEdit);

            currentStudent.litratureScore = ImportUtil("请输入大学语文成绩：", currentStudent.litratureScore, isEdit);
            currentStudent.advancedMathScore = ImportUtil("请输入高等数学成绩：", currentStudent.advancedMathScore, isEdit);
            currentStudent.progDesignScore = ImportUtil("请输入程序设计成绩：", currentStudent.progDesignScore, isEdit);

            Console.Clear();

            Console.WriteLine(isEdit ? "=== 修改完成 ===" : "=== 录入完成 ===");

            Console.WriteLine($"姓名：{currentStudent.name}，学号：{currentStudent.id}，年龄：{currentStudent.age} 岁");
            Console.WriteLine($"总分：{currentStudent.sum}，平均分：{currentStudent.avg:f2}");
            Console.WriteLine();
        }
        #endregion

        #region 等级评定
        /// <summary>
        /// 等级评定方法
        /// </summary>
        static void GradeEvaluation()
        {
            Console.WriteLine("=== 等级评定 ===");
            if (currentStudent == null)
            {
                Console.WriteLine("请先录入学生信息和成绩。");
                return;
            }
            string grade;
            if (currentStudent.avg >= 90)
                grade = "A 优秀";
            else if (currentStudent.avg >= 80)
                grade = "B 良好";
            else if (currentStudent.avg >= 70)
                grade = "C 中等";
            else if (currentStudent.avg >= 60)
                grade = "D 及格";
            else if (currentStudent.avg >= 0)
                grade = "F 不及格";
            else
            {
                Console.WriteLine("成绩无效，无法评定等级。");
                Console.WriteLine();
                return;
            }
            Console.WriteLine($"学生 {currentStudent.name} 的等级评定为：{grade}");
            Console.WriteLine();
        }
        #endregion

        static void Main(string[] args)
        {
            Student student = new Student();
            do
            {
                Console.WriteLine("=== 学生信息评定器 v2.0 ===");
                Console.WriteLine("1. 录入学生信息和成绩\n2. 查看等级评定\n3. 重新输入\n0. 退出程序");
                Console.Write("请选择：");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        if (currentStudent != null)
                        {
                            Console.WriteLine("已有学生信息。如需覆盖，请使用 3");
                            Console.WriteLine();
                            break;
                        }
                        currentStudent = new Student();
                        Console.WriteLine("=== 学生信息成绩录入 ===");
                        StudentInfoImport(false);
                        break;
                    case "2":
                        Console.Clear();
                        GradeEvaluation();
                        break;
                    case "3":
                        Console.Clear();
                        if (currentStudent == null)
                        {
                            Console.WriteLine("请先录入学生信息和成绩。");
                            Console.WriteLine();
                            break;
                        }
                        Console.WriteLine($"=== 修改学生：{currentStudent.name} ===");
                        Console.WriteLine("提示：留空以保留当前值。");
                        StudentInfoImport(true);
                        break;
                    case "0":
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("无效的选择，请重新输入。");
                        Console.WriteLine();
                        break;
                }
            } while (true);
        }
    }
}
