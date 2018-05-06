using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KerasSharp.Models;
using KerasSharp.Engine.Topology;
using artfulplace.Ilisgate.V2.KerasExtension;
using System.IO;

namespace artfulplace.Ilisgate.V2.Extensions
{
    /// <summary>
    /// Kerasのモデルの保存、読み込みを行う拡張メソッドを提供します。
    /// </summary>
    public static class KerasModelSavingExtensions
    {
        // https://github.com/keras-team/keras/blob/master/keras/engine/saving.py
        // https://github.com/keras-team/keras/blob/3b440235e237ef59ec5763c413e7f4292dab5d79/keras/engine/network.py

        public static void LoadWeightsH5(this Model model, string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                var data = DataParser.CreateDataFromJson<WeightModel>(sr.ReadToEnd());

                LoadKerasLayers(data, model.layers);
            }
        }

        public static void SaveWeights(this Model model, string filePath)
        {
            var weightModel = SaveKerasLayers(model.layers);
            var data = DataParser.WriteObjectToJson(weightModel);

            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(data);
            }
        }

        private static WeightModel SaveKerasLayers(List<KerasSharp.Engine.Topology.Layer> layers)
        {
            var weightModel = new WeightModel();
            weightModel.WeightValueDictionary = new Dictionary<string, Array>();
            weightModel.LayerNames = layers.Select(x => x.name).ToArray();

            foreach (var layer in layers)
            {
                var g = layer.name;
                var weightValues = KerasSharp.Backends.Current.K.batch_get_value(layer.weights);

                List<string> weightNames = new List<string>();
                for (var i = 0; i < weightValues.Count; i++)
                {
                    var w = layer.weights[i];
                    var val = weightValues[i];

                    var name = "";
                    if (!string.IsNullOrEmpty(w.name))
                    {
                        name = w.name;
                    }
                    else
                    {
                        name = $"param_{i}";
                    }
                    weightNames.Add(name);

                    weightModel.WeightValueDictionary.Add(name, val);
                }

                weightModel.WeightNameDictionary.Add(g, weightNames);
            }

            return weightModel;
        }

        private static void LoadKerasLayers(WeightModel model, List<Layer> layers)
        {
            var weightValueTuples = new List<Tuple<Tensor, Array>>();
            foreach (var layer in layers)
            {
                var weightNames = model.WeightNameDictionary[layer.name];
                
                var symbolic_weights = layer.weights;
                var weightValues = weightNames.Select(n => model.WeightValueDictionary[n]).ToArray();

                weightValueTuples.AddRange(symbolic_weights.Select((w, i) => Tuple.Create(w, weightValues[i])));
            }

            KerasSharp.Backends.Current.K.batch_set_value(weightValueTuples);
        }

    }


/*
    def load_weights(self, filepath, by_name= False,
                    skip_mismatch= False, reshape= False):
        """Loads all layer weights from a HDF5 save file.
        If `by_name` is False(default) weights are loaded
       based on the network's topology, meaning the architecture
        should be the same as when the weights were saved.
        Note that layers that don't have weights are not taken
        into account in the topological ordering, so adding or
        removing layers is fine as long as they don't have weights.
        If `by_name` is True, weights are loaded into layers
        only if they share the same name.This is useful
        for fine-tuning or transfer-learning models where
        some of the layers have changed.
# Arguments
            filepath: String, path to the weights file to load.
            by_name: Boolean, whether to load weights by name
                or by topological order.
            skip_mismatch: Boolean, whether to skip loading of layers
                where there is a mismatch in the number of weights,
                or a mismatch in the shape of the weight
                (only valid when `by_name`=True).
            reshape: Reshape weights to fit the layer when the correct number
                of weight arrays is present but their shape does not match.
# Raises
            ImportError: If h5py is not available.
        """
        if h5py is None:
            raise ImportError('`load_weights` requires h5py.')
        with h5py.File(filepath, mode= 'r') as f:
            if 'layer_names' not in f.attrs and 'model_weights' in f:
                f = f['model_weights']
            if by_name:
                saving.load_weights_from_hdf5_group_by_name(
                    f, self.layers, skip_mismatch=skip_mismatch,
                    reshape=reshape)
            else:
                saving.load_weights_from_hdf5_group(
                    f, self.layers, reshape=reshape)
*/
}
