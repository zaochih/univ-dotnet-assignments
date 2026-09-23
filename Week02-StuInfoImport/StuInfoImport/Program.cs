using System;
using System.Collections.Generic;
using System.Linq;

namespace StuInfoImport
{
    internal class Program
    {
        class Student
        {
            public string Name = "";
            public long Id = 0;
            public int Age = 0;
            public double LiteratureScore = 0.0;
            public double AdvancedMathScore = 0.0;
            public double ProgDesignScore = 0.0;
            public double Sum => LiteratureScore + AdvancedMathScore + ProgDesignScore;
            public double Avg => Sum / 3.0;
        }

        static List<Student> students = new List<Student>();

        #region 安全输入方法
        /// <summary>
        /// 安全读取一行输入。输入流结束（如 Ctrl+Z）时退出程序，避免无限循环。
        /// </summary>
        /// <param name="prompt">在控制台提供给用户的输入提示</param>
        /// <returns>去除首尾空白后的输入内容</returns>
        static string ReadLineSafe(string prompt)
        {
            Console.Write(prompt);
            try
            {
                string input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("输入已结束，程序退出。");
                    Environment.Exit(0);
                }
                return input.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[读取失败] {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// 安全读取非空字符串，输入为空时反复提示。
        /// </summary>
        /// <param name="prompt">在控制台提供给用户的输入提示</param>
        /// <param name="currentValue">原始值</param>
        /// <param name="isEdit">标记本次调用是否为编辑用途（编辑用途下空输入不修改原始值）</param>
        /// <returns>用户输入的字符串；编辑模式下输入为空则返回原始值</returns>
        static string ReadString(string prompt, string currentValue, bool isEdit)
        {
            while (true)
            {
                string input = ReadLineSafe(prompt);
                if (input.Length > 0)
                {
                    return input;
                }
                if (isEdit)
                {
                    return currentValue;
                }
                Console.WriteLine("[输入无效] 内容不能为空，请重新输入。");
            }
        }

        /// <summary>
        /// 安全读取数值，格式错误、溢出或超出范围时捕获异常并反复提示。
        /// </summary>
        /// <typeparam name="T">目标数值类型（int / long / double）</typeparam>
        /// <param name="prompt">在控制台提供给用户的输入提示</param>
        /// <param name="currentValue">原始值</param>
        /// <param name="isEdit">标记本次调用是否为编辑用途（编辑用途下空输入不修改原始值）</param>
        /// <param name="min">允许的最小值</param>
        /// <param name="max">允许的最大值</param>
        /// <returns>合法的数值；编辑模式下输入为空则返回原始值</returns>
        static T ReadNumber<T>(string prompt, T currentValue, bool isEdit, T min, T max) where T : IComparable<T>
        {
            while (true)
            {
                string input = ReadLineSafe(prompt);
                if (isEdit && input.Length == 0)
                {
                    return currentValue;
                }

                try
                {
                    T value = (T)Convert.ChangeType(input, typeof(T));
                    if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(input), $"取值范围为 {min} ~ {max}");
                    }
                    return value;
                }
                catch (FormatException)
                {
                    Console.WriteLine("[输入无效] 格式错误，请输入数字。");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("[输入无效] 数值过大或过小。");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($"[输入无效] 取值范围为 {min} ~ {max}，请重新输入。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[输入无效] {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 安全清屏。输出被重定向时 Console.Clear 会抛出异常，此处忽略。
        /// </summary>
        static void ClearScreen()
        {
            try
            {
                Console.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine();
            }
        }
        #endregion

        #region 学生信息导入
        /// <summary>
        /// 学生信息导入方法
        /// </summary>
        /// <param name="student">要录入或修改的学生</param>
        /// <param name="isEdit">标记本次调用是否为编辑。</param>
        static void StudentInfoImport(Student student, bool isEdit)
        {
            student.Name = ReadString("请输入姓名：", student.Name, isEdit);
            student.Id = ReadNumber("请输入学号：", student.Id, isEdit, 0L, long.MaxValue);
            student.Age = ReadNumber("请输入年龄：", student.Age, isEdit, 1, 150);

            student.LiteratureScore = ReadNumber("请输入大学语文成绩：", student.LiteratureScore, isEdit, 0.0, 100.0);
            student.AdvancedMathScore = ReadNumber("请输入高等数学成绩：", student.AdvancedMathScore, isEdit, 0.0, 100.0);
            student.ProgDesignScore = ReadNumber("请输入程序设计成绩：", student.ProgDesignScore, isEdit, 0.0, 100.0);

            ClearScreen();

            Console.WriteLine(isEdit ? "=== 修改完成 ===" : "=== 录入完成 ===");
            PrintStudent(student);
        }

        static void PrintStudent(Student student)
        {
            Console.WriteLine($"姓名：{student.Name}，学号：{student.Id}，年龄：{student.Age} 岁");
            Console.WriteLine($"总分：{student.Sum}，平均分：{student.Avg:f2}");
            Console.WriteLine();
        }

        static void AddStudent()
        {
            Student student = new Student();
            StudentInfoImport(student, false);
            students.Add(student);
        }

        /// <summary>
        /// 重新输入：按序号选择名单中的学生并修改其信息
        /// </summary>
        static void EditStudent()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("当前名单为空，请先录入学生信息和成绩。");
                Console.WriteLine();
                return;
            }

            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].Name}（学号：{students[i].Id}）");
            }
            int index = ReadNumber("请输入要重新输入的学生序号：", 0, false, 1, students.Count);
            Student student = students[index - 1];

