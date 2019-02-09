using System;


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
            for(int MakeTrainDataCount = 0;MakeTrainDataCount < 4; MakeTrainDataCount++) {
                double[] inData = { (MakeTrainDataCount / 2), MakeTrainDataCount % 2, 0 };
                double[] outData = { inData[0] * inData[1], (inData[0] != inData[1]) ? 1 : 0, 0 };
                //0 0 0 | 0 0 0
                //0 1 0 | 0 1 0 
                //1 0 0 | 0 1 0
                //1 1 0 | 1 0 0
                trainData.InputData[MakeTrainDataCount] = inData;
                trainData.AnswerData[MakeTrainDataCount] = outData;
            }

            double[] InputData = { 0, 0, 0 }; //ネットワークに入力するデータ
            for(int TrainNumber = 0;TrainNumber < 10000; TrainNumber++) { //学習回数
                for(InputData[0] = 0;InputData[0] <= 1; InputData[0]++) { 
                    for(InputData[1] = 0;InputData[1] <= 1; InputData[1]++) {
                        //それぞれのケースで学習させるため

                        //すべてのレイヤーのIsCalcedをfalseにする。
                        for (int TissueCount = 0; TissueCount < Depth; TissueCount++)
                            Tissue[TissueCount].IsCalced = false;

                        //順方向でネットワークに信号の流す。
                        double[] Data = (double[])InputData.Clone(); //ネットワークに流れるデータ
                        for(int Learning_Network_Count = 0;Learning_Network_Count < Depth; Learning_Network_Count++) {
                            Tissue[Learning_Network_Count].SetData(Data);
                            Data = Tissue[Learning_Network_Count].GetData();
                        }

                        double[] Answer = trainData.GetAnswer(InputData);

                        //最後のレイヤーを学習させる。
                        double[] LastLayerOutput = Tissue[Depth - 1].GetData(); //最後の出力を取得
                        for (int OwnCount = 0; OwnCount < NeuronNumber; OwnCount++) {
                            double Differentiated_Value = (LastLayerOutput[OwnCount] - Answer[OwnCount]) * LastLayerOutput[OwnCount] * (1 - LastLayerOutput[OwnCount]); //最後の部分を除いて共通の部分の微分値を計算
                            //(g(u[j][l]) - t[j]) * g(u[j][l]) * (1 - g(u[j][l])
                            for (int IntercessionCount = 0; IntercessionCount < NeuronNumber; IntercessionCount++) {
                                Tissue[Depth - 1].Updated_Weigth[OwnCount, IntercessionCount] = Tissue[Depth - 1].Weigth[OwnCount, IntercessionCount] - Epsilon * Differentiated_Value * Tissue[Depth - 2].Input[IntercessionCount];
                                //重みを計算してUpdated_Weigthに格納
                            }
                        }

                        //中間層の学習
                        Tissue[Depth - 1].GetDelta_in_lastLayer(Answer); //最後のレイヤーのδを取得
                        for (int TrainLayerCount = Depth - 2;/*最後にひとつ前*/TrainLayerCount >= 0; TrainLayerCount--) { //逆誤差伝播法
                            double[] TrainTmpOutput = Tissue[TrainLayerCount].GetData(); //そのレイヤーのアウトプット、つまりg(u)を取得
                            for (int TrainNeuronOwnCount = 0; TrainNeuronOwnCount < NeuronNumber; TrainNeuronOwnCount++) { //各ニューロンに対して
                                Tissue[TrainLayerCount].Delta[TrainNeuronOwnCount] = Tissue[TrainLayerCount + 1].Getsumof_Delta_Weigth(TrainNeuronOwnCount) * TrainTmpOutput[TrainNeuronOwnCount] * (1 - TrainTmpOutput[TrainNeuronOwnCount]);

                                for (int TrainNeuronIntercessionCount = 0; TrainNeuronIntercessionCount < NeuronNumber; TrainNeuronIntercessionCount++) {
                                    Tissue[TrainLayerCount].Updated_Weigth[TrainNeuronOwnCount, TrainNeuronIntercessionCount]
                                        = Tissue[TrainLayerCount].Delta[TrainNeuronOwnCount] * Tissue[TrainLayerCount].Input[TrainNeuronIntercessionCount];
                                }
                            }
                        }

                        for (int UpdateWeigthCount = 0; UpdateWeigthCount < Depth; UpdateWeigthCount++)
                            Tissue[UpdateWeigthCount].Update_Weigth();

                        foreach(double delta in Tissue[Depth - 1].Delta) {
                            Console.Write(delta.ToString());
                        }
                        Console.WriteLine();
                    }
                }
            }



        }
    }


}
