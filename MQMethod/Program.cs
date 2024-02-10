using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MQMethod.Service;
using MQMethod.utils;

//Main Entrence--------------------------------
//选择数据文件
string fileName = "test.json";
//创建文件处理器并开始吸收数据
InputDataHandler inputDataHandler = new InputDataHandler(fileName);
//创建数据结构,注入文件数据
DataStructure dataStructure = new DataStructure(inputDataHandler.Variable_number, inputDataHandler.Logic_Function, inputDataHandler.UnRelated_terms);
//数据模型初始化与创建
dataStructure.StructureInitilize();
//算法模块创建与初始化
MQalgorithm Method = new MQalgorithm(dataStructure);
//运行算法
Method.Run();
//优化模块创建
MQOptimize optimize = new MQOptimize(Method.models, inputDataHandler.Logic_Function);
//优化算法运行
optimize.Run();

optimize.TestFunc();



//Debug窗口相关
Console.WriteLine("Press enter to close...");
Console.ReadLine();
//Self-defined DataStrcuture----------------