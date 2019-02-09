using System;
using System.Collections.Generic;
using System.Text;

namespace My_Neural_Network
{
    public class LayerClass
    {
        int NeuronNumber; //ニューロンの数
        public double[,] Weigth; //重み。受け取る時、つまり入力時にかけられる。 [自身,相手] 
        public double[,] Updated_Weigth; //アップデートされた重み。終わったときにWeigthに上書き
        double[] Bias; //バイアス。入力時
        double[] Delta; //δ

        public double[] Input; //そのままの入力。つまり前回の出力
        double[] Output; //出力
        public bool IsCalced = false; //出力が計算されているか。高速化のため。

        public LayerClass(int NeuronNumber) { //コンストラクタ and 初期化
            this.NeuronNumber = NeuronNumber;
            //Initialize
            Weigth = new double[NeuronNumber, NeuronNumber]; //重み初期化
            Random random = new Random();
            for (int SetWeigthCount_X = 0; SetWeigthCount_X < NeuronNumber; SetWeigthCount_X++)
                for (int SetWeigthCount_Y = 0; SetWeigthCount_Y < NeuronNumber; SetWeigthCount_Y++)
                    Weigth[SetWeigthCount_X, SetWeigthCount_Y] = (double)random.Next(-30, 30) / 30d;

            Updated_Weigth = new double[NeuronNumber, NeuronNumber];
            for (int SetWeigthCount_X = 0; SetWeigthCount_X < NeuronNumber; SetWeigthCount_X++)
                for (int SetWeigthCount_Y = 0; SetWeigthCount_Y < NeuronNumber; SetWeigthCount_Y++)
                    Updated_Weigth[SetWeigthCount_X, SetWeigthCount_Y] = 0; //0で初期化


                    Bias = new double[NeuronNumber]; //バイアス初期化
            for (int SetBiasCount = 0; SetBiasCount < NeuronNumber; SetBiasCount++)
                Bias[SetBiasCount] = 0; //とりあえず0で初期化

            Delta = new double[NeuronNumber]; //デルタ初期化
            for (int SetDeltaCount = 0; SetDeltaCount < NeuronNumber; SetDeltaCount++)
                Delta[SetDeltaCount] = 0; //とりあえ(ry

            Input = new double[NeuronNumber]; //入力初期化

            Output = new double[NeuronNumber]; //出力初期化
        }

        public void SetData(double[] InputData) { //データ入力
            this.Input = (double[])InputData.Clone(); //入力をコピー
        }
        
        public double[] GetData() { //出力
            if (!IsCalced) //処理されていないのであれば
                CalcOut();
            return Output;
        }

        private void CalcOut() { //出力計算
            //入力に重みをかけ、バイアスとの和を求め、活性化関数に通す。
            //double[] Weigthed_Input = new double[NeuronNumber];
            double Sum = 0;
            int OwnCount, IntercessionCount; //高速化のため。
            for (OwnCount = 0; OwnCount < NeuronNumber; OwnCount++) {
                Sum = 0; //合計の初期化
                for (IntercessionCount = 0; IntercessionCount < NeuronNumber; IntercessionCount++)
                    Sum += Input[OwnCount] * Weigth[OwnCount, IntercessionCount] + Bias[OwnCount]; //合計を求める。
                //Weigthed_Input[OwnCount] = Sum; //重み付けされた入力
                Output[OwnCount] = ActFunc(Sum);
            }
            IsCalced = true;
        }

        public double ActFunc(double x) { //活性化関数
            //シグモイド
            return 1d / (1 + Math.Pow(Math.E, -x));
        }

        public void GetDelta_in_lastLayer(double[] Answer) {
            if (!IsCalced) throw new Exception("計算されていないため誤差を取得できません。");
            for (int GetDeltaCount = 0; GetDeltaCount < NeuronNumber; GetDeltaCount++)
                Delta[GetDeltaCount] = Math.Abs(Output[GetDeltaCount] - Answer[GetDeltaCount]);
        }

        public void Update_Weigth() {
            Weigth = (double[,])Updated_Weigth;
        }

        public double Getsumof_Delta_Weigth(int Intercession) {
            double Result = 0;
            for(int GetSumCount = 0;GetSumCount < NeuronNumber; GetSumCount++)
                Result += Delta[GetSumCount] * Weigth[GetSumCount, Intercession];
            return Result;
        }
    }
}
