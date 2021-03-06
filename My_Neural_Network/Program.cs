﻿using System;


namespace My_Neural_Network
{
    class Program
    {
        static void Main(string[] args) {
            int Depth = 3; //ネットワークの深さ
            int NeuronNumber = 3; //各レイヤーのニューロン数
            double Epsilon = 0.1;

            LayerClass[] Tissue = new LayerClass[Depth];
            for (int LayerCount = 0; LayerCount < Depth; LayerCount++)
                Tissue[LayerCount] = new LayerClass(NeuronNumber);

            //教師データを作成する。            
            TrainData<double> trainData = new TrainData<double>(4,3);
            
            for (int MakeTrainDataCount = 0;MakeTrainDataCount < 4; MakeTrainDataCount++) {
                double[] inData = { (MakeTrainDataCount / 2), MakeTrainDataCount % 2, 0 };
                double[] outData = { inData[0] * inData[1], (inData[0] != inData[1]) ? 1 : 0, 0 };
                //0 0 0 | 0 0 0
                //0 1 0 | 0 1 0 
                //1 0 0 | 0 1 0
                //1 1 0 | 1 0 0
                trainData.InputData[MakeTrainDataCount] = inData;
                trainData.AnswerData[MakeTrainDataCount] = outData;
            }
            /*
            for (int MakeTrainDataCount = 0; MakeTrainDataCount < 8; MakeTrainDataCount++) {
                double[] inData = {(int)(MakeTrainDataCount / 4), ((int)(MakeTrainDataCount / 2) % 2), MakeTrainDataCount % 2};
                double[] outData = {(inData[0] + inData[1] + inData[2]) >= 2 ? 1 : 0, (inData[0] + inData[1] + inData[2]) % 2, 0 };
                //0 0 0 | 0 0 0
                //0 1 0 | 0 1 0 
                //1 0 0 | 0 1 0
                //1 1 0 | 1 0 0
                trainData.InputData[MakeTrainDataCount] = inData;
                trainData.AnswerData[MakeTrainDataCount] = outData;
            }
            */

            double[] InputData = { 0, 0, 0 }; //ネットワークに入力するデータ
            for (int TrainNumber = 0; TrainNumber < 10000; TrainNumber++) { //学習回数
                for (InputData[0] = 0; InputData[0] <= 1; InputData[0]++) {
                    for (InputData[1] = 0; InputData[1] <= 1; InputData[1]++) {
                        //for (InputData[2] = 0; InputData[2] <= 1; InputData[2]++) {
                        //それぞれのケースで学習させるため

                        //すべてのレイヤーのIsCalcedをfalseにする。
                        for (int TissueCount = 0; TissueCount < Depth; TissueCount++)
                            Tissue[TissueCount].IsCalced = false;

                        //順方向でネットワークに信号の流す。
                        double[] Data = (double[])InputData.Clone(); //ネットワークに流れるデータ
                        for (int Learning_Network_Count = 0; Learning_Network_Count < Depth; Learning_Network_Count++) {
                            Tissue[Learning_Network_Count].SetData(Data);
                            Data = Tissue[Learning_Network_Count].GetData();
                        }

                        double[] Answer = trainData.GetAnswer(InputData);

                        /*
                        foreach (double x in InputData)
                            Console.Write(x + ":");
                        Console.Write("||");
                        foreach (double x in Answer)
                            Console.Write(x + ":");
                        Console.WriteLine();
                        */

                        //最後のレイヤーを学習させる。
                        double[] LastLayerOutput = Tissue[Depth - 1].GetData(); //最後の出力を取得

                        /*
                        foreach (double x in LastLayerOutput)
                            Console.Write(x + ":");
                        Console.WriteLine();
                        */

                        for (int OwnCount = 0; OwnCount < NeuronNumber; OwnCount++) {
                            double Differentiated_Value
                                = (LastLayerOutput[OwnCount] - Answer[OwnCount])
                                * LastLayerOutput[OwnCount] * (1 - LastLayerOutput[OwnCount]); //最後の部分を除いて共通の部分の微分値を計算
                                                                                               //(g(u[j][l]) - t[j]) * g(u[j][l]) * (1 - g(u[j][l])
                            for (int IntercessionCount = 0; IntercessionCount < NeuronNumber; IntercessionCount++) {
                                Tissue[Depth - 1].Updated_Weigth[OwnCount, IntercessionCount]
                                    = Tissue[Depth - 1].Weigth[OwnCount, IntercessionCount]
                                    - Epsilon * Differentiated_Value * Tissue[Depth - 1].Input[IntercessionCount];
                                //重みを計算してUpdated_Weigthに格納
                            }
                        }

                        //中間層の学習
                        Tissue[Depth - 1].GetDelta_in_lastLayer(Answer); //最後のレイヤーのδを取得

                        /*
                        foreach (double x in Tissue[Depth - 1].Delta) {
                            Console.Write(x + ":");
                        }
                        Console.WriteLine();
                        */

                        for (int TrainLayerCount = Depth - 2; TrainLayerCount >= 0; TrainLayerCount--) { //逆誤差伝播法
                            double[] TrainTmpOutput = Tissue[TrainLayerCount].GetData(); //そのレイヤーのアウトプット、つまりg(u)を取得
                            for (int TrainNeuronOwnCount = 0; TrainNeuronOwnCount < NeuronNumber; TrainNeuronOwnCount++) { //各ニューロンに対して
                                Tissue[TrainLayerCount].Delta[TrainNeuronOwnCount] //自身のδを計算
                                    = Tissue[TrainLayerCount + 1].Getsumof_Delta_Weigth(TrainNeuronOwnCount)
                                    * TrainTmpOutput[TrainNeuronOwnCount] * (1 - TrainTmpOutput[TrainNeuronOwnCount]);

                                for (int TrainNeuronIntercessionCount = 0; TrainNeuronIntercessionCount < NeuronNumber; TrainNeuronIntercessionCount++) { //相手(ひとつ前大しての重みを計算)
                                    Tissue[TrainLayerCount].Updated_Weigth[TrainNeuronOwnCount, TrainNeuronIntercessionCount]
                                        = Tissue[TrainLayerCount].Weigth[TrainNeuronOwnCount, TrainNeuronIntercessionCount]
                                        - Epsilon * Tissue[TrainLayerCount].Delta[TrainNeuronOwnCount]
                                        * Tissue[TrainLayerCount].Input[TrainNeuronIntercessionCount];
                                }
                            }
                        }

                        for (int UpdateWeigthCount = 0; UpdateWeigthCount < Depth; UpdateWeigthCount++) //重み更新
                            Tissue[UpdateWeigthCount].Update_Weigth();

                        /*
                        if (TrainNumber % 1000 != 0)
                            continue;
                        for (int count = 0; count < NeuronNumber; count++)
                            Console.Write(Tissue[Depth - 1].Delta[count] + ":");
                        Console.WriteLine(TrainNumber);
                        */

                        /*
                        for (int tc = 0; tc < Depth; tc++) {
                            Console.WriteLine("Layer:" + tc);
                            for (int oc = 0; oc < NeuronNumber; oc++) {
                                for (int ic = 0; ic < NeuronNumber; ic++)
                                    Console.Write(oc + " to " + ic + " : " + Tissue[tc].Weigth[oc, ic] + ",");
                            }
                        }
                        Console.WriteLine("@@@\n");
                        */
                    }
                }
            }

            for (int d = 0; d < 4; d++) {
                double[] tmp = trainData.InputData[d];
                double[] Answerdata = trainData.GetAnswer(tmp);

                /*
                foreach (double x in tmp)
                    Console.Write(x + ":");
                Console.WriteLine();
                foreach (double x in Answerdata)
                    Console.Write(x + ":");
                Console.WriteLine();
                */

                for (int i = 0; i < Depth; i++) {
                    Tissue[i].SetData(tmp);
                    tmp = Tissue[i].GetData();
                }

                Tissue[Depth - 1].GetDelta_in_lastLayer(Answerdata);
                //tmp = Tissue[Depth - 1].Delta;
                foreach (double da in tmp)
                    Console.Write(da + ":");
                Console.WriteLine();
            }

            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
