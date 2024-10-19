using System.Collections.Generic;
using UnityEngine;

namespace CorePublic.Helpers
{
    public class VolumeFiller
    {
        private readonly Bounds _encapsualtor;
        private readonly List<Bounds> _filledBounds;
        private readonly int _trialCount;

        public VolumeFiller(Bounds encapsulator, int trialCount)
        {
            _encapsualtor = encapsulator;
            _filledBounds = new List<Bounds>();
            _trialCount = trialCount;
        }

        public bool FindPositionToBounds(Bounds targetBounds, out Vector3 position)
        {
            position=Vector3.zero;
        
            for (int i = 0; i < _trialCount; i++)
            {
                Bounds shiftedBounds = targetBounds;
                shiftedBounds.center = Utils.RandomPointInBounds(_encapsualtor);
            
                if (BoundsIsEncapsulated(_encapsualtor,shiftedBounds))
                {
                    //true if generated bound instersected with another filled bound
                    bool failed = false;
                
                    foreach (Bounds bounds in _filledBounds)
                    {
                        if (shiftedBounds.Intersects(bounds))
                        {
                            failed = true;
                            break;
                        }
                    
                    }
                
                    if(failed) continue;
                    position = shiftedBounds.center;
                    _filledBounds.Add(shiftedBounds);
                    return true;
                }
            }
            Debug.LogError("Couldnt generate position in scope of trials");
        
            return false;
        } 
    
        static bool BoundsIsEncapsulated(Bounds encapsulator, Bounds encapsulated)
        {
            return encapsulator.Contains(encapsulated.min) && encapsulator.Contains(encapsulated.max);
        }
    }
}