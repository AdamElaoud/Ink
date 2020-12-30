using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public Sound[] sounds;
    bool playing;
    Queue<string> queue = new Queue<string>();
    string current;
    

	void Awake() {
        // insure only 1 copy of AudioManager
        if (instance == null) {
		    instance = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
            return;
        }		

		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}

    void Start() {
        playing = false;
        AddToQueue("Cool Vibes");
        AddToQueue("Dances and Dames");
        AddToQueue("Just As Soon");
        AddToQueue("Walking Along");
    }

    void Update() {
        if (!playing) {
            current = queue.Dequeue();
            Play(current);
            queue.Enqueue(current);
            playing = true;
        }

        if (!Playing(current))
            playing = false;
    }

	public void Play(string soundName) {
		Sound sound = Array.Find(sounds, s => s.name == soundName);

		if (sound == null) {
			Debug.LogWarning("Sound: " + soundName + " not found!");
			return;
		}

		sound.source.volume = sound.volume;
		sound.source.pitch = sound.pitch;

		sound.source.Play();
        print("Now Playing: " + soundName);
	}

    public void AddToQueue(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);

        if (sound == null) {
            Debug.LogWarning("Can't add to queue! Sound: " + soundName + " not found!");
            return;
        }

        queue.Enqueue(soundName);
    }

    bool Playing(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);

        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not playing. It was not found!");
            return false;
        }

        return sound.source.isPlaying;
    }
}
