using System.Collections.Generic;
using UnityEngine;

public interface IAudioSourceGuideable { GuideItem GetData(AudioSource audioSource); }

public class GuideDatabaseAudioSource : IAudioSourceGuideable
{
    public virtual GuideItem GetData(AudioSource audioSource) => default;
    public static readonly Dictionary<string, GuideItem> AudioSourceGuides = new Dictionary<string, GuideItem>
    {
        ["AudioSource_Clip"] = GuideDatabaseHelpers.Item("Audio Clip", "Menentukan file audio yang akan diputar.", "AudioSource tidak memiliki Audio Clip.", "AudioSource memiliki Audio Clip."),
        ["AudioSource_Output"] = GuideDatabaseHelpers.Item("Output", "Menentukan Audio Mixer Group tujuan suara.", "Suara dikirim ke output utama.", "Suara diproses oleh Audio Mixer Group."),
        ["AudioSource_Mute"] = GuideDatabaseHelpers.Item("Mute", "Membisukan suara AudioSource.", "Suara dapat terdengar.", "Suara dibisukan."),
        ["AudioSource_PlayOnAwake"] = GuideDatabaseHelpers.Item("Play On Awake", "Menentukan apakah suara diputar saat GameObject aktif.", "AudioSource menunggu perintah Play.", "AudioSource diputar otomatis."),
        ["AudioSource_Loop"] = GuideDatabaseHelpers.Item("Loop", "Menentukan apakah Audio Clip diputar berulang.", "Audio Clip diputar satu kali.", "Audio Clip diputar berulang."),
        ["AudioSource_Volume"] = GuideDatabaseHelpers.Item("Volume", "Mengatur tingkat keras suara AudioSource.", "Suara tidak terdengar.", "Suara menggunakan volume sedang.", "Suara menggunakan volume penuh."),
        ["AudioSource_Pitch"] = GuideDatabaseHelpers.Item("Pitch", "Mengatur kecepatan dan tinggi rendah nada audio.", "Audio lambat dan bernada rendah.", "Audio menggunakan pitch normal.", "Audio cepat dan bernada tinggi."),
        ["AudioSource_SpatialBlend"] = GuideDatabaseHelpers.Item("Spatial Blend", "Menentukan perpaduan suara 2D dan 3D.", "Suara bersifat 2D.", "Suara menggunakan perpaduan 2D dan 3D.", "Suara bersifat 3D."),
        ["AudioSource_Priority"] = GuideDatabaseHelpers.Item("Priority", "Menentukan prioritas AudioSource.", "AudioSource berprioritas tinggi.", "AudioSource berprioritas normal.", "AudioSource berprioritas rendah."),
        ["AudioSource_Bypass"] = GuideDatabaseHelpers.Item("Bypass Effects", "Menentukan apakah AudioSource melewati efek audio.", "Efek audio diproses.", "Efek audio dilewati."),
        ["AudioSource_3D"] = GuideDatabaseHelpers.Item("3D Sound Settings", "Mengatur perilaku suara berdasarkan jarak dan posisi.", "Audio menggunakan pengaturan 3D standar.", "Audio memiliki pengaruh 3D aktif.")
    };
}

public sealed class AudioSourceClipGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Clip"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && c.clip != null ? 1 : 0); }
public sealed class AudioSourceOutputGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Output"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && c.outputAudioMixerGroup != null ? 1 : 0); }
public sealed class AudioSourceMuteGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Mute"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && c.mute ? 1 : 0); }
public sealed class AudioSourcePlayOnAwakeGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_PlayOnAwake"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && c.playOnAwake ? 1 : 0); }
public sealed class AudioSourceLoopGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Loop"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && c.loop ? 1 : 0); }
public sealed class AudioSourceVolumeGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Volume"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c == null ? 0 : GuideDatabaseHelpers.Range(c.volume, 0f, 1f)); }
public sealed class AudioSourcePitchGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Pitch"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c == null ? 1 : GuideDatabaseHelpers.Range(c.pitch, 0.5f, 1.5f)); }
public sealed class AudioSourceSpatialBlendGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_SpatialBlend"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c == null ? 0 : GuideDatabaseHelpers.Range(c.spatialBlend, 0f, 1f)); }
public sealed class AudioSourcePriorityGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Priority"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c == null ? 1 : GuideDatabaseHelpers.Range(c.priority, 0f, 128f)); }
public sealed class AudioSourceBypassGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_Bypass"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && (c.bypassEffects || c.bypassListenerEffects || c.bypassReverbZones) ? 1 : 0); }
public sealed class AudioSource3DSettingsGuide : GuideDatabaseAudioSource { private readonly string key = "AudioSource_3D"; public override GuideItem GetData(AudioSource c) => GuideDatabaseHelpers.State(AudioSourceGuides[key], c != null && c.spatialBlend > 0f ? 1 : 0); }
