using UnityEngine;

/// <summary>
/// 풀에서 사용되는 단발성 사운드 재생 전용 컴포넌트
/// - AudioSource로 SFX 재생
/// - 볼륨 계산은 SoundManager에서 완료된 값만 전달받음
/// - 재생 종료 시 자동으로 풀 반환
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;
    private PooledObject pooledObject;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pooledObject = GetComponent<PooledObject>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 기본 2D
    }

    /// <summary>
    /// 사운드 재생
    /// (볼륨은 SoundManager에서 최종 계산된 값만 전달됨)
    /// </summary>
    public void Play(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            ReturnToPool();
            return;
        }

        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.Play();

        CancelInvoke();
        Invoke(nameof(ReturnToPool), clip.length);
    }

    private void ReturnToPool()
    {
        if (pooledObject != null)
            pooledObject.ReturnToPool();
        else
            gameObject.SetActive(false);
    }
}
