using System;
using System.Collections.Generic;
using System.Linq;
using Features.Character_Namespace.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Grounds
{
   Metal,
   Wood,
   Untagged,
   Grass,
   Glass,
   BarefootWood
}
[RequireComponent(typeof(AudioSource))]
public class FootStepBehaviour : MonoBehaviour
{
   [SerializeField] private List<GroundAudio> groundAudioList;
   [SerializeField] private ThirdPersonManager thirdPersonManager;
   private AudioSource _audioSource;

   private void Awake()
   {
      _audioSource = GetComponent<AudioSource>();
   }

   //Animations Event
   private void Step()
   {
      if(thirdPersonManager.TryGetGroundedColliders(out Collider[] floorColliders) && floorColliders.Length != 0)
      {
         print(floorColliders[0].tag);

         AudioClip clip = GetRandomClip(GetCorrectGroundAudio(floorColliders[0].tag));
         if(!floorColliders[0].CompareTag("BarefootWood")){
            _audioSource.volume = 0.013f;
         } else {
            _audioSource.volume = 0.1f;         
         }
         _audioSource.PlayOneShot(clip);
      }
   }
   
   //Animations Event for Ladder
   private void LadderStep()
   {
      AudioClip clip = GetRandomClip(GetCorrectGroundAudio("Metal"));
      _audioSource.volume = 0.013f;
      _audioSource.PlayOneShot(clip);
   }
   private AudioClip GetRandomClip(AudioClip[] audioClips)
   {
      int index = Random.Range(0, audioClips.Length - 1);
      return audioClips[index];
   }

   //Get the correct step sound given by the tag.
   //Untagged objects always results into concrete.
   private AudioClip[] GetCorrectGroundAudio(String tagName)
   {
      return groundAudioList.Where(audio => audio.type.ToString().Equals(tagName)).Select(audio => audio.audioClips).FirstOrDefault();
   }
   
   [System.Serializable]
   public class GroundAudio
   {
      public AudioClip[] audioClips;
      public Grounds type;
   }
}
