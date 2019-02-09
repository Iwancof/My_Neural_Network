using System;


namespace My_Neural_Network
{
    class Program
    {
        static void Main(string[] args) {
            int Depth = 3; //ネットワークの深さ
            int NeuronNumber = 3; //各レイヤーのニューロン数

            LayerClass[] Tissue = new LayerClass[Depth];
            for (int LayerCount = 0; LayerCount < Depth; LayerCount++)
                Tissue[LayerCount] = new LayerClass(NeuronNumber);

            //教師データを作成する。            
        }
    }


}
