// Inception TensorFlow-Serving client in C#

using UnityEngine;
using System.Collections;
using Grpc.Core; //https://github.com/grpc/grpc/tree/master/src/csharp/experimental
using Tensorflow.Serving;
using TensorFlowServing.Utils;
using System;
using System.IO;
using System.Collections.Generic;


public class Client : MonoBehaviour {

    // textures
   	public Texture2D backgroundTexture;
    public Texture2D imageTexture;
    public GUISkin skin;
    public GUIStyle style;
    byte[] imageBytes;

    string ipAddress = "100.64.156.205"; // IP address of TF-Server
    string port = "8500"; // port of TF-Server
    string resultString = ""; // final result string

    List<Tuple<string, float>> SendAndReceive(byte[] imageData)
    {
        var tf_channel = new Channel(ipAddress, Convert.ToInt32(port), ChannelCredentials.Insecure);
        var tf_client = new PredictionService.PredictionServiceClient(tf_channel);

        try
        {
            //Create prediction request
            var request = new PredictRequest()
            {
                ModelSpec = new ModelSpec()
                {
                    Name = "inception",
                    SignatureName = "predict_images"
                }
            };

            var imageTensor = TensorProtoBuilder.TensorProtoFromImage(imageData);

            // Add image tensor to request
            request.Inputs.Add("images", imageTensor);
            // Send request and get response
            PredictResponse predictResponse = tf_client.Predict(request);
            // Decode response
            var classesTensor = predictResponse.Outputs["classes"];
            string[] classes = TensorProtoDecoder.TensorProtoToStringArray(classesTensor);

            var scoresTensor = predictResponse.Outputs["scores"];
            float[] scores = TensorProtoDecoder.TensorProtoToFloatArray(scoresTensor);

            List<Tuple<string, float>> predictResult = new List<Tuple<string, float>>();

            for (int i = 0; i < classes.Length; i++)
            {
                predictResult.Add(new Tuple<string, float>(classes[i], scores[i]));
            }
            return predictResult;

       
        }
        catch (Exception e)
        {
            if (e is RpcException)
            {
                RpcException re = (RpcException)e;
                Debug.Log(re.Status.Detail);
                Debug.Log(re.StatusCode);
            }
            Debug.Log(e.Message);
            throw;
        }
    }

    private void Start()
    {
        imageBytes = File.ReadAllBytes("Assets/example1.jpg");
        imageTexture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        imageTexture.LoadImage(imageBytes);
        backgroundTexture = Texture2D.whiteTexture;
        style.normal.background = new Texture2D(1, 1);
   
    }

    void OnGUI() {
        

        // Draws a single image in a square the size of the screen
        int width = Screen.width;
		int height = Screen.height;
		int left = 0;
		int top  = 0;


        if (width > height)
			width = height;
		else
			height = width;

		left = Screen.width/2 - width/2;
		top = Screen.height/2 - height/2;

        int imageWidth = 400;
        int imageHeight = 400;
        int imageLeftPoint = left + width/2 - imageWidth/2;
        int imageTopPoint = top+10;

        GUI.DrawTexture(new Rect(left, top, width, height), backgroundTexture);
        GUI.DrawTexture(new Rect(imageLeftPoint, imageTopPoint, imageWidth, imageHeight), imageTexture);

        if (GUI.Button(new Rect(imageLeftPoint, imageTopPoint + imageHeight + 10, imageWidth, 20), "Predict"))
        {
            string displayString = "";
            var resultList = SendAndReceive(imageBytes);    
            foreach (Tuple<string, float> tuple in resultList)
            {
                displayString = displayString + tuple.Item1 + ":" + tuple.Item2 + "\n";
            }
            resultString = displayString;
        }

        GUI.Box(new Rect(imageLeftPoint, imageTopPoint + imageHeight + 50, imageWidth, 200), resultString, style);

    }
}
