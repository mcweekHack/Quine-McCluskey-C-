using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using MQMethod.utils;
/// <summary>
/// 与数据结构相关代码具体实现
/// 1 means"1",0 means"0",-1 means"-"
/// </summary>
namespace MQMethod.Service
{
    /// <summary>
    /// 最小项的存储结构
    /// </summary>
    internal class TermsModel
    {
        /// <summary>
        /// 所属的最小项集合
        /// </summary>
        public Stack<int> MinTerm;
        /// <summary>
        /// 是否被标记
        /// </summary>
        public bool SubCube;
        /// <summary>
        /// 最小项的各个取值具体情况
        /// </summary>
        public int[] term;
        /// <summary>
        /// 一共多少个变量
        /// </summary>
        public int Variable_num;
        /// <summary>
        /// 清空本数据项内的数据
        /// </summary>
        public void ClearData()
        {
            MinTerm.Clear();
            SubCube = false;
            Tools.IntArrayClear(term);
            Variable_num = 0;
        }
        /// <summary>
        /// <see cref="TermsModel"/>类的初始化
        /// </summary>
        /// <param name="Variable_number">变量个数</param>
        /// <param name="term_">该最小项的取值具体情况</param>
        /// <param name="MinTerm_">所属最小项</param>
        public TermsModel(int Variable_number, int[] term_,int MinTerm_)
        {
            SubCube = false;
            Variable_num = Variable_number;
            term = new int[Variable_num];
            MinTerm = new Stack<int>();
            for(var i = 0;i< Variable_num; i++)
            {
                term[i] = term_[i];
            }
            MinTerm.Push(MinTerm_);
        }
    }


    /// <summary>
    /// 群表的存储结构
    /// </summary>
    internal class DataStructure
    {
        public int Variable_number;//变量个数
        int[] Logic_Func;//最小项集合
        int[] UnRelated_terms;//无关项集合
        int[] tmp;//数据缓存变量
        /// <summary>
        /// 群表的结构
        /// </summary>
        public Queue<TermsModel>[] terms;



        /// <summary>
        /// <see cref="DataStructure"/>类的创建
        /// </summary>
        /// <param name="variable_number">变量个数</param>
        /// <param name="logic_Func">最小项集合</param>
        /// <param name="unRelated_terms">无关项</param>
        public DataStructure(int variable_number, int[] logic_Func, int[] unRelated_terms)
        {
            Variable_number = variable_number;
            Logic_Func = logic_Func;
            UnRelated_terms = unRelated_terms;
            terms = new Queue<TermsModel>[Variable_number+1];
            for(var i = 0; i < Variable_number+1; i++)
            {
                terms[i] = new Queue<TermsModel>();
                terms[i].Clear();
            }
            tmp = new int[Variable_number];
        }
        /// <summary>
        ///数据结构的初始化
        /// </summary>
        public void StructureInitilize()
        {
            int count;
            for(var i = 0; i < Logic_Func.Length; i++)
            {
                Tools.IntArrayClear(tmp);
                Tools.Int2bit(tmp, Logic_Func[i]);
                count = Tools.Return1Number(tmp);
                terms[count].Enqueue(new TermsModel(Variable_number, tmp, Logic_Func[i]));
            }
            for(var i = 0; i < UnRelated_terms.Length; i++)
            {
                Tools.IntArrayClear(tmp);
                Tools.Int2bit(tmp, UnRelated_terms[i]);
                count = Tools.Return1Number(tmp);
                terms[count].Enqueue(new TermsModel(Variable_number, tmp, Logic_Func[i]));
            }
            return;
        }
        /// <summary>
        /// 清空指定群的数据项
        /// </summary>
        /// <param name="GroupNumber">群的代号如果全部清理输入-1</param>
        public void ClearData(int GroupNumber)
        {
            if (GroupNumber != -1)
                terms[GroupNumber].Clear();
            else
            {
                for(var i = 0;i<terms.Length;i++)
                { terms[i].Clear(); }
            }
            return;
        }
        /// <summary>
        /// 向群表内添加数据项
        /// </summary>
        /// <param name="item">对应的数据项</param>
        public void AddData(TermsModel item)
        {
            int count = Tools.Return1Number(item.term);
            foreach(var term in terms[count]) 
            {
                if (Tools.ReturnDifference(term.term, item.term) == 0)
                    return;
            }
            terms[count].Enqueue(item);
        }
        /// <summary>
        /// 合并差别为1的数据项
        /// </summary>
        /// <param name="item1">第一个数据项</param>
        /// <param name="item2">第二个数据项</param>
        /// <param name="res">返回结果到res</param>
        /// <returns>是否合并成功,如果差别为1则返回成功,否则返回失败</returns>
        public bool CombineTerms(TermsModel item1, TermsModel item2, TermsModel res)
        {
            if (Tools.ReturnDifference(item1.term, item2.term) != 1)
                return false;
            res.ClearData();
            res.Variable_num = Variable_number;
            foreach (var item in item1.MinTerm)
            {
                res.MinTerm.Push(item);
            }
            foreach (var item in item2.MinTerm)
            {
                res.MinTerm.Push(item);
            }
            for(var i = 0;i < item1.term.Length; i++)
            {
                if (item1.term[i] == item2.term[i])
                    res.term[i] = item1.term[i];
                else
                    res.term[i] = -1;
            }
            item1.SubCube = true;
            item2.SubCube = true;
            return true;
        }
        /// <summary>
        /// 打印本数据结构,用于测试用途
        /// </summary>
        public void TestFunc()
        {
            for(var i = 0; i < Variable_number + 1; i++)
            {
                Console.WriteLine($"The Group {i} has {terms[i].Count} terms.");
                if(terms[i].Count>0)
                    Console.WriteLine($"The Group {i} Sheet:");
                foreach(var item in terms[i])
                {
                    Console.Write($"The MinTermGroup[");
                    foreach(var item_ in item.MinTerm)
                    {
                        Console.Write($"{item_},");
                    }
                    Console.Write("]: ");
                    for(var j = 0; j < item.term.Length; j++)
                    {
                        Console.Write($"{item.term[j]},");
                    }
                    Console.WriteLine($" IsSubCube: {item.SubCube}");
                }
            }
            return;
        }

    }


