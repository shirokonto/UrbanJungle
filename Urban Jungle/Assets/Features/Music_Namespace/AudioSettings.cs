using DataStructures.Variables;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Features.Audio_Namespace.Logic
{
    public class AudioSettings : MonoBehaviour
    {
        [Header("Volume")]
        [SerializeField] private AudioMixer volumeMixer;
        [SerializeField] private FloatVariable volume;
        [SerializeField] private Slider musicSlider;
        
        /*
         * By raising the SetVolume() method inside of the Slider Unity Event, it will also get called when starting the game.
         * To prevent that the floatVariable is set to the default Slider.Value when starting the game, there is a 'false' boolean needed.
         * gameobject.SetActive() doesn't work here cause it is true when starting the game.
         */
        
        private bool valueIsChangeable;
    
        private void Awake()
        {
            if (PlayerPrefs.HasKey("Volume"))
            {
                volume.Set(PlayerPrefs.GetFloat("Volume"));
            }
            
            valueIsChangeable = false;
            UpdateVolume();
        }

        private void OnEnable()
        {
            valueIsChangeable = true;
            UpdateVolume();
        }

        private void OnDisable()
        {
            valueIsChangeable = false;
        }

        private void UpdateVolume()
        {
            //use the float of the scriptable object to override the current volume
            float dbMusic = volume.Get() != 0 ? Mathf.Log10(volume.Get()) * 20 : -80f;
        
            volumeMixer.SetFloat("Vol", dbMusic);
        
            musicSlider.value = volume.Get();
        }

        public void SetVolume()
        {
            if (!valueIsChangeable)
            {
                return;
            }
        
            volume.Set(musicSlider.value);
        
            //use the float of the slider to override the current volume
            float dbMusic = volume.Get() != 0 ? Mathf.Log10(volume.Get()) * 20 : -80f;
            volumeMixer.SetFloat("Vol", dbMusic);
        
            PlayerPrefs.SetFloat("Volume", volume.Get());
        }
    }
}
