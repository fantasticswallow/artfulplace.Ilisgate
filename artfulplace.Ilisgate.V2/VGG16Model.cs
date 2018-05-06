using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KerasSharp.Models;
using KerasSharp.Layers;
using KerasSharp.Activations;
using KerasSharp.Engine.Topology;
using KerasSharp;
using artfulplace.Ilisgate.V2.Extensions;

namespace artfulplace.Ilisgate.V2.KerasExtension
{
    /// <summary>
    /// VGG16なモデル
    /// </summary>
    public static class VGG16Model
    {
        // https://github.com/keras-team/keras/blob/49f5b931410bc2e56378f20a15e8ac919e0efb88/keras/applications/vgg16.py
        // WEIGHTS_PATH_NO_TOP = 'https://github.com/fchollet/deep-learning-models/releases/download/v0.1/vgg16_weights_tf_dim_ordering_tf_kernels_notop.h5'


        /// <summary>
        /// モデル作って返します。
        /// </summary>
        /// <returns></returns>
        public static Model CreateModel(string weightFile)
        {
            var img_input = Python.Input(new int?[] { 224, 224, 3 });
            var x = new Conv2D(64, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(img_input);
            x = new Conv2D(64, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new MaxPooling2D(new int[] { 2, 2 }).Call(x);

            x = new Conv2D(128, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(128, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new MaxPooling2D(new int[] { 2, 2 }).Call(x);

            x = new Conv2D(256, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(256, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(256, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new MaxPooling2D(new int[] { 2, 2 }).Call(x);

            x = new Conv2D(512, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(512, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(512, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new MaxPooling2D(new int[] { 2, 2 }).Call(x);

            x = new Conv2D(512, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(512, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new Conv2D(512, new[] { 3, 3 }, activation: new ReLU(), padding: PaddingType.Same).Call(x);
            x = new MaxPooling2D(new int[] { 2, 2 }).Call(x);

            
            var model = new Model(img_input, x, "vgg16");

            model.LoadWeightsH5(weightFile);

            return model;
        }

    }
}