            ClearScreen();
            Console.WriteLine($"=== 修改学生：{student.Name} ===");
            Console.WriteLine("提示：留空以保留当前值。");
            StudentInfoImport(student, true);
        }
        #endregion

        #region 等级评定
        /// <summary>
        /// 根据平均分返回等级
        /// </summary>
        static string GetGrade(double avg)
        {
            if (avg >= 90)
                return "A 优秀";
            else if (avg >= 80)
                return "B 良好";
            else if (avg >= 70)
                return "C 中等";
            else if (avg >= 60)
                return "D 及格";
            else if (avg >= 0)
                return "F 不及格";
            else
                return "成绩无效";
        }

        /// <summary>
        /// 等级评定方法：对名单中所有学生进行评定
        /// </summary>
        static void GradeEvaluation()
        {
            Console.WriteLine("=== 等级评定 ===");
            if (students.Count == 0)
            {
                Console.WriteLine("请先录入学生信息和成绩。");
                Console.WriteLine();
                return;
            }
            for (int i = 0; i < students.Count; i++)
            {
                Student student = students[i];
                Console.WriteLine($"{i + 1}. {student.Name}：平均分 {student.Avg:f2}，等级 {GetGrade(student.Avg)}");
            }
            Console.WriteLine();
        }
        #endregion

        #region 删除学生信息
        static void DeleteStudentByName()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("当前名单为空，无可删除的学生。");
                Console.WriteLine();
                return;
            }

            string nameToDelete = ReadString("请输入要删除的学生姓名：", "", false);

            int deletedCount = students.RemoveAll(s => s.Name == nameToDelete);
            if (deletedCount > 0)
            {
                Console.WriteLine($"已删除 {deletedCount} 个名为 {nameToDelete} 的学生。");
            }
            else
            {
                Console.WriteLine($"未找到名为 {nameToDelete} 的学生。");
            }
            Console.WriteLine();
        }
        #endregion

        #region 显示学生名单
        static void ShowAllStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("当前名单为空。");
                Console.WriteLine();
                return;
            }

            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"=== 学生 {i + 1} / {students.Count} ===");
                PrintStudent(students[i]);
            }
        }
        #endregion

        #region 搜索学生信息
        static void SearchStudentByName()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("当前名单为空，无可搜索的学生。");
                Console.WriteLine();
                return;
            }

            string nameToSearch = ReadString("请输入要搜索的学生姓名：", "", false);
            List<Student> foundStudents = students
                .Where(s => s.Name.IndexOf(nameToSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
            if (foundStudents.Count > 0)
            {
                Console.WriteLine($"找到 {foundStudents.Count} 个名字中包含 {nameToSearch} 的学生：");
                foreach (Student student in foundStudents)
                {
                    PrintStudent(student);
                }
            }
            else
            {
                Console.WriteLine($"未找到名字中包含 {nameToSearch} 的学生。");
                Console.WriteLine();
            }
        }
        #endregion

        #region 菜单
        /// <summary>
        /// 学生名单管理子菜单
        /// </summary>
        static void StudentListMenu()
        {
            while (true)
            {
                Console.WriteLine("======== 学生名单管理器 ========");
                Console.WriteLine($"当前共有 {students.Count} 名学生");
                Console.WriteLine("1. 添加学生\n2. 删除学生（按姓名）\n3. 显示全部名单\n4. 搜索学生（模糊忽略大小写）\n5. 返回主菜单");
                Console.WriteLine();
                string choice = ReadLineSafe("请输入菜单选项：");
                ClearScreen();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("=== 添加学生 ===");
                        AddStudent();
                        break;
                    case "2":
                        Console.WriteLine("=== 删除学生 ===");
                        DeleteStudentByName();
                        break;
                    case "3":
                        Console.WriteLine("=== 学生名单 ===");
                        ShowAllStudents();
                        break;
                    case "4":
                        Console.WriteLine("=== 搜索学生 ===");
                        SearchStudentByName();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("无效的选择，请重新输入。");
                        Console.WriteLine();
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("==== 学生信息管理系统 v4.0 ====");
                    Console.WriteLine("1. 录入学生信息和成绩\n2. 查看等级评定\n3. 重新输入\n4. 学生名称管理\n0. 退出");
                    string choice = ReadLineSafe("请选择：");
                    ClearScreen();
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("=== 学生信息成绩录入 ===");
                            AddStudent();
                            break;
                        case "2":
                            GradeEvaluation();
                            break;
                        case "3":
                            Console.WriteLine("=== 重新输入 ===");
                            EditStudent();
                            break;
                        case "4":
                            StudentListMenu();
                            ClearScreen();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("无效的选择，请重新输入。");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // 兜底：任何未预料的异常都不会让程序崩溃
                    Console.WriteLine($"[程序异常] {ex.Message}，已返回主菜单。");
                    Console.WriteLine();
                }
            }
        }
        #endregion
    }
}
