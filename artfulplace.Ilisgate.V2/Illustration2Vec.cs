using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KerasSharp.Layers;
using KerasSharp.Models;
using KerasSharp.Optimizers;
using KerasSharp;
using KerasSharp.Losses;
using KerasSharp.Activations;
using artfulplace.Ilisgate.V2.Extensions;
using artfulplace.Ilisgate.V2.KerasExtension;

namespace artfulplace.Ilisgate.V2
{
    public class Illustration2Vec
    {

        private Model model { get; set; }

        public Illustration2Vec()
        {
            var inputTensor = new InputLayer(new int?[] { 224, 224, 3 });

            var vgg16_model = VGG16Model.CreateModel("");
            foreach (var layer in vgg16_model.layers.Take(12))
            {
                layer.trainable = false;
            }

            var x = vgg16_model.layers.Last().output;
            // List <KerasSharp.Engine.Topology.Tensor> x = null;
            x = new Flatten().Call(x);
            x = new BatchNormalization().Call(x);
            x = new Dense(5000, activation:"relu").Call(x);
            x = new Dropout(0.3).Call(x);
            x = new Dense(5000, activation: "sigmoid").Call(x);

            model = new Model(vgg16_model.input, x, "altI2v");
            model.Compile(new Adam(), new BinaryCrossEntropy());
        }

        public void Train()
        {
            foreach (var i in Enumerable.Range(0, 500))
            {
                var names = Directory.GetFiles("", "*.pkl");
                names = names.Shuffle().ToArray();

                var Xs = new int[0];
                var Ys = new int[0];

                var idx = 0;
                foreach (var name in names)
                {
                    Array X = null;
                    try
                    {
                        X = null; // pickle.Load();
                    }
                    catch
                    {
                        continue;
                    }

                    if (idx >= 15000)
                    {
                        break;
                    }
                    //  Xs.append(X)
                    //  ys.append(y)

                   
                }

                //Xs = np.array(Xs)
                //ys = np.array(ys)
                model.fit(Xs, Ys);
                model.SaveWeights("");
            }
        }

        public void Pred()
        {
            // set last trained file;
            model.LoadWeightsH5("");

            // pickles読み込む
            var tag_index = new List<string>(); // pickle.loads(open('make_datapair/tag_index.pkl', 'rb').read())
            var index_tag = tag_index.Select((x, i) => Tuple.Create(i, x)).ToDictionary(x => x.Item1, x => x.Item2);


            foreach (var file in Directory.GetFiles(""))
            {
                // pickle.loads(open(name,'rb').read() )
                Array X = null;
                var result = model.predict(X);
                var result2 = result[0];
                var result3 = result2.Cast<double>().Select((w, i) => Tuple.Create(i, w)).ToArray();
                foreach (var res in result3.OrderByDescending(x => x.Item2).Take(30))
                {
                    Console.WriteLine($"{file} tag={index_tag[res.Item1]} prob={res.Item2}");
                }

            }


            
        }

        ///
        /// https://github.com/GINK03/alt-i2v/blob/master/alt_i2v_V2.py
        /// 

        /*

        import keras
from keras.preprocessing.image import ImageDataGenerator
from keras.models import Sequential, Model, load_model
from keras.layers import Input, Activation, Dropout, Flatten, Dense, Reshape, merge
from keras.preprocessing.image import ImageDataGenerator
from keras import optimizers
from keras.preprocessing.image import ImageDataGenerator
from keras.layers.normalization import BatchNormalization as BN
from keras.layers.core import Dropout
from keras.applications.vgg16 import VGG16 
import numpy as np
import os
from PIL import Image
import glob 
import pickle
import sys
import random
import msgpack
import numpy as np
import json

input_tensor = Input(shape=(224, 224, 3))
vgg16_model = VGG16(include_top=False, weights='imagenet', input_tensor=input_tensor)
for layer in vgg16_model.layers[:12]: # default 15
  layer.trainable = False
x = vgg16_model.layers[-1].output 
x = Flatten()(x)
x = BN()(x)
x = Dense(5000, activation='relu')(x)
x = Dropout(0.3)(x)
x = Dense(5000, activation='sigmoid')(x)
model = Model(input=vgg16_model.input, output=x)
model.compile(loss='binary_crossentropy', optimizer='adam')

def train():
  for i in range(500):
    print('now iter {} load pickled dataset...'.format(i))
    Xs = []
    ys = []
    // for (var name in enumerate), for (var idx in name) って感じ
    names = [name for idx, name in enumerate( glob.glob('../dataset/*.pkl') )]
    random.shuffle( names )
    for idx, name in enumerate(names):
      try:
        X,y = pickle.loads(open(name,'rb').read() ) 
      except EOFError as e:
        continue
      if idx%100 == 0:
        print('now scan iter', idx)
      if idx >= 15000:
        break
      Xs.append( X )
      ys.append( y )

    Xs = np.array( Xs )
    ys = np.array( ys )
    model.fit(Xs, ys, epochs=1 )
    print('now iter {} '.format(i))
    model.save_weights('models/{:09d}.h5'.format(i))

def pred():
  // """
  // tag_index = pickle.loads(open('tag_index.pkl', 'rb').read())
  // index_tag = { index:tag for tag, index in tag_index.items() }
  // name_img150 = []
  // for name in filter(lambda x: '.jpg' in x, sys.argv):
  //   img = Image.open('{name}'.format(name=name))
  //   img = img.convert('RGB')
  //   img150 = np.array(img.resize((150, 150)))
  //   name_img150.append( (name, img150) )
  // """
  model.load_weights(sorted(glob.glob('models/*.h5'))[-1]) 

  tag_index = pickle.loads( open('make_datapair/tag_index.pkl', 'rb').read() )
  index_tag = { index:tag for tag,index in tag_index.items() }


  for name in glob.glob('./make_datapair/dataset/*'):
    X, y = pickle.loads( open(name,'rb').read() )
    result = model.predict(np.array([X]) )
    result = result.tolist()[0]
    result = { i:w for i,w in enumerate(result)}
    for i,w in sorted(result.items(), key=lambda x:x[1]*-1)[:30]:
      print("{name} tag={tag} prob={prob}".format(name=name, tag=index_tag[i], prob=w) )

if __name__ == '__main__':
  if '--train' in sys.argv:
    train()
  if '--pred' in sys.argv:
    pred()

        */


    }
}
