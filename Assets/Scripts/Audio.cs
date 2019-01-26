﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour {

	private static Dictionary<string, AudioClip> tracks;
	private static AudioSource sfxSource;
	private static AudioSource musicSource;

	public AudioSource initialSfx;
	public AudioSource initialMusic;

	// Use this for initialization
	void Awake() {
		if (tracks == null) {
			tracks = new Dictionary<string, AudioClip>();
			tracks.Add("Victory", Resources.Load<AudioClip>("Audio/Victory"));
		}

		sfxSource = initialSfx;
		musicSource = initialMusic;
	}

	public static void playSfx(string id) {
		playSound(id, false, false);
	}

	public static void playMusic(string id) {
		playSound(id, true, true);
	}

	private static void playSound(string id, bool isMusic, bool loop) {
		AudioClip clip = tracks[id];
		if (clip == null) {
			throw new UnityException("The requested audio track has not yet been imported in the Audio.cs file");
		}

		AudioSource source = isMusic ? musicSource : sfxSource;
		source.clip = clip;
		source.loop = loop;
		source.Play();
	}
}
