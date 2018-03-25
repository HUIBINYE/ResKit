///****************************************************************************
// * Copyright (c) 2017 snowcold
// * Copyright (c) 2017 liangxie
// * 
// * http://qframework.io
// * https://github.com/liangxiegame/QFramework
// * https://github.com/liangxiegame/QSingleton
// * https://github.com/liangxiegame/QChain
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// * 
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// * 
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// * THE SOFTWARE.
// ****************************************************************************/
//
//namespace QFramework
//{
//	using System.Collections.Generic;
//	using UnityEngine;
//	
//
//	/// <summary>
//	/// TODO:目前,不支持本地化
//	/// </summary>
//	[QMonoSingletonPath("[Audio]/AudioManager")]
//	public class AudioManager : MonoBehaviour,ISingleton
//	{
//
//		// 用来存储的Key
//		const string KEY_AUDIO_MANAGER_SOUND_ON = "KEY_AUDIO_MANAGER_SOUND_ON";
//
//		const string KEY_AUDIO_MANAGER_MUSIC_ON = "KEY_AUDIO_MANAGER_MUSIC_ON";
//		const string KEY_AUDIO_MANAGER_VOICE_ON = "KEY_AUDIO_MANAGER_VOICE_ON";
//		const string KEY_AUDIO_MANAGER_VOICE_VOLUME = "KEY_AUDIO_MANAGER_VOICE_VOLUME";
//		const string KEY_AUDIO_MANAGER_SOUND_VOLUME = "KEY_AUDIO_MANAGER_SOUND_VOLUME";
//		const string KEY_AUDIO_MANAGER_MUSIC_VOLUME = "KEY_AUDIO_MANAGER_MUSIC_VOLUME";
//
//		/// <summary>
//		/// 读取音频数据
//		/// </summary>
//		void ReadAudioSetting()
//		{
//			SoundOn = PlayerPrefs.GetInt(KEY_AUDIO_MANAGER_SOUND_ON, 1) == 1 ? true : false;
//			MusicOn = PlayerPrefs.GetInt(KEY_AUDIO_MANAGER_MUSIC_ON, 1) == 1 ? true : false;
//			VoiceOn = PlayerPrefs.GetInt(KEY_AUDIO_MANAGER_VOICE_ON, 1) == 1 ? true : false;
//
//			SoundVolume = PlayerPrefs.GetFloat(KEY_AUDIO_MANAGER_SOUND_VOLUME, 1.0f);
//			MusicVolume = PlayerPrefs.GetFloat(KEY_AUDIO_MANAGER_MUSIC_VOLUME, 1.0f);
//			VoiceVolume = PlayerPrefs.GetFloat(KEY_AUDIO_MANAGER_VOICE_VOLUME, 1.0f);
//		}
//
//		/// <summary>
//		/// 保存音频数据
//		/// </summary>
//		void SaveAudioSetting()
//		{
//			PlayerPrefs.SetInt(KEY_AUDIO_MANAGER_SOUND_ON, SoundOn == true ? 1 : 0);
//			PlayerPrefs.SetInt(KEY_AUDIO_MANAGER_MUSIC_ON, MusicOn == true ? 1 : 0);
//			PlayerPrefs.SetInt(KEY_AUDIO_MANAGER_VOICE_ON, MusicOn == true ? 1 : 0);
//			PlayerPrefs.SetFloat(KEY_AUDIO_MANAGER_SOUND_VOLUME, SoundVolume);
//			PlayerPrefs.SetFloat(KEY_AUDIO_MANAGER_MUSIC_VOLUME, MusicVolume);
//			PlayerPrefs.SetFloat(KEY_AUDIO_MANAGER_VOICE_VOLUME, VoiceVolume);
//		}
//
//		void OnApplicationQuit()
//		{
//			SaveAudioSetting();
//		}
//
//		void OnApplicationFocus(bool focus)
//		{
//			SaveAudioSetting();
//		}
//
//		void OnApplicationPause(bool pause)
//		{
//			SaveAudioSetting();
//		}
//		
//		protected AudioUnit mMainUnit;
//		protected AudioUnit mVoiceUnit;
//
//		public void OnSingletonInit()
//		{
//			Log.I("AudioManager OnSingletonInit");
//
//			SafeObjectPool<AudioUnit>.Instance.Init(10, 1);
//			mMainUnit = AudioUnit.Allocate();
//			mMainUnit.usedCache = false;
//			mVoiceUnit = AudioUnit.Allocate();
//			mVoiceUnit.usedCache = false;
//
//			CheckAudioListener();
//
//			gameObject.transform.position = Vector3.zero;
//
//			// 读取存储
//			ReadAudioSetting();
//		}
//
//		public void Dispose()
//		{
//		}
//
//		public override void Init()
//		{
//			Log.I("AudioManager.Init");
//		}
//
//		public void CheckAudioListener()
//		{
//			// 确保有一个AudioListener
//			if (FindObjectOfType<AudioListener>() == null)
//			{
//				gameObject.AddComponent<AudioListener>();
//			}
//		}
//
//		public bool SoundOn { get; private set; }
//		public bool MusicOn { get; private set; }
//		public bool VoiceOn { get; private set; }
//
//		public float SoundVolume { get; private set; }
//		public float MusicVolume { get;private set;}
//		public float VoiceVolume { get; private set; }
//
//		int mCurSourceIndex;
//
//		/// <summary>
//		/// 播放音乐
//		/// </summary>
//		void PlayMusic(AudioMusicMsg musicMsg)
//		{
//
//			if (!MusicOn && musicMsg.allowMusicOff)
//			{
//				musicMsg.onMusicBeganCallback.InvokeGracefully();
//
//				musicMsg.onMusicEndedCallback.InvokeGracefully();
//				return;
//			}
//
//			Log.I(">>>>>> Start Play Music");
//
//			// TODO: 需要按照这个顺序去 之后查一下原因
//			//需要先注册事件，然后再play
//			mMainUnit.SetOnStartListener(delegate(AudioUnit musicUnit)
//			{
//				musicMsg.onMusicBeganCallback.InvokeGracefully();
//
//				//调用完就置为null，否则应用层每注册一个而没有注销，都会调用
//				mMainUnit.SetOnStartListener(null);
//			});
//
//			mMainUnit.SetAudio(gameObject, musicMsg.MusicName, musicMsg.Loop);
//
//			mMainUnit.SetOnFinishListener(delegate(AudioUnit musicUnit)
//			{
//				musicMsg.onMusicEndedCallback.InvokeGracefully();
//
//				//调用完就置为null，否则应用层每注册一个而没有注销，都会调用
//				mMainUnit.SetOnFinishListener(null);
//			});
//		}
//
//		void SetVolume(AudioUnit audioUnit, VolumeLevel volumeLevel)
//		{
//			switch (volumeLevel)
//			{
//				case VolumeLevel.Max:
//					audioUnit.SetVolume(1.0f);
//					break;
//				case VolumeLevel.Normal:
//					audioUnit.SetVolume(0.5f);
//					break;
//				case VolumeLevel.Min:
//					audioUnit.SetVolume(0.2f);
//					break;
//			}
//		}
//
//		public AudioUnit PlaySound(bool loop = false, System.Action<AudioUnit> callBack = null, int customEventID = -1)
//		{
//			if (string.IsNullOrEmpty(name))
//			{
//				return null;
//			}
//
//			AudioUnit unit = SafeObjectPool<AudioUnit>.Instance.Allocate();
//
//			unit.SetAudio(gameObject, name, loop);
//			unit.SetOnFinishListener(callBack);
//			unit.customEventID = customEventID;
//			return unit;
//		}
//
//		/// <summary>
//		/// 停止播放音乐 
//		/// </summary>
//		void StopMusic()
//		{
//			mMainUnit.Stop();
//		}
//
//		/// <summary>
//		/// 播放音效
//		/// </summary>
//		void PlaySound(AudioSoundMsg soundMsg)
//		{
//			AudioUnit unit = SafeObjectPool<AudioUnit>.Instance.Allocate();
//
//			unit.SetOnStartListener(delegate(AudioUnit soundUnit)
//			{
//				soundMsg.onSoundBeganCallback.InvokeGracefully();
//				
//				unit.SetOnStartListener(null);
//			});
//
//			unit.SetAudio(gameObject, soundMsg.SoundName, false);
//
//			unit.SetOnFinishListener(delegate(AudioUnit soundUnit)
//			{
//				soundMsg.onSoundEndedCallback.InvokeGracefully();
//
//				unit.SetOnFinishListener(null);
//			});
//		}
//
//		/// <summary>
//		/// 播放语音
//		/// </summary>
//		void PlayVoice(AudioVoiceMsg msg)
//		{
//			mVoiceUnit.SetOnStartListener(delegate(AudioUnit musicUnit)
//			{
//				SetVolume(mMainUnit, VolumeLevel.Min);
//
//				msg.onVoiceBeganCallback.InvokeGracefully();
//
//				mVoiceUnit.SetOnStartListener(null);
//			});
//
//			mVoiceUnit.SetAudio(gameObject, msg.voiceName, msg.loop);
//
//			mVoiceUnit.SetOnFinishListener(delegate(AudioUnit musicUnit)
//			{
//				SetVolume(mMainUnit, VolumeLevel.Max);
//
//				msg.onVoiceEndedCallback.InvokeGracefully();
//
//				mVoiceUnit.SetOnFinishListener(null);
//			});
//		}
//
//		void StopVoice()
//		{
//			mVoiceUnit.Stop();
//		}
//
//		private AudioManager() {}
//
//		public static AudioManager Instance
//		{
//			get { return QMonoSingletonProperty<AudioManager>.Instance; }
//		}
//
//		//常驻内存不卸载音频资源
//		protected ResLoader mRetainResLoader;
//
//		protected List<string> mRetainAudioNames;
//
//		/// <summary>
//		/// 添加常驻音频资源，建议尽早添加，不要在用的时候再添加
//		/// </summary>
//		void AddRetainAudio(string audioName)
//		{
//			if (mRetainResLoader == null)
//				mRetainResLoader = ResLoader.Allocate();
//			if (mRetainAudioNames == null)
//				mRetainAudioNames = new List<string>();
//
//			if (!mRetainAudioNames.Contains(audioName))
//			{
//				mRetainAudioNames.Add(audioName);
//				mRetainResLoader.Add2Load(audioName);
//				mRetainResLoader.LoadAsync();
//			}
//		}
//
//		/// <summary>
//		/// 删除常驻音频资源
//		/// </summary>
//		void RemoveRetainAudio(string audioName)
//		{
//			if (mRetainAudioNames != null && mRetainAudioNames.Remove(audioName))
//			{
//				mRetainResLoader.ReleaseRes(audioName);
//			}
//		}
//	}
//}