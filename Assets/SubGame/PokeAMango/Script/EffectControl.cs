using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{
    public class EffectControl : MonoBehaviour
    {
        [SerializeField]
        Volume _volume;
        DepthOfField _dof;

        private void Awake()
        {
            VolumeProfile profile = _volume.sharedProfile;

            profile.TryGet<DepthOfField>(out _dof);
            _dof.active = false;
        }




        public void DofOn()
        {
            VolumeProfile profile = _volume.sharedProfile;

            profile.TryGet<DepthOfField>(out _dof);
            _dof.active = true;
        }

        public void DofOff()
        {
            VolumeProfile profile = _volume.sharedProfile;

            profile.TryGet<DepthOfField>(out _dof);
            _dof.active = false;
        }
    }
}
