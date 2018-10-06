using Tensorflow;
using UnityEngine;

namespace TensorFlowServing.Utils
{
    class TensorProtoDecoder
    {

        public static float TensorProtoToFloat(TensorProto tesnorProto)
        {
            return tesnorProto.FloatVal[0];
        }

        public static float[] TensorProtoToFloatArray(TensorProto tesnorProto, int arrayLen=0)
        {
            int receivedArrayLen = tesnorProto.FloatVal.Count;
            if (arrayLen == 0)
            {
                arrayLen = receivedArrayLen;
            }
            else
            {
                if (arrayLen != receivedArrayLen)
                {
                    throw new System.IndexOutOfRangeException("Expected array length does not match received length");
                }
            }
            float[] floatArray = new float[arrayLen];
            tesnorProto.FloatVal.CopyTo(floatArray, 0);
            return floatArray;
        }

        public static string[] TensorProtoToStringArray(TensorProto tesnorProto, int arrayLen = 0)
        {
            int receivedArrayLen = tesnorProto.StringVal.Count;
            if (arrayLen == 0)
            {
                arrayLen = receivedArrayLen;
            }
            else
            {
                if (arrayLen != receivedArrayLen)
                {
                    throw new System.IndexOutOfRangeException("Expected array length does not match received length");
                }
            }
            string[] stringArray = new string[arrayLen];
            Google.Protobuf.ByteString[] byteStringArray = new Google.Protobuf.ByteString[arrayLen];
            tesnorProto.StringVal.CopyTo(byteStringArray, 0);
            for(int i = 0; i < arrayLen; i++)
            {
                stringArray[i] = byteStringArray[i].ToString(System.Text.Encoding.UTF8);
            }
            return stringArray;
        }

    }
}
