using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class WorldBuilder : MonoBehaviour
    {
        public GameObject[] Modules;        
        public int Iterations;
        /// <summary>
        /// Random seed for debugging. 
        /// </summary>
        public int Seed;    
        System.Random rnd;

        List<GameObject> createdModules, availableModules;
        void Start()
        {
            rnd = new System.Random(Seed);
            
            BuildWorld();
        }

        void BuildWorld()
        {

            createdModules = new List<GameObject>();            
            var initialModule = GameObject.Instantiate(Modules.ElementAt(rnd.Next(Modules.Count())));
            initialModule.GetComponent<Module>().Init(Seed);            
            createdModules.Add(initialModule);
            availableModules = createdModules;

            for (int i = 0; i < Iterations; i++)
            {
                var module = availableModules.ElementAt(rnd.Next(availableModules.Count));
                var targetPoint = module.GetComponent<Module>().GetOutput();

                //Shuffle and try every blocks to fit 
                var shuffledBlocks = Modules.OrderBy(d => rnd.Next()).ToArray();
                foreach (var sBlock in shuffledBlocks)
                {
                    var candidate = GameObject.Instantiate(sBlock);  
                    candidate.GetComponent<Module>().Init(Seed);
                    candidate.gameObject.transform.position = targetPoint.transform.position;
                    candidate.transform.LookAt(targetPoint.transform.position + targetPoint.transform.forward);


                    //Check if there is an any overlapping
                    var bound = candidate.GetComponent<BoxCollider>().bounds;
                    var isSafe = true;
                    foreach (var item in createdModules)
                    {
                        if (bound.Intersects(item.GetComponent<BoxCollider>().bounds))
                        {
                            //Try another module
                            GameObject.Destroy(candidate);
                            isSafe = false;
                            break;
                        }
                    }

                    if (isSafe)
                    {
                        //Module connected safely
                        createdModules.Add(candidate);
                        break;
                    }
                }

                availableModules = createdModules.Where(d => d.GetComponent<Module>().AvailableOutputs.Any()).ToList();
                if (!availableModules.Any())
                {
                    //No availabel output on any modules. Stop the proccess
                    break;
                }
            }

            foreach (var item in createdModules)
            {
            
                //Disable overlap test colliders
                item.GetComponent<BoxCollider>().enabled = false;
            }

        }
                
    }


}
