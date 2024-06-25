using HslCommunication.BasicFramework;
using static System.Net.Mime.MediaTypeNames;

namespace web.Helpers
{
    
    public static class SerialnumHelper
    {
        // private static string StartupPath = "C:\\Works\\GitWare\\2023-TJU-DBD-Police-Affair-System\\back";
        private static HslCommunication.BasicFramework.SoftNumericalOrder? softNumericalOrder;
        public static void FormSeqTest_Load()
        {
                softNumericalOrder = new HslCommunication.BasicFramework.SoftNumericalOrder(
                "P",                 // "P2023090000001" 中的P前缀，代码中仍然可以更改为其他的
                "yyyyMM",            // "P2023090000001" 中的20230909，可以格式化时间，也可以为""，也可以设置为"yyyyMMddHHmmss";
                7,                  // "P2023090000001" 中的0000001，总位数为7，然后不停的累加，即使日期时间变了，也不停的累加，最好长度设置大一些
                 "./numericalOrder.txt"  // 该生成器会自动存储当前值到文件去，实例化时从文件加载，自动实现数据同步
                );
        }

        public static string GetSerialNum(string Head)
        {

            if (softNumericalOrder != null)
                return softNumericalOrder.GetNumericalOrder(Head);
            else
                return "Error";
        }
    }

}
