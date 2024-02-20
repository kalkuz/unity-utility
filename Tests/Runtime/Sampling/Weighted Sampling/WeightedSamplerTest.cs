using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Sampling.WeightedSampling;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Kalkuz.Utility.Tests
{
  public class WeightedSamplerTests
  {
    [UnityTest]
    public IEnumerator TestSample()
    {
      // Define a list of weighted items
      var items = new List<WeightedItem<string>>
      {
        new("apple", 1),
        new("banana", 2),
        new("cherry", 3)
      };

      // Initialize a weighted sampler
      var sampler = new WeightedSampler<string>(items);

      // Generate 10000 samples and count the number of times each item is selected
      var counts = new Dictionary<string, int>();
      for (int i = 0; i < 10000; i++)
      {
        var sample = sampler.Sample();
        if (!counts.ContainsKey(sample))
        {
          counts[sample] = 0;
        }

        counts[sample]++;
      }

      yield return null;

      // Check that the counts are proportional to the weights
      Assert.AreApproximatelyEqual(counts["apple"], 10000f / 6, 200f);
      Assert.AreApproximatelyEqual(counts["banana"], 20000f / 6, 200f);
      Assert.AreApproximatelyEqual(counts["cherry"], 30000f / 6, 200f);
    }
  }
}