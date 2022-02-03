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
   BarefootWood,
   BarefootUntagged
}
[RequireComponent(typeof(AudioSource))]
public class FootStepBehaviour : MonoBehaviour
{
   [SerializeField] private float soundTimeout = 0.1f;
   [SerializeField] private List<GroundAudio> groundAudioList;
   [SerializeField] private ThirdPersonManager thirdPersonManager;
   private AudioSource _audioSource;
   private float _soundTimeoutDelta;
   private bool _barefoot;

   private void Awake()
   {
      _audioSource = GetComponent<AudioSource>();
      _barefoot = true;
      _soundTimeoutDelta = soundTimeout;
   }

   private void Update()
   {
      _soundTimeoutDelta -= Time.deltaTime;
   }

   //Animations Event
   private void Step()
   {
      if(thirdPersonManager.TryGetGroundedColliders(out Collider[] floorColliders) && floorColliders.Length != 0 && _soundTimeoutDelta <= 0)
      {
         string groundTag = floorColliders[0].tag;
         Debug.Log(groundTag);

         if (_barefoot && floorColliders[0].tag.Equals("Untagged"))
         {
            groundTag = Grounds.BarefootUntagged.ToString();
         }
         SetAudioVolume(groundTag);
         AudioClip clip = GetRandomClip(GetCorrectGroundAudio(groundTag));
         _audioSource.PlayOneShot(clip);
         _soundTimeoutDelta = soundTimeout;
      }
   }
   
   //Animations Event for Ladder
   private void LadderStep()
   {
      AudioClip clip = GetRandomClip(GetCorrectGroundAudio("Metal"));
      _audioSource.volume = 0.1f;
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

   public void PutOnShoes()
   {
      _barefoot = false;
   }

   private void SetAudioVolume(String groundTag)
   {
      if (String.Equals(groundTag, Grounds.BarefootUntagged.ToString()) || String.Equals(groundTag, Grounds.BarefootWood.ToString()))
      {
         _audioSource.volume = 0.3f;
      }else
      {
         _audioSource.volume = 0.1f;
      }
   }
   
   [Serializable]
   public class GroundAudio
   {
      public AudioClip[] audioClips;
      public Grounds type;
   }
}
