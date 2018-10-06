using Tensorflow;

namespace TensorFlowServing.Utils
{
	public class TensorProtoBuilder
	{
		public static TensorProto TensorProtoFromImage(byte[] imageData)
		{
			var imageFeatureShape = new TensorShapeProto();
			imageFeatureShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = 1 });

			var imageTensorBuilder = new TensorProto();
			imageTensorBuilder.Dtype = DataType.DtFloat;
			imageTensorBuilder.TensorShape = imageFeatureShape;
            imageTensorBuilder.StringVal.Add(Google.Protobuf.ByteString.CopyFrom(imageData));
            imageTensorBuilder.Dtype = DataType.DtString;

            return imageTensorBuilder;
		}
	}
}