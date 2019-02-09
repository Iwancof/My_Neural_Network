using System;
using System.Collections.Generic;
using System.Text;

namespace My_Neural_Network
{
    public class LayerClass
    {
        int NeuronNumber; //ニューロンの数
        double[,] Weigth; //重み。受け取る時、つまり入力時にかけられる。 [自身,相手] 
        double[] Bias; //バイアス。入力時
        double[] Delta; //δ

        double[] Input; //そのままの入力。つまり前回の出力
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
        }

        public double ActFunc(double x) {
            //シグモイド
            return 1d / (1 + Math.Pow(Math.E, -x));
        }

    }
}
