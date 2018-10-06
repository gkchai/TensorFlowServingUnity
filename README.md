# TensorFlow Serving Unity Application

Example Unity application using TensorFlow Serving. Predicts classes for an Image in Unity 
using [Inception](https://github.com/tensorflow/models/tree/master/research/inception) machine learning model.

## TensorFlow Serving Server
- Follow instructions [here](https://www.tensorflow.org/serving/serving_inception) to download
and run TF-Serving Docker with Inception model server.

## TensorFlow Serving Unity Client
- `Assets/Client.cs` is the Unity `C#` script with the request and response stub connecting 
to the server.

## Examples
![example1](https://github.com/gkchai/TensorFlowServingUnity/blob/master/ex1.JPG?raw=true "Example 1")

![example2](https://github.com/gkchai/TensorFlowServingUnity/blob/master/ex2.JPG?raw=true "Example 2")
