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
        static List<Student> students = new List<Student>();

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
            Student currentStudent = new Student();

            string nameString = ImportUtil("请输入姓名：", currentStudent.name, isEdit);
            while (string.IsNullOrWhiteSpace(nameString))
            {
                Console.WriteLine("[输入无效] 姓名不能为空。");
                Console.WriteLine();
                nameString = ImportUtil("请输入姓名：", currentStudent.name, isEdit);
            }
            currentStudent.name = nameString;
            currentStudent.id = ImportUtil("请输入学号：", currentStudent.id, isEdit);
            currentStudent.age = ImportUtil("请输入年龄：", currentStudent.age, isEdit);

            currentStudent.litratureScore = ImportUtil("请输入大学语文成绩：", currentStudent.litratureScore, isEdit);
            currentStudent.advancedMathScore = ImportUtil("请输入高等数学成绩：", currentStudent.advancedMathScore, isEdit);
            currentStudent.progDesignScore = ImportUtil("请输入程序设计成绩：", currentStudent.progDesignScore, isEdit);

            students.Add(currentStudent);

            Console.Clear();

            Console.WriteLine(isEdit ? "=== 修改完成 ===" : "=== 录入完成 ===");

            Console.WriteLine($"姓名：{currentStudent.name}，学号：{currentStudent.id}，年龄：{currentStudent.age} 岁");
            Console.WriteLine($"总分：{currentStudent.sum}，平均分：{currentStudent.avg:f2}");
            Console.WriteLine();
        }
        #endregion

        #region 删除学生信息
        static void DeleteStudentByName()
        {
            Console.Write("请输入要删除的学生姓名：");
            string nameToDelete = Console.ReadLine();

            int deletedCount = students.RemoveAll(s => s.name == nameToDelete);
            if (deletedCount > 0)
            {
                Console.WriteLine($"已删除 {deletedCount} 个名为 {nameToDelete} 的学生。");
            }
            else
            {
                Console.WriteLine($"未找到名为 {nameToDelete} 的学生。");
            }
        }
        #endregion

        #region 搜索学生信息
        static void SearchStudentByName()
        {
            Console.Write("请输入要搜索的学生姓名：");
            string nameToSearch = Console.ReadLine();
            List<Student> foundStudents = students.Where(s => s.name.IndexOf(nameToSearch, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            if (foundStudents.Count > 0)
            {
                Console.WriteLine($"找到 {foundStudents.Count} 个名字中包含 {nameToSearch} 的学生：");
                foreach (Student student in foundStudents)
                {
                    Console.WriteLine($"姓名：{student.name}，学号：{student.id}，年龄：{student.age} 岁");
                    Console.WriteLine($"总分：{student.sum}，平均分：{student.avg:f2}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"未找到名为 {nameToSearch} 的学生。");
            }
        }
        #endregion

        static void Main(string[] args)
        {
            Student student = new Student();
            do
            {
                Console.WriteLine("=== 学生名单管理器 ===");
                Console.WriteLine($"当前共有 {students.Count} 名学生");
                Console.WriteLine("1. 添加学生\n2. 删除学生（按姓名）\n3. 显示名单\n4. 搜索学生\n0. 退出");
                Console.Write("请选择：");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("=== 学生信息成绩录入 ===");
                        StudentInfoImport(false);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("=== 删除学生信息 ===");
                        DeleteStudentByName();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("=== 学生名单 ===");
                        int i = 0;
                        foreach (Student s in students)
                        {
                            i++;
                            Console.WriteLine($"=== 学生 {i} / {students.Count} ===");
                            Console.WriteLine($"姓名：{s.name}，学号：{s.id}，年龄：{s.age} 岁");
                            Console.WriteLine($"总分：{s.sum}，平均分：{s.avg:f2}");
                            Console.WriteLine();
                        }
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("=== 搜索学生信息 ===");
                        SearchStudentByName();
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
