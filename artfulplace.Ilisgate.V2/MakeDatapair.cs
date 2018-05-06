using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using artfulplace.Ilisgate.V2.Extensions;

namespace artfulplace.Ilisgate.V2
{
    public class MakeDataPair
    {
        public void MakeTagIndex()
        {
            var tagFreq = new Dictionary<string, int>();
            foreach (var file in Directory.GetFiles(""))
            {
                var tags = file.Split(' ');
                foreach (var tag in tags)
                {
                    if (!tagFreq.ContainsKey(tag))
                    {
                        tagFreq[tag] = 0;
                    }
                    tagFreq[tag] += 1;
                }
            }

            var tagData = tagFreq.Select(x => Tuple.Create(x.Key, x.Value)).OrderByDescending(x => x.Item2).Take(5000);
            tagData.Shuffle();

            var tag_index = tagData.Select((x, i) => Tuple.Create(x, i)).ToArray();

            // Save(tag_index);
        }

        public void MakePair()
        {
  //tag_index = pickle.loads(open('tag_index.pkl', 'rb').read())
  //target_size = (224, 224)
  //for name in glob.glob('../datasetdownload/imgs/*.txt'):
  //  img_name = name.replace('.txt', '.jpg')
  //  if not os.path.exists(img_name):
  //    continue

  //  save_name = 'dataset/{}.pkl'.format(img_name.split('/').pop().replace('.jpg', '') + '.pkl')
  //  if os.path.exists(save_name):
  //    continue
  //  img = Image.open(img_name)
  //  try:
  //    img = img.convert('RGB')
  //  except OSError as e:
  //    continue


  //  w, h = img.size
  //  if w > h :
  //    blank = Image.new('RGB', (w, w))
  //  if w <= h :
  //    blank = Image.new('RGB', (h, h))
  //  blank.paste(img, (0, 0))
  //  blank = blank.resize(target_size)
  //  X = np.asanyarray(blank)
  //  X = X / 255.0
  //  y = [0.0] * len(tag_index)
  //  for tag in open(name).read().split():
  //    if tag_index.get(tag) is None:
  //      continue
  //    index = tag_index[tag]
  //    y[index] = 1.0
  //  if all(map(lambda x: x == 0.0, y)):
  //    continue
  //  #print( X )
  //  #print( y )
  //  print(name)

  //  open(save_name, 'wb').write(pickle.dumps((X, y)))
        }
    }
}
