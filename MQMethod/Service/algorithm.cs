using MQMethod.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MQMethod.Service
{
    /// <summary>
    ///MQ算法实现
    /// </summary>
    internal class MQalgorithm
    {
        DataStructure sheet_org;//原表
        DataStructure sheet_tmp1;//缓冲表1
        DataStructure sheet_tmp2;//缓冲表2        
        /// <summary>
        /// MQ算法的运行结果
        /// </summary>
        public Stack<TermsModel> models;
        TermsModel tmp;
        /// <summary>
        ///<see cref="MQalgorithm"/>类的初始化
        /// </summary>
        /// <param name="item">传入的数据</param>
        public MQalgorithm(DataStructure item)
        {
            models = new Stack<TermsModel>();
            models.Clear();
            sheet_org = item;
            sheet_tmp1 = item;
            sheet_tmp2 = new DataStructure(sheet_org.Variable_number,null,null);
            tmp = new TermsModel(sheet_org.Variable_number, new int[sheet_org.Variable_number],0);
        }
        /// <summary>
        /// 运行算法
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            bool IsLoop = true;
            while (IsLoop)
            {
                sheet_tmp2.ClearData(-1);
                IsLoop = false;
                for (var i = 0; i < sheet_org.terms.Length - 1; i++)
                {
                    foreach (var item in sheet_tmp1.terms[i])
                    {
                        foreach (var item2 in sheet_tmp1.terms[i + 1])
                        {
                            if (sheet_org.CombineTerms(item, item2, tmp))
                            {
                                IsLoop = true;
                                sheet_tmp2.AddData(tmp);
                                tmp = new TermsModel(sheet_org.Variable_number, new int[sheet_org.Variable_number], 0);
                            }
                        }
                    }
                }
                for (var j = 0; j < sheet_tmp1.terms.Length; j++)
                {
                    foreach (var item in sheet_tmp1.terms[j])
                    {
                        if (!item.SubCube)
                            models.Push(item);
                    }
                }
                if (sheet_tmp1 == sheet_org)
                {
                    sheet_tmp1 = sheet_tmp2;
                    sheet_tmp2 = sheet_org;
                }
                else
                {
                    sheet_tmp2 = sheet_tmp1;
                    sheet_tmp1 = sheet_org;
                }
            }
            return;
        }
        /// <summary>
        /// 打印结果集,为测试用途函数
        /// </summary>
        public void TestFunc()
        {
            Console.WriteLine("The result is : ");
            foreach(var item in models)
            {
                Console.Write("MinTerms:[");
                foreach(var item2 in item.MinTerm)
                {
                    Console.Write($"{item2},");
                }
                Console.Write("]: ");
                for(var i = 0;i < item.term.Length; i++)
                {
                    Console.Write($"{item.term[i]},");
                }
                Console.WriteLine($"IsSubCube: {item.SubCube}");
            }
        }
    }
    internal class MQOptimize
    {
        /// <summary>
        /// 最终优化后的结果
        /// </summary>
        public Stack<TermsModel> result;
        TermsModel[] Models;
        int[] MinTerm;
        Dictionary<TermsModel, HashSet<int>> tmp;
        /// <summary>
        /// <see cref="MQOptimize"/>类的初始化,素项表加速
        /// </summary>
        /// <param name="models">MQ算法的结果</param>
        public MQOptimize(Stack<TermsModel> models, int[] minTerm)
        {

            result = new Stack<TermsModel>();
            result.Clear();

            
            int m = 0;
            Models = new TermsModel[models.Count];
            while( models.Count > 0 )
            {
                Models[m++] = models.Pop();
            }

            
            MinTerm = minTerm;
            Tools.AdjustArrayUpper(MinTerm);

            tmp = new Dictionary<TermsModel, HashSet<int>>();
            tmp.Clear();
        }
        /// <summary>
        /// 优化算法运行
        /// </summary>
        public void Run()
        {
            bool IsHas;
            TermsModel MinMod = null;
            int count = -1;
            for(var x = 0;x < Models.Length; x++)
            {
                foreach (var item in Models[x].MinTerm)
                {
                    IsHas = false;
                    for (var z = 0;z < Models.Length; z++)
                    {
                        if (z != x && Models[z].MinTerm.Contains(item))
                        {
                            IsHas = true;
                            break;
                        }       
                    }
                    if(!IsHas)
                    {
                        Models[x].SubCube = true;
                        result.Push(Models[x]);
                        break;
                    }
                }
            }

            for(var i = 0;i < Models.Length; i++)
            {
                tmp[Models[i]] = new HashSet<int>();
                foreach(var item in Models[i].MinTerm)
                {
                    tmp[Models[i]].Add(item);
                }
            }

            while (true)
            {
                foreach(var item in tmp)
                {
                    foreach(var item2 in item.Value)
                    {
                        IsHas = false;
                        foreach(var item3 in result)
                        {
                            if (!item3.MinTerm.Contains(item2)) continue;
                            IsHas = true;
                            break;
                        }
                        if(IsHas) item.Value.Remove(item2);
                    }
                    if (item.Value.Count == 0) tmp.Remove(item.Key);
                }
                if (tmp.Count == 0) break;
                count = -1;
                foreach(var item in tmp)
                {
                    if (count >= item.Value.Count) continue;
                    count = item.Value.Count;
                    MinMod = item.Key;
                }
                MinMod.SubCube = true;
                result.Push(MinMod);
            }
            return;
        }
        /// <summary>
        /// 打印最终结果,用于测试用途
        /// </summary>
        public void TestFunc()
        {
            Console.WriteLine($"The result({result.Count}) is : ");
            foreach (var item in result)
            {
                Console.Write("MinTerms:[");
                foreach (var item2 in item.MinTerm)
                {
                    Console.Write($"{item2},");
                }
                Console.Write("]: ");
                for (var i = 0; i < item.term.Length; i++)
                {
                    Console.Write($"{item.term[i]},");
                }
                Console.WriteLine($"IsSubCube: {item.SubCube}");
            }
        }
    }
}