    /// <summary>
    /// 输入数据处理器
    /// </summary>
    internal class InputDataHandler
    {
        //与文件IO相关
        const string InputFilePath = "./Input/";
        JObject Json_data;


        /// <summary>
        /// 存储变量个数
        /// </summary>
        public int Variable_number = 0;
        /// <summary>
        /// 处理的文件名
        /// </summary>
        public string FileName;
        /// <summary>
        /// 最小项
        /// </summary>
        public int[] Logic_Function;
        /// <summary>
        /// 无关项
        /// </summary>
        public int[] UnRelated_terms;
        /// <summary>
        /// <see cref="InputDataHandler"/>类的创建与初始化
        /// </summary>
        /// <param name="FileName">读取的文件名</param>
        public InputDataHandler(string FileName)
        {
            this.FileName = FileName;
            this.Initilize();
        }
        void Initilize()
        {
            string path = InputFilePath + FileName;
            Json_data = JObject.Parse(File.ReadAllText(path));
            Variable_number = (int)Json_data["Variable_number"];
            int size1 = (int)Json_data["Logic_Function"]["size"];
            int size2 = (int)Json_data["UnRelated_terms"]["size"];
            if (size1 > 0)
            {
                Logic_Function = new int[size1];
                for(var i = 0; i < size1; i++)
                {
                    Logic_Function[i] = (int)Json_data["Logic_Function"]["data"][i];
                }
            }
            else
                Logic_Function =[];
            if (size2 > 0)
            {
                UnRelated_terms = new int[size2];
                for (var i = 0; i < size1; i++)
                {
                    Logic_Function[i] = (int)Json_data["UnRelated_terms"]["data"][i];
                }
            }
            else
                UnRelated_terms = [];
        }


        /// <summary>
        /// 打印获取到的数据,测试函数
        /// </summary>
        /// <returns></returns>
        public void TestFunc()
        {
            Console.WriteLine(FileName);
            Console.WriteLine(Variable_number);
            for(var i = 0; i < Logic_Function.Length; i++)
            {
                Console.Write($"{Logic_Function[i]},");
            }
            Console.WriteLine("");
            for(var i = 0;i < UnRelated_terms.Length; i++)
            {
                Console.Write($"{UnRelated_terms[i]},");
            }
            Console.WriteLine("");
        }
    }


}
