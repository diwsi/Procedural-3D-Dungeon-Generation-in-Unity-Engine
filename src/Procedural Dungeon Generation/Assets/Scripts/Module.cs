using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Module : MonoBehaviour
    {

        System.Random rnd;
        public GameObject[] Outputs;
        public List<GameObject> AvailableOutputs { get; set; }
     
        public GameObject GetOutput()
        {
            //Get an output and seal
            var op = AvailableOutputs.ElementAt(rnd.Next(AvailableOutputs.Count));            
            AvailableOutputs.Remove(op);
            return op;

        }


        public void Init(int seed)
        {
            rnd = new System.Random(seed);
            AvailableOutputs = Outputs.ToList();
        }

    }
}
