using UnityEngine;
using Unity.Barracuda;
using System;
using System.Linq;

public class IC : MonoBehaviour
{
    public NNModel model;
    public Texture2D sampleOX;
    private IWorker worker;
    void Start()
    {
        worker = model.CreateWorker();
        using(var input = new Tensor(1, 150, 150, 1))
        {
            for(var y = 0; y < 150; y++) {
                for(var x = 0; x < 150; x++){
                    var tx = x * sampleOX.width / 150;
                    var ty = y * sampleOX.height / 150;
                    var targetPixel = sampleOX.GetPixel(tx, ty);
                    var temp = targetPixel.r * 0.21 + targetPixel.g * 0.72 + targetPixel.b * 0.07;
                    input[0, 149 - y, x, 0] = (float)temp; //for values from 0 - 255
                }
            }
            Tensor outputTensor = worker.Execute(input).PeekOutput("dense_1");
            var outputArr = outputTensor.ToReadOnlyArray();
            Debug.Log(outputArr);
            for(int i=0; i<outputArr.Length; i++) {
                Debug.Log("Arr " + i + " " + outputArr[i]);
            }
            
            if(outputArr[0] > 0.5) {
                 Debug.Log($"Image was recognised as class: X");
             } else {
                 Debug.Log($"Image was recognised as class: O");
             }
            //var indexWithHightestProbability = output[0];
            //Debug.Log($"Image was recognised as class number: " + output[0]);
        }
    }
}
