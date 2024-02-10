using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 工具集合的命名空间
///二进制转化方案:数组的递增方向与二进制位权递增方向相同 
/// </summary>
namespace MQMethod.utils
{
    internal class Tools
    {
        static Queue<int> tmp = new Queue<int>();

        /// <summary>
        /// 将存储二进制的int数组转化为十进制数字
        /// </summary>
        /// <param name="term">用于存储二进制的int数组</param>
        /// <returns>返回值为转化的结果</returns>
        public static int Bit2int(int[] term)
        {
            int result = 0;
            for(int i = 0; i < term.Length; i++)
            {
                result += (int)(Math.Pow(2, i)) * term[i];
            }
            return result;
        }
        /// <summary>
        /// 将十进制数字转化为二进制数字串
        /// </summary>
        /// <param name="res">用于存储二进制数字串</param>
        /// <param name="val">需要转化的十进制数字</param>
        public static void Int2bit(int[] res,int val)
        {
            tmp.Clear();
            while (val > 0)
            {
                tmp.Enqueue(val%2);
                val /= 2;
            }
            int i = 0;
            while (tmp.Count > 0)
            {
                res[i] = tmp.Dequeue();
                i++;
            }
            return;
        }
        /// <summary>
        /// 将int数组全部清空为0
        /// </summary>
        /// <param name="array">需要被清空的数组</param>
        public static void IntArrayClear(int[] array)
        {
            for(var i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
        }
        /// <summary>
        /// 返回数组中1的个数
        /// </summary>
        /// <param name="array">需要被处理的数组</param>
        /// <returns></returns>
        public static int Return1Number(int[] array)
        {
            int res = 0;
            for(var i = 0; i < array.Length; i++)
            {
                if (array[i]  == 1)
                    res++;
            }
            return res;
        }
        /// <summary>
        /// 对比两个int数组的差别
        /// </summary>
        /// <param name="array1">第一个数组</param>
        /// <param name="array2">第二个数组</param>
        /// <returns>返回不同的个数</returns>
        public static int ReturnDifference(int[] array1, int[] array2)
        {
            int res = 0;
            for(var i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    res++;
            }
            return res;
        }
        /// <summary>
        /// 判断数组是否为递增数组,若不是则调整为递增数组
        /// </summary>
        /// <param name="array">The array.</param>
        public static void AdjustArrayUpper(int[] array)
        {
            bool IsUp = true;
            for(var i = 0;i < array.Length-1; i++)
            {
                if (array[i] > array[i+1])
                {
                    IsUp = false;
                    break;
                }
            }
            if (IsUp) return;
            QuickSort(array);
        }
        /// <summary>
        /// 快速排序算法
        /// </summary>
        /// <param name="array">指定的数组</param>
        static void QuickSort(int[] array)
        {
            quickSort(array, 0, array.Length - 1);
        }
        /// <summary>
        /// 快排组件
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="st">起点</param>
        /// <param name="ed">终点</param>
        static void quickSort(int[] arr, int _left, int _right)
        {
            int left = _left;
            int right = _right;
            int temp = 0;
            if (left <= right)
            {
                temp = arr[left];
                while (left != right)
                {

                    while (right > left && arr[right] >= temp)
                        right--;
                    arr[left] = arr[right];

                    while (left < right && arr[left] <= temp)
                        left++;
                    arr[right] = arr[left];

                }
                arr[right] = temp;
                quickSort(arr, _left, left - 1);
                quickSort(arr, right + 1, _right);
            }
        }
        /// <summary>
            /// 打印数组
            /// </summary>
            /// <param name="array">The array.</param>
        public static void SHowIntArray(int[] array)
        {
            Console.Write("The array: [");
            for(var i = 0;i < array.Length; i++)
            {
                Console.Write($"{array[i]},");
            }
            Console.WriteLine("].");
        }
    }
}
