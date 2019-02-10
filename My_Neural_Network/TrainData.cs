using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace My_Neural_Network
{
    public class TrainData<T>
    {
        public T[][] InputData; //入力データ
        public T[][] AnswerData; //教師データ
        int Number_Of_Data;
        int Number_Of_EachDatas;

        public TrainData(int Number_Of_Data,int Number_Of_EachDatas) { //データの数
            this.Number_Of_Data = Number_Of_Data;
            this.Number_Of_EachDatas = Number_Of_EachDatas;
            InputData = new T[Number_Of_Data][];
            AnswerData = new T[Number_Of_Data][];
            for(int SetDataCount = 0;SetDataCount < Number_Of_Data; SetDataCount++) {
                InputData[SetDataCount] = new T[Number_Of_EachDatas];
                AnswerData[SetDataCount] = new T[Number_Of_EachDatas];
            }
        }

        public T[] GetAnswer(T[] Data) { //入力から答えを出す。
            int ResultCount;
            for(ResultCount = 0;ResultCount < Number_Of_Data; ResultCount++) {
                if (InputData[ResultCount].SequenceEqual(Data))
                    break;
                if (ResultCount == Number_Of_Data - 1)
                    throw new Exception("教師データが見つかりません。");
            }
            //Console.WriteLine(ResultCount);
            return AnswerData[ResultCount];
        }
    }
}
